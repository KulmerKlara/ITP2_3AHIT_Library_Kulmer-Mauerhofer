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
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn)
            {
                CommandText = @"INSERT INTO Users (Name, Email, Password, Role, Phone) 
                                VALUES (@Name, @Email, @Password, @Role, @Phone)"
            };

            // Set parameters to prevent SQL injection
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Role", user.Role);
            cmd.Parameters.AddWithValue("@Phone", user.Phone);

            cmd.ExecuteNonQuery(); // Execute the insert command
        }

        /// <summary>
        /// Retrieves a user from the database by their email address.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>A User object if found; otherwise, null.</returns>
        public User? GetUserByEmail(string email)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand("SELECT * FROM Users WHERE Email = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                // Map database fields to a User object
                return new User(
                    Convert.ToInt32(reader["UserID"]),
                    reader["Name"].ToString(),
                    reader["Email"].ToString(),
                    reader["Password"].ToString(),
                    reader["Phone"].ToString(),
                    reader["Role"].ToString()
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
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand("SELECT * FROM Users WHERE Name = @Name", conn);
            cmd.Parameters.AddWithValue("@Name", name);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new User(
                    Convert.ToInt32(reader["UserID"]),
                    reader["Name"].ToString(),
                    reader["Email"].ToString(),
                    reader["Password"].ToString(),
                    reader["Phone"].ToString(),
                    reader["Role"].ToString()
                );
            }

            return null;
        }
    }
}
