using System;
using System.Data.SQLite;

namespace Library.Data
{
    public class UserRepository
    {
        public void AddUser(User user)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn)
            {
                CommandText = @"INSERT INTO Users (Name, Email, Password, Role, Phone) 
                                VALUES (@Name, @Email, @Password, @Role, @Phone)"
            };

            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", user.Password);
            cmd.Parameters.AddWithValue("@Role", user.Role);
            cmd.Parameters.AddWithValue("@Phone", user.Phone);
            cmd.ExecuteNonQuery();
        }

        public User? GetUserByEmail(string email)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand("SELECT * FROM Users WHERE Email = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", email);
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
