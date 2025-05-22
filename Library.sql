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


INSERT INTO Users (Name, Email, Password, Role, Phone, Address)
VALUES 
  ('Employee1', 'employee1@example.com', 'password1', 'Employee', '123-456-7890', 'Address 1'),
  ('Employee2', 'employee2@example.com', 'password2', 'Employee', '098-765-4321', 'Address 2');
