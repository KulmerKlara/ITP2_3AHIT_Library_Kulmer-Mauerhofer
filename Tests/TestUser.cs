using Library.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SQLite;

namespace Tests
{
    /// <summary>
    /// Testklasse für <see cref="UserRepository"/>.
    /// </summary>
    [TestClass]
    public class UserRepositoryTests
    {
        private SQLiteConnection _connection;
        private UserRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SQLiteConnection("Data Source=:memory:");
            _connection.Open();

            Database.OverrideConnection(_connection);

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Users (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    Email TEXT UNIQUE,
                    Password TEXT,
                    Role TEXT,
                    Phone TEXT
                );
            ";
            cmd.ExecuteNonQuery();

            _repository = new UserRepository();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
        }

        [TestMethod]
        public void AddUser_ShouldInsertUserIntoDatabase()
        {
            var user = new User(0, "Max Mustermann", "max@example.com", "password123", "1234567890", "User");

            _repository.AddUser(user);

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            cmd.Parameters.AddWithValue("@Email", user.Email);
            var count = Convert.ToInt32(cmd.ExecuteScalar());

            Assert.AreEqual(1, count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddUser_NullUser_ShouldThrowException()
        {
            _repository.AddUser(null!);
        }

        [TestMethod]
        public void GetUserByEmail_ShouldReturnUser_WhenUserExists()
        {
            InsertUser("Anna", "anna@example.com", "pw", "User", "555-1111");

            var user = _repository.GetUserByEmail("anna@example.com");

            Assert.IsNotNull(user);
            Assert.AreEqual("Anna", user!.Name);
            Assert.AreEqual("anna@example.com", user.Email);
        }

        [TestMethod]
        public void GetUserByEmail_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var user = _repository.GetUserByEmail("nonexistent@example.com");
            Assert.IsNull(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUserByEmail_EmptyEmail_ShouldThrowException()
        {
            _repository.GetUserByEmail("");
        }

        [TestMethod]
        public void GetUserByName_ShouldReturnUser_WhenUserExists()
        {
            InsertUser("Bernd", "bernd@example.com", "pw", "Admin", "555-2222");

            var user = _repository.GetUserByName("Bernd");

            Assert.IsNotNull(user);
            Assert.AreEqual("Bernd", user!.Name);
            Assert.AreEqual("bernd@example.com", user.Email);
        }

        [TestMethod]
        public void GetUserByName_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var user = _repository.GetUserByName("Nonexistent");
            Assert.IsNull(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUserByName_EmptyName_ShouldThrowException()
        {
            _repository.GetUserByName("");
        }

        private void InsertUser(string name, string email, string password, string role, string phone)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Users (Name, Email, Password, Role, Phone)
                VALUES (@Name, @Email, @Password, @Role, @Phone)";
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Role", role);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.ExecuteNonQuery();
        }
    }
}

