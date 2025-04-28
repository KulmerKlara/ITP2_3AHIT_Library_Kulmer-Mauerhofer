namespace Library.Data;

public class User
{
    public int UserId { get; protected set; }
    public string Name { get; set; }
    public string Email { get; set; }    
    public string Password { get; protected set; }    
    public string telephone { get; protected set; }
   
    public void login(string email, string password)
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
    public void register(string name, string email, string password, string telephone)
    {
        Name = name;
        Email = email;
        Password = password;
        this.telephone = telephone;
        Console.WriteLine("Registration successful!");
    }
}
