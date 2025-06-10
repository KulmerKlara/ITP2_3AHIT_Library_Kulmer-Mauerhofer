namespace Library.Data;

public class User_Employee_Service
{
    private static readonly string[] Roles = new[]
    {
        "Admin", "User", "Guest", "Moderator", "Editor"
    };

    /// <summary>
    /// Generates a list of users with dummy data.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public Task<User[]> GetUsersAsync(int count)
    {
        return Task.FromResult(Enumerable.Range(1, count).Select(index => new User(
            index,
            $"User{index}",
            $"user{index}@example.com",
            $"Password{index}",
            $"123-456-789{index}",
            Roles[index % Roles.Length]
        )).ToArray());
    }
}

