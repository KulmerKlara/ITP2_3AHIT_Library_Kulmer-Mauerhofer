namespace Library.Data;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public string Summary { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Book(int bookId, string title, string author, string genre, string summary, bool isAvailable = true)
    {
        BookId = bookId;
        Title = title;
        Author = author;
        Genre = genre;
        Summary = summary;
        IsAvailable = isAvailable;
    }
}