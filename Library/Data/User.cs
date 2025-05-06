namespace Library.Data;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }

    public User(int userId, string name, string email, string password, string phone, string role)
    {
        Id = userId;
        Name = name;
        Email = email;
        Password = password;
        Phone = phone;
        Role = role;
    }

    public void Login(string email, string password)
    {
        if (email == Email && password == Password)
        {
            Console.WriteLine("Login successful!");
        }
        else
        {
            Console.WriteLine("Invalid email or password.");
        }
    }
    public void register(string name, string email, string password, string phone)
    {
        Name = name;
        Email = email;
        Password = password;
        this.Phone = phone;
        Console.WriteLine("Registration successful!");
    }
}
