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
        SELECT b.BookID, b.Title, b.Author, b.Genre, b.Summary, b.IsAvailable, ul.GiveBackDate
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
                    Convert.ToBoolean(reader["IsAvailable"]),
                    reader["GiveBackDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["GiveBackDate"])
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

        /// <summary>
        /// Verlängert das Rückgabedatum eines Buches in der UserBookList um eine angegebene Anzahl Tage.
        /// </summary>
        /// <param name="userId">Die ID des Users.</param>
        /// <param name="bookId">Die ID des Buches.</param>
        /// <param name="daysToExtend">Anzahl der Tage zur Verlängerung (1 bis 4).</param>
        public void ExtendGiveBackDate(int userId, int bookId, int daysToExtend)
        {
            if (daysToExtend < 1 || daysToExtend > 4)
                throw new ArgumentOutOfRangeException(nameof(daysToExtend), "Die Verlängerung muss zwischen 1 und 4 Tagen liegen.");

            using var conn = Database.GetConnection();
            using var cmdGetDate = new SQLiteCommand("SELECT GiveBackDate FROM UserBookList WHERE UserID = @UserID AND BookID = @BookID", conn);
            cmdGetDate.Parameters.AddWithValue("@UserID", userId);
            cmdGetDate.Parameters.AddWithValue("@BookID", bookId);

            var result = cmdGetDate.ExecuteScalar();
            if (result == null || result == DBNull.Value)
                throw new InvalidOperationException("Es existiert kein Rückgabedatum für dieses Buch in der Liste des Users.");

            var currentGiveBackDate = Convert.ToDateTime(result);
            var newGiveBackDate = currentGiveBackDate.AddDays(daysToExtend);

            using var cmdUpdate = new SQLiteCommand(@"UPDATE UserBookList 
                                              SET GiveBackDate = @NewGiveBackDate
                                              WHERE UserID = @UserID AND BookID = @BookID", conn);
            cmdUpdate.Parameters.AddWithValue("@NewGiveBackDate", newGiveBackDate.ToString("yyyy-MM-dd"));
            cmdUpdate.Parameters.AddWithValue("@UserID", userId);
            cmdUpdate.Parameters.AddWithValue("@BookID", bookId);

            cmdUpdate.ExecuteNonQuery();
        }


    }
}
