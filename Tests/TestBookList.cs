using Library.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class BookRepositoryTests
    {
        private SQLiteConnection _connection;
        private BookRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SQLiteConnection("Data Source=:memory:");
            _connection.Open();
            Database.OverrideConnection(_connection);

            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Books (
                    BookID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT,
                    Author TEXT,
                    Genre TEXT,
                    Summary TEXT,
                    IsAvailable INTEGER,
                    GiveBackDate DATE
                );
                CREATE TABLE Loans (
                    LoanID INTEGER PRIMARY KEY AUTOINCREMENT,
                    BookID INTEGER,
                    UserID INTEGER,
                    LoanDate DATE,
                    DueDate DATE,
                    ReturnedDate DATE,
                    Extended BOOLEAN
                );
            ";
            cmd.ExecuteNonQuery();

            _repository = new BookRepository();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
        }

        [TestMethod]
        public void AddBook_ShouldInsertIntoDatabase()
        {
            var book = new Book(0, "Test", "Autor", "Genre", "Zusammenfassung", true, null);
            _repository.AddBook(book);

            var books = _repository.GetAllBooks();
            Assert.AreEqual(1, books.Count);
            Assert.AreEqual("Test", books[0].Title);
        }

        [TestMethod]
        public void GetAllBooks_ShouldReturnAllInsertedBooks()
        {
            AddBook("Buch 1");
            AddBook("Buch 2");

            var books = _repository.GetAllBooks();
            Assert.AreEqual(2, books.Count);
        }

        [TestMethod]
        public void GetBooksByGenre_ShouldFilterCorrectly()
        {
            AddBook("Krimi 1", genre: "Krimi");
            AddBook("Fantasy 1", genre: "Fantasy");

            var krimis = _repository.GetBooksByGenre("Krimi");
            Assert.AreEqual(1, krimis.Count);
            Assert.AreEqual("Krimi 1", krimis[0].Title);
        }

        [TestMethod]
        public void SearchBooks_ShouldReturnMatchingResults()
        {
            AddBook("Harry Potter", "J.K. Rowling", "Fantasy");
            AddBook("Die Zauberflöte", "Mozart", "Oper");

            var results = BookRepository.SearchBooks("Harry");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Harry Potter", results[0].Title);
        }

        [TestMethod]
        public void UpdateBook_ShouldModifyExistingRecord()
        {
            var book = AddBook("Original");
            book.Title = "Updated";
            _repository.UpdateBook(book);

            var updated = _repository.GetAllBooks().FirstOrDefault();
            Assert.AreEqual("Updated", updated.Title);
        }

        [TestMethod]
        public void DeleteBook_ShouldRemoveBook()
        {
            var book = AddBook("Zum Löschen");
            _repository.DeleteBook(book.BookId);

            var books = _repository.GetAllBooks();
            Assert.AreEqual(0, books.Count);
        }

        [TestMethod]
        public void GetBorrowedBooks_ShouldReturnOnlyLoanedBooks()
        {
            var book = AddBook("Geliehenes Buch");
            AddLoan(book.BookId, 1);

            var borrowed = _repository.GetBorrowedBooks();
            Assert.AreEqual(1, borrowed.Count);
            Assert.AreEqual("Geliehenes Buch", borrowed[0].Title);
        }

        [TestMethod]
        public void GetAvailableBooks_ShouldReturnBooksNotLoaned()
        {
            var availableBook = AddBook("Verfügbar");
            var borrowedBook = AddBook("Nicht verfügbar");
            AddLoan(borrowedBook.BookId, 1);

            var available = _repository.GetAvailableBooks();
            Assert.AreEqual(1, available.Count);
            Assert.AreEqual("Verfügbar", available[0].Title);
        }

        // Hilfsmethoden
        private Book AddBook(string title, string author = "Autor", string genre = "Genre", string summary = "Zusammenfassung", bool isAvailable = true)
        {
            var book = new Book(0, title, author, genre, summary, isAvailable, null);
            _repository.AddBook(book);
            return _repository.GetAllBooks().FirstOrDefault(b => b.Title == title);
        }

        private void AddLoan(int bookId, int userId)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Loans (BookID, UserID, LoanDate, DueDate, Extended)
                VALUES (@BookID, @UserID, DATE('now'), DATE('now', '+14 days'), 0)";
            cmd.Parameters.AddWithValue("@BookID", bookId);
            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.ExecuteNonQuery();
        }
    }
}
