using System;
using System.Data.SQLite;

namespace Library.Data
{
    /// <summary>
    /// Repository class for managing user-related database operations.
    /// </summary>
    public class UserRepository
    {
        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user object to insert.</param>
        public void AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn)
            {
                CommandText = @"INSERT INTO Users (Name, Email, Password, Role, Phone) 
                                VALUES (@Name, @Email, @Password, @Role, @Phone)"
            };

            // Set parameters to prevent SQL injection
            cmd.Parameters.AddWithValue("@Name", user.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("@Email", user.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("@Password", user.Password ?? string.Empty);
            cmd.Parameters.AddWithValue("@Role", user.Role ?? string.Empty);
            cmd.Parameters.AddWithValue("@Phone", user.Phone ?? string.Empty);

            cmd.ExecuteNonQuery(); // Execute the insert command
        }

        /// <summary>
        /// Retrieves a user from the database by their email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>A User object if found; otherwise, null.</returns>
        public User? GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand("SELECT * FROM Users WHERE Email = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                // Map database fields to a User object
                return new User(
                    Convert.ToInt32(reader["UserID"]),
                    reader["Name"]?.ToString() ?? string.Empty,
                    reader["Email"]?.ToString() ?? string.Empty,
                    reader["Password"]?.ToString() ?? string.Empty,
                    reader["Phone"]?.ToString() ?? string.Empty,
                    reader["Role"]?.ToString() ?? string.Empty
                );
            }

            return null; // No matching user found
        }

        /// <summary>
        /// Retrieves a user from the database by their username (Name field).
        /// </summary>
        /// <param name="name">The username to search for.</param>
        /// <returns>A User object if found; otherwise, null.</returns>
        public User? GetUserByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));

            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand("SELECT * FROM Users WHERE Name = @Name", conn);
            cmd.Parameters.AddWithValue("@Name", name);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new User(
                    Convert.ToInt32(reader["UserID"]),
                    reader["Name"]?.ToString() ?? string.Empty,
                    reader["Email"]?.ToString() ?? string.Empty,
                    reader["Password"]?.ToString() ?? string.Empty,
                    reader["Phone"]?.ToString() ?? string.Empty,
                    reader["Role"]?.ToString() ?? string.Empty
                );
            }

            return null;
        }
    }
}
