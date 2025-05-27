using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Library.Data
{
    /// <summary>
    /// Repository for managing the relationship between users and their book lists.
    /// </summary>
    public class UserBookListRepository
    {
        /// <summary>
        /// Adds a book to a user's book list. If the entry already exists, it is ignored.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="bookId">The ID of the book.</param>
        public void AddBookToUserList(int userId, int bookId, DateTime giveBackDate)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn)
            {
                CommandText = @"INSERT OR IGNORE INTO UserBookList (UserID, BookID, GiveBackDate) 
                                VALUES (@UserID, @BookID, @GiveBackDate)"
            };
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@BookID", bookId);
            cmd.Parameters.AddWithValue("@GiveBackDate", giveBackDate.ToString("yyyy-MM-dd"));
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes a book from a user's book list.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="bookId">The ID of the book.</param>
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

        /// <summary>
        /// Retrieves the list of books associated with a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of books belonging to the user.</returns>
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

        public bool IsBookAlreadyInUserList(int userId, int bookId)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand("SELECT COUNT(1) FROM UserBookList WHERE UserID = @UserID AND BookID = @BookID", conn);
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@BookID", bookId);

            var count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }

    }
}
