namespace Library.Data;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public string Summary { get; set; }
    public bool IsAvailable { get; set; } = true; // Default value is true

    public Book(int id, string title, string author, string genre, string summary, bool isAvailable = true)
    {
        Id = id;
        Title = title;
        Author = author;
        Genre = genre;
        Summary = summary;
        IsAvailable = isAvailable;
    }
}