using Library.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    /// <summary>
    /// Testklasse für <see cref="AuthService"/>.
    /// </summary>
    [TestClass]
    public class AuthServiceTests
    {
        private AuthService _authService;

        /// <summary>
        /// Initialisiert die Testumgebung vor jedem Test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _authService = new AuthService();
        }

        /// <summary>
        /// Testet, ob ein Benutzer korrekt gesetzt wird.
        /// </summary>
        [TestMethod]
        public void SetUser_ShouldSetLoggedInUser()
        {
            var user = new User { UserID = 1, Username = "testuser" };

            _authService.SetUser(user);

            Assert.IsNotNull(_authService.LoggedInUser);
            Assert.AreEqual("testuser", _authService.LoggedInUser.Username);
        }

        /// <summary>
        /// Testet, ob der Benutzer beim Logout entfernt wird.
        /// </summary>
        [TestMethod]
        public void Logout_ShouldClearLoggedInUser()
        {
            var user = new User { UserID = 2, Username = "logoutuser" };
            _authService.SetUser(user);

            _authService.Logout();

            Assert.IsNull(_authService.LoggedInUser);
        }

        /// <summary>
        /// Testet, ob IsAuthenticated korrekt true zurückgibt, wenn ein Benutzer eingeloggt ist.
        /// </summary>
        [TestMethod]
        public void IsAuthenticated_ShouldReturnTrueWhenUserIsLoggedIn()
        {
            var user = new User { UserID = 3, Username = "authuser" };

            _authService.SetUser(user);

            Assert.IsTrue(_authService.IsAuthenticated);
        }

        /// <summary>
        /// Testet, ob IsAuthenticated false zurückgibt, wenn kein Benutzer eingeloggt ist.
        /// </summary>
        [TestMethod]
        public void IsAuthenticated_ShouldReturnFalseWhenNoUserIsLoggedIn()
        {
            Assert.IsFalse(_authService.IsAuthenticated);
        }
    }
}
