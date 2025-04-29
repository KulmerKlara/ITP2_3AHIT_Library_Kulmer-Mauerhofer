namespace Library.Data;
public class Employee : User
{
    
    public int Id { get; protected set; }
    public string Name { get; set; }
    public string email { get; set; }
    public string Password { get; protected set; }
    public string telephone { get; protected set; }
    public string Position { get; set; }
    public decimal Salary { get; set; }

    public Employee(int userId, string name, string email, string password, string telephone, decimal salary) 
        : base(userId, name, email, password, telephone)
    {
        Id = userId;
        Name = name;
        this.email = email;
        Password = password;
        this.telephone = telephone;
        Position = "Librarian"; // Default position
        Salary = salary; 
    }
    public void AddBook() { }

    public void Promote(string newPosition, decimal newSalary)
    {
        Position = newPosition;
        Salary = newSalary;
        Console.WriteLine($"Employee demoted to {newPosition} with a salary of {newSalary}.");
    }
}