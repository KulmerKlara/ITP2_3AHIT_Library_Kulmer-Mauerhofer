using Library.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SQLite;

namespace Tests;

[TestClass]
public class UnitTest1
{
    private SQLiteConnection _connection;
    private UserBookListRepository _repository;

    [TestInitialize]
    public void Setup()
    {
        // In-Memory DB for testing
        _connection = new SQLiteConnection("Data Source=:memory:");
        _connection.Open();

        // Replace global connection for tests
        Database.OverrideConnection(_connection);

        // Create required schema
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


}