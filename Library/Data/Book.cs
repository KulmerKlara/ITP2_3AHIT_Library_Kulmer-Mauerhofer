namespace Library.Data;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public string Summary { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime? GiveBackDate { get; set; }

    /// <summary>
    /// Constructor for the Book class.
    /// </summary>
    /// <param name="bookId"></param>
    /// <param name="title"></param>
    /// <param name="author"></param>
    /// <param name="genre"></param>
    /// <param name="summary"></param>
    /// <param name="isAvailable"></param>
    /// <param name="giveBackDate"></param>
    public Book(int bookId, string title, string author, string genre, string summary, bool isAvailable = true, DateTime? giveBackDate = null)
    {
        BookId = bookId;
        Title = title;
        Author = author;
        Genre = genre;
        Summary = summary;
        IsAvailable = isAvailable;
        GiveBackDate = giveBackDate;
    }


}