using Library.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;

/// <summary>
/// Enthält Unit-Tests für die <see cref="UserBookListRepository"/>-Klasse.
/// Testet das Hinzufügen, Entfernen und Abfragen von Büchern in der Benutzer-Bücherliste.
/// Verwendet eine In-Memory-SQLite-Datenbank für isolierte Tests.
/// </summary>
/// <remarks>
/// Die Tests stellen sicher, dass die Methoden von <see cref="UserBookListRepository"/> korrekt funktionieren.
/// Die Datenbankverbindung wird für jeden Test neu initialisiert und nach jedem Test bereinigt.
/// </remarks>
namespace Tests
{
    /// <summary>
    /// Testklasse für <see cref="UserBookListRepository"/>.
    /// </summary>
    [TestClass]
    public class UserBookListRepositoryTests
    {
        /// <summary>
        /// In-Memory SQLite-Verbindung für die Tests.
        /// </summary>
        private SQLiteConnection _connection;

        /// <summary>
        /// Instanz des zu testenden Repositories.
        /// </summary>
        private UserBookListRepository _repository;

        /// <summary>
        /// Initialisiert die Testumgebung vor jedem Test.
        /// Erstellt eine neue In-Memory-Datenbank und das notwendige Schema.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // In-Memory DB für Tests
            _connection = new SQLiteConnection("Data Source=:memory:");
            _connection.Open();

            // Überschreibt die globale Verbindung für Tests
            Database.OverrideConnection(_connection);

            // Erstellt das benötigte Datenbankschema
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Books (
                    BookID INTEGER PRIMARY KEY,
                    Title TEXT,
                    Author TEXT,
                    Genre TEXT,
                    Summary TEXT,
                    IsAvailable INTEGER
                );
                CREATE TABLE UserBookList (
                    UserID INTEGER,
                    BookID INTEGER,
                    PRIMARY KEY (UserID, BookID)
                );";
            cmd.ExecuteNonQuery();

            _repository = new UserBookListRepository();
        }

        /// <summary>
        /// Bereinigt die Testumgebung nach jedem Test.
        /// Schließt und entfernt die Datenbankverbindung.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
        }

        /// <summary>
        /// Testet, ob das Hinzufügen eines Buches zur Benutzerliste funktioniert.
        /// </summary>
        [TestMethod]
        public void AddBookToUserList_ShouldAddEntry()
        {
            InsertBook(1, "Testbuch");

            _repository.AddBookToUserList(100, 1);

            var books = _repository.GetUserBookList(100);

            Assert.AreEqual(1, books.Count);
            Assert.AreEqual("Testbuch", books[0].Title);
        }

        /// <summary>
        /// Testet, ob das Entfernen eines Buches aus der Benutzerliste funktioniert.
        /// </summary>
        [TestMethod]
        public void RemoveBookFromUserList_ShouldRemoveEntry()
        {
            InsertBook(2, "Entfernbares Buch");
            _repository.AddBookToUserList(200, 2);

            _repository.RemoveBookFromUserList(200, 2);
            var books = _repository.GetUserBookList(200);

            Assert.AreEqual(0, books.Count);
        }

        /// <summary>
        /// Testet, ob die korrekten Bücher für einen Benutzer zurückgegeben werden.
        /// </summary>
        [TestMethod]
        public void GetUserBookList_ShouldReturnCorrectBooks()
        {
            InsertBook(3, "Buch A");
            InsertBook(4, "Buch B");

            _repository.AddBookToUserList(300, 3);
            _repository.AddBookToUserList(300, 4);

            var books = _repository.GetUserBookList(300);

            Assert.AreEqual(2, books.Count);
            Assert.IsTrue(books.Any(b => b.Title == "Buch A"));
            Assert.IsTrue(books.Any(b => b.Title == "Buch B"));
        }

        /// <summary>
        /// Testet, ob <see cref="UserBookListRepository.IsBookAlreadyInUserList"/> true zurückgibt, wenn das Buch existiert.
        /// </summary>
        [TestMethod]
        public void IsBookAlreadyInUserList_ShouldReturnTrueIfExists()
        {
            InsertBook(5, "Vorhandenes Buch");
            _repository.AddBookToUserList(400, 5);

            bool exists = _repository.IsBookAlreadyInUserList(400, 5);

            Assert.IsTrue(exists);
        }

        /// <summary>
        /// Testet, ob <see cref="UserBookListRepository.IsBookAlreadyInUserList"/> false zurückgibt, wenn das Buch nicht existiert.
        /// </summary>
        [TestMethod]
        public void IsBookAlreadyInUserList_ShouldReturnFalseIfNotExists()
        {
            InsertBook(6, "Nichtvorhandenes Buch");

            bool exists = _repository.IsBookAlreadyInUserList(500, 6);

            Assert.IsFalse(exists);
        }

        /// <summary>
        /// Fügt ein Buch mit den angegebenen Parametern in die Datenbank ein.
        /// </summary>
        /// <param name="id">Die ID des Buches.</param>
        /// <param name="title">Der Titel des Buches.</param>
        private void InsertBook(int id, string title)
        {
            using var cmd = _connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Books (BookID, Title, Author, Genre, Summary, IsAvailable)
                VALUES (@id, @title, 'Autor', 'Genre', 'Zusammenfassung', 1)";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.ExecuteNonQuery();
        }
    }
}