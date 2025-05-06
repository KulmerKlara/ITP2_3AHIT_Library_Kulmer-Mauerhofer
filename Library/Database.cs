using System.Data.SQLite;
using System.IO;

namespace Library.Data
{
    public static class Database
    {
        private static readonly string _dbPath = "library.db";

        public static SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection($"Data Source={_dbPath}");
            conn.Open();
            return conn;
        }


        public static void Initialize()
        {
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
                using var conn = GetConnection();
                using var cmd = new SQLiteCommand(conn);

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
                cmd.ExecuteNonQuery();
            }
        }
    }
}
