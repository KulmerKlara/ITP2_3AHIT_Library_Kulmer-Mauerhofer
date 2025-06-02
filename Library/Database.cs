using System.Data.SQLite;
using System.IO;

namespace Library.Data
{
    public static class Database
    {
        private const string ConnectionString = "Data Source=library.db";

        public static SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            return connection;
        }
        private static SQLiteConnection _testConnection;
        public static void OverrideConnection(SQLiteConnection connection)
        {
            _testConnection = connection;
        }
        public static void Initialize()
        {
            if (!File.Exists(ConnectionString))
            {
                SQLiteConnection.CreateFile(ConnectionString);
                using var conn = GetConnection();
                using var cmd = new SQLiteCommand(conn);

                cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Users (
    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Email TEXT NOT NULL,
    Password TEXT NOT NULL,
    Role TEXT CHECK (Role IN ('Customer', 'Employee')) NOT NULL,
    Phone TEXT,
    Address TEXT
);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
INSERT INTO Users (Name, Email, Password, Role, Phone) 
SELECT 'Employee1', 'employee1@example.com', 'password1', 'Employee', '123-456-7890'
WHERE NOT EXISTS(SELECT 1 FROM Users WHERE Email = 'employee1@example.com');
";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
INSERT INTO Users (Name, Email, Password, Role, Phone) 
SELECT 'Employee2', 'employee2@example.com', 'password2', 'Employee', '098-765-4321'
WHERE NOT EXISTS(SELECT 1 FROM Users WHERE Email = 'employee2@example.com');
";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Books (
    BookID INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Author TEXT NOT NULL,
    Genre TEXT,
    Summary TEXT,
    IsAvailable BOOLEAN DEFAULT 1,
    GiveBackDate DATE
);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Reservations (
    ReservationID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    BookID INTEGER NOT NULL,
    ReservationAt DATE NOT NULL,
    PickupDeadline DATE NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (BookID) REFERENCES Books(BookID)
);";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Loans (
    LoanID INTEGER PRIMARY KEY AUTOINCREMENT,
    BookID INTEGER NOT NULL,
    UserID INTEGER NOT NULL,
    LoanDate DATE NOT NULL,
    DueDate DATE NOT NULL,
    ReturnedDate DATE,
    Extended BOOLEAN DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (BookID) REFERENCES Books(BookID)
);";
                cmd.ExecuteNonQuery();

                // Neu: GiveBackDate-Spalte in UserBookList
                cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS UserBookList (
    ListID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    BookID INTEGER NOT NULL,
    AddedAt DATE NOT NULL DEFAULT (DATE('now')),
    GiveBackDate DATE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (BookID) REFERENCES Books(BookID),
    UNIQUE(UserID, BookID)
);";
                cmd.ExecuteNonQuery();

                // Beispielbücher einfügen (wenn noch nicht vorhanden)
                cmd.CommandText = @"
INSERT INTO Books(Title, Author, Genre, Summary, IsAvailable)
SELECT 'Der Herr der Ringe', 'J.R.R. Tolkien', 'Fantasy', 'Ein episches Fantasy-Abenteuer.', 1
WHERE NOT EXISTS(SELECT 1 FROM Books WHERE Title = 'Der Herr der Ringe');

INSERT INTO Books(Title, Author, Genre, Summary, IsAvailable)
SELECT '1984', 'George Orwell', 'Dystopie', 'Ein Klassiker über Überwachung und Freiheit.', 1
WHERE NOT EXISTS(SELECT 1 FROM Books WHERE Title = '1984');

INSERT INTO Books(Title, Author, Genre, Summary, IsAvailable)
SELECT 'Die Verwandlung', 'Franz Kafka', 'Novelle', 'Die Geschichte von Gregor Samsa.', 1
WHERE NOT EXISTS(SELECT 1 FROM Books WHERE Title = 'Die Verwandlung');

INSERT INTO Books(Title, Author, Genre, Summary, IsAvailable)
SELECT 'Der kleine Prinz', 'Antoine de Saint-Exupéry', 'Kinderbuch', 'Ein philosophisches Märchen.', 1
WHERE NOT EXISTS(SELECT 1 FROM Books WHERE Title = 'Der kleine Prinz');

INSERT INTO Books(Title, Author, Genre, Summary, IsAvailable)
SELECT 'Faust', 'Johann Wolfgang von Goethe', 'Drama', 'Das klassische Drama um den Pakt mit dem Teufel.', 1
WHERE NOT EXISTS(SELECT 1 FROM Books WHERE Title = 'Faust');
";
                cmd.ExecuteNonQuery();
            }
            else
            {
                // Migration: Falls GiveBackDate noch nicht in Books existiert
                using var conn = GetConnection();
                using var cmdCheck = new SQLiteCommand("PRAGMA table_info(Books);", conn);
                using var reader = cmdCheck.ExecuteReader();

                bool giveBackDateExists = false;
                while (reader.Read())
                {
                    if (reader["name"].ToString() == "GiveBackDate")
                    {
                        giveBackDateExists = true;
                        break;
                    }
                }

                if (!giveBackDateExists)
                {
                    using var cmdAlter = new SQLiteCommand("ALTER TABLE Books ADD COLUMN GiveBackDate DATE;", conn);
                    cmdAlter.ExecuteNonQuery();
                }

                // Migration: Falls GiveBackDate noch nicht in UserBookList existiert
                using var cmdCheckUserBookList = new SQLiteCommand("PRAGMA table_info(UserBookList);", conn);
                using var readerUserBookList = cmdCheckUserBookList.ExecuteReader();

                bool giveBackDateExistsUserBookList = false;
                while (readerUserBookList.Read())
                {
                    if (readerUserBookList["name"].ToString() == "GiveBackDate")
                    {
                        giveBackDateExistsUserBookList = true;
                        break;
                    }
                }

                if (!giveBackDateExistsUserBookList)
                {
                    using var cmdAlterUserBookList = new SQLiteCommand("ALTER TABLE UserBookList ADD COLUMN GiveBackDate DATE;", conn);
                    cmdAlterUserBookList.ExecuteNonQuery();
                }
            }
        }
    }
}
