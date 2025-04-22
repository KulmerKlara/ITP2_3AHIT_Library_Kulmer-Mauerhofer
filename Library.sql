CREATE TABLE Users (
    UserID INTEGER PRIMARY KEY,
    UserName TEXT NOT NULL,
    Password TEXT NOT NULL,
    Role TEXT CHECK (Role IN ('Kunde', 'Mitarbeiter')),
    Email TEXT,
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
    UserID INTEGER,
    BookID INTEGER,
    ReservationAt DATE,
    PickupDeadline DATE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (BookID) REFERENCES Books(BookID)
);

CREATE TABLE Loans (
    LoanID INTEGER PRIMARY KEY,
    BookID INTEGER,
    UserID INTEGER,
    LoanDate DATE,
    ReturnedDate DATE,
    DueDate DATE,
    Extended BOOLEAN DEFAULT 0,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (BookID) REFERENCES Books(BookID)
);
