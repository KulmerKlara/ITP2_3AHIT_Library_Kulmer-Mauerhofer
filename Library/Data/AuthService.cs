namespace Library.Data
{
    /// <summary>
    /// Service for managing authentication state in the application.
    /// </summary>
    public class AuthService
    {
        /// <summary>
        /// Gets the currently logged-in user. Null if no user is logged in.
        /// </summary>
        public User LoggedInUser { get; private set; }

        /// <summary>
        /// Sets the currently logged-in user.
        /// </summary>
        /// <param name="user">The user to set as logged in.</param>
        public void SetUser(User user)
        {
            LoggedInUser = user;
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        public void Logout()
        {
            LoggedInUser = null;
        }

        /// <summary>
        /// Gets a value indicating whether a user is authenticated.
        /// </summary>
        public bool IsAuthenticated => LoggedInUser != null;
    }
}
