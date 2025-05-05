namespace Library.Data;
public class Employee : User
{
    public Employee(int userId, string name, string email, string password, string phone)
    : base(userId, name, email, password, phone, "Employee")
    {
    }

    public void AddBook()
    {
        // Implement book adding logic here
    }
}
