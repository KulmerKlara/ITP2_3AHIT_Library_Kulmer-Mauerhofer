using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace YourAppNamespace.Pages.Employee
{
    [Authorize(Roles = "employee")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
