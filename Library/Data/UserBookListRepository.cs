using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Library.Data
{
    public class UserBookListRepository
    {
        public void AddBookToUserList(int userId, int bookId)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn)
            {
                CommandText = @"INSERT OR IGNORE INTO UserBookList (UserID, BookID) 
                                VALUES (@UserID, @BookID)"
            };
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@BookID", bookId);
            cmd.ExecuteNonQuery();
        }

        public void RemoveBookFromUserList(int userId, int bookId)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn)
            {
                CommandText = @"DELETE FROM UserBookList 
                                WHERE UserID = @UserID AND BookID = @BookID"
            };
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@BookID", bookId);
            cmd.ExecuteNonQuery();
        }

        public List<Book> GetUserBookList(int userId)
        {
            var books = new List<Book>();
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(@"
                SELECT b.BookID, b.Title, b.Author, b.Genre, b.Summary, b.IsAvailable
                FROM Books b
                JOIN UserBookList ul ON b.BookID = ul.BookID
                WHERE ul.UserID = @UserID", conn);
            cmd.Parameters.AddWithValue("@UserID", userId);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                books.Add(new Book(
                    Convert.ToInt32(reader["BookID"]),
                    reader["Title"].ToString(),
                    reader["Author"].ToString(),
                    reader["Genre"].ToString(),
                    reader["Summary"].ToString(),
                    Convert.ToBoolean(reader["IsAvailable"])
                ));
            }

            return books;
        }
    }
}
