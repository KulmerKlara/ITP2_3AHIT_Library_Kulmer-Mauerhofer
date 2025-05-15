namespace Library.Data
{
    /// <summary>
    /// Represents a user of the library system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user's unique ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user's name (used as a username).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the user's role (e.g., Customer or Employee).
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userId">The unique user ID.</param>
        /// <param name="name">The user's name.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="phone">The user's phone number.</param>
        /// <param name="role">The user's role in the system.</param>
        public User(int userId, string name, string email, string password, string phone, string role)
        {
            Id = userId;
            Name = name;
            Email = email;
            Password = password;
            Phone = phone;
            Role = role;
        }

        /// <summary>
        /// Simulates a login attempt with the provided credentials.
        /// </summary>
        /// <param name="email">The email used for login.</param>
        /// <param name="password">The password used for login.</param>
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

        /// <summary>
        /// Simulates a registration process by setting user properties.
        /// </summary>
        /// <param name="name">The user's name.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="phone">The user's phone number.</param>
        public void register(string name, string email, string password, string phone)
        {
            Name = name;
            Email = email;
            Password = password;
            this.Phone = phone;

            Console.WriteLine("Registration successful!");
        }
    }
}
