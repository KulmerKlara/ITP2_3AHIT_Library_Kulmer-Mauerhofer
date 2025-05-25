using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage; // Server Blazor
// Für WASM: using Microsoft.AspNetCore.Components.WebAssembly.ProtectedBrowserStorage;

namespace Library.Data
{
    public class AuthService
    {
        private readonly ProtectedLocalStorage _localStorage;
        private const string UserStorageKey = "loggedInUser";

        public User LoggedInUser { get; private set; }

        public bool IsAuthenticated => LoggedInUser != null;

        public AuthService(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task InitializeAsync()
        {
            var result = await _localStorage.GetAsync<string>(UserStorageKey);
            if (result.Success && !string.IsNullOrEmpty(result.Value))
            {
                LoggedInUser = JsonSerializer.Deserialize<User>(result.Value);
            }
        }

        public async Task SetUserAsync(User user)
        {
            LoggedInUser = user;
            var json = JsonSerializer.Serialize(user);
            await _localStorage.SetAsync(UserStorageKey, json);
        }

        public async Task LogoutAsync()
        {
            LoggedInUser = null;
            await _localStorage.DeleteAsync(UserStorageKey);
        }
    }
}
