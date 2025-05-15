using System.Data.SQLite;
using System.IO;

namespace Library.Data
{
    /// <summary>
    /// Provides methods to manage the SQLite database connection and initialization.
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Path to the SQLite database file.
        /// </summary>
        private static readonly string _dbPath = "library.db";

        /// <summary>
        /// Creates and opens a new SQLite connection to the database.
        /// </summary>
        /// <returns>An open <see cref="SQLiteConnection"/> object.</returns>
        public static SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection($"Data Source={_dbPath}");
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Initializes the database by creating the file and tables if the database does not exist.
        /// </summary>
        public static void Initialize()
        {
            // Check if the database file already exists
            if (!File.Exists(_dbPath))
            {
                // Create the SQLite database file
                SQLiteConnection.CreateFile(_dbPath);

                // Open connection to the newly created database
                using var conn = GetConnection();

                // Create a command to define the database schema
                using var cmd = new SQLiteCommand(conn);

                // SQL commands to create the necessary tables with their relationships and constraints
                cmd.CommandText = @"
                    CREATE TABLE Users (
                        UserID INTEGER PRIMARY KEY,
                        Name TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        Password TEXT NOT NULL,
                        Role TEXT CHECK (Role IN ('Customer', 'Employee')) NOT NULL,
                        Phone TEXT,
                        Address TEXT
                    );

                    CREATE TABLE Books (
                        BookID INTEGER PRIMARY KEY,
                        Title TEXT NOT NULL,
                        Author TEXT NOT NULL,
                        Genre TEXT,
                        Summary TEXT,
                        IsAvailable BOOLEAN DEFAULT 1
                    );

                    CREATE TABLE Reservations (
                        ReservationID INTEGER PRIMARY KEY,
                        UserID INTEGER NOT NULL,
                        BookID INTEGER NOT NULL,
                        ReservationAt DATE NOT NULL,
                        PickupDeadline DATE NOT NULL,
                        FOREIGN KEY (UserID) REFERENCES Users(UserID),
                        FOREIGN KEY (BookID) REFERENCES Books(BookID)
                    );

                    CREATE TABLE Loans (
                        LoanID INTEGER PRIMARY KEY,
                        BookID INTEGER NOT NULL,
                        UserID INTEGER NOT NULL,
                        LoanDate DATE NOT NULL,
                        DueDate DATE NOT NULL,
                        ReturnedDate DATE,
                        Extended BOOLEAN DEFAULT 0,
                        FOREIGN KEY (UserID) REFERENCES Users(UserID),
                        FOREIGN KEY (BookID) REFERENCES Books(BookID)
                    );
                ";

                // Execute the SQL commands to create tables
                cmd.ExecuteNonQuery();
            }
        }
    }
}
