namespace Library.Data
{
    public class AuthService
    {
        public User LoggedInUser { get; private set; }

        public void SetUser(User user)
        {
            LoggedInUser = user;
        }

        public void Logout()
        {
            LoggedInUser = null;
        }

        public bool IsAuthenticated => LoggedInUser != null;
    }
}
