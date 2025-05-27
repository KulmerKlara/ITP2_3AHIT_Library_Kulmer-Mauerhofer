using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Library.Data
{
    public class BookRepository
    {
        public void AddBook(Book book)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn)
            {
                CommandText = @"INSERT INTO Books (Title, Author, Genre, Summary, IsAvailable, GiveBackDate) 
                                VALUES (@Title, @Author, @Genre, @Summary, @IsAvailable, @GiveBackDate)"
            };

            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Genre", book.Genre);
            cmd.Parameters.AddWithValue("@Summary", book.Summary);
            cmd.Parameters.AddWithValue("@IsAvailable", book.IsAvailable ? 1 : 0);
            cmd.Parameters.AddWithValue("@GiveBackDate", book.GiveBackDate?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public List<Book> GetAllBooks()
        {
            var books = new List<Book>();
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand("SELECT * FROM Books", conn);
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

        public List<Book> GetBooksByGenre(string genre)
        {
            var books = new List<Book>();
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand("SELECT * FROM Books WHERE Genre = @Genre", conn);
            cmd.Parameters.AddWithValue("@Genre", genre);
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

        public static List<Book> SearchBooks(string searchTerm)
        {
            var results = new List<Book>();
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn);

            cmd.CommandText = @"SELECT BookID, Title, Author, Genre, Summary, IsAvailable, GiveBackDate
                                FROM Books 
                                WHERE Title LIKE @searchTerm 
                                   OR Author LIKE @searchTerm 
                                   OR Genre LIKE @searchTerm";
            cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new Book(
                    Convert.ToInt32(reader["BookID"]),
                    reader["Title"].ToString(),
                    reader["Author"].ToString(),
                    reader["Genre"].ToString(),
                    reader["Summary"].ToString(),
                    Convert.ToBoolean(reader["IsAvailable"]),
                    reader["GiveBackDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["GiveBackDate"])
                ));
            }
            return results;
        }

        public void UpdateBook(Book book)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn);

            cmd.CommandText = @"UPDATE Books 
                                SET Title = @Title, Author = @Author, Genre = @Genre, Summary = @Summary, 
                                    IsAvailable = @IsAvailable, GiveBackDate = @GiveBackDate 
                                WHERE BookID = @BookID";
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Genre", book.Genre);
            cmd.Parameters.AddWithValue("@Summary", book.Summary);
            cmd.Parameters.AddWithValue("@IsAvailable", book.IsAvailable ? 1 : 0);
            cmd.Parameters.AddWithValue("@GiveBackDate", book.GiveBackDate?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BookID", book.BookId);

            cmd.ExecuteNonQuery();
        }
        public void Availability(Book updatedBook)
        {
            var books = GetAllBooks();
            var existingBook = books.FirstOrDefault(b => b.BookId == updatedBook.BookId);
            if (existingBook != null)
            {
                existingBook.Title = updatedBook.Title;
                existingBook.Author = updatedBook.Author;
                existingBook.Genre = updatedBook.Genre;
                existingBook.Summary = updatedBook.Summary;
                existingBook.IsAvailable = updatedBook.IsAvailable;
                // Bei Datenbankanbindung: Kontext speichern
            }
        }

        public void DeleteBook(int bookId)
        {
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn);

            cmd.CommandText = "DELETE FROM Books WHERE BookID = @BookID;";
            cmd.Parameters.AddWithValue("@BookID", bookId);

            cmd.ExecuteNonQuery();
        }

        public List<Book> GetBorrowedBooks()
        {
            var books = new List<Book>();
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(@"
        SELECT b.BookID, b.Title, b.Author, b.Genre, b.Summary, b.IsAvailable, l.DueDate
        FROM Books b
        INNER JOIN Loans l ON b.BookID = l.BookID
        WHERE l.ReturnedDate IS NULL
    ", conn);
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
                    reader["DueDate"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["DueDate"])
                ));
            }

            return books;
        }


        public List<Book> GetAvailableBooks()
        {
            var books = new List<Book>();
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(@"
        SELECT * FROM Books 
        WHERE BookID NOT IN (
            SELECT BookID FROM Loans WHERE ReturnedDate IS NULL
        )
    ", conn);
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

    }
}

