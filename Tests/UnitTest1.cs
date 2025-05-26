/*using Library.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        BookRepository bookRepository = new BookRepository();
        bookRepository.AddBook(new Book(1, "Test Book", "Test Author", "Fiction", "This is a test book summary.", true, null));
        var books = bookRepository.GetAllBooks();

        Assert.IsNotNull(books);
        Assert.IsTrue(books.Count > 0);
        Assert.AreEqual("Test Book", books[1].Title);
    }
}*/