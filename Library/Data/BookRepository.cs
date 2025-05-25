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
                CommandText = @"INSERT INTO Books (Title, Author, Genre, Summary, IsAvailable) 
                                VALUES (@Title, @Author, @Genre, @Summary, @IsAvailable)"
            };

            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Genre", book.Genre);
            cmd.Parameters.AddWithValue("@Summary", book.Summary);
            cmd.Parameters.AddWithValue("@IsAvailable", book.IsAvailable ? 1 : 0);
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
                    Convert.ToBoolean(reader["IsAvailable"])
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
                    Convert.ToBoolean(reader["IsAvailable"])
                ));
            }

            return books;
        }


        public static List<Book> SearchBooks(string searchTerm)
        {
            var results = new List<Book>();
            using var conn = Database.GetConnection();
            using var cmd = new SQLiteCommand(conn);

            // Suche mit LIKE und Platzhaltern, case insensitive für Titel, Autor und Genre
            cmd.CommandText = @"SELECT BookID, Title, Author, Genre, Summary, IsAvailable 
                    FROM Books 
                    WHERE Title LIKE @searchTerm 
                       OR Author LIKE @searchTerm 
                       OR Genre LIKE @searchTerm";
            cmd.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new Book(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.IsDBNull(3) ? null : reader.GetString(3),
                    reader.IsDBNull(4) ? null : reader.GetString(4),
                    reader.GetBoolean(5)
                ));
            }

            return results;
        }
        public void UpdateBook(Book updatedBook)
        {
            var books = new List<Book>();
            var index = books.FindIndex(b => b.BookId == updatedBook.BookId);
            if (index != -1)
            {
                books[index] = updatedBook;
            }
        }

    }
}
