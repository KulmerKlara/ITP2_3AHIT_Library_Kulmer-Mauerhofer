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


    }
}
