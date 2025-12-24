using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Restuarant.Pages.Admin
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnGet()
        {
            // Check if already logged in
            if (HttpContext.Session.GetString("AdminLoggedIn") == "true")
            {
                Response.Redirect("/Admin/Index");
            }
        }

        public IActionResult OnPost()
        {
            // Hardcoded credentials
            const string validEmail = "Ahmedtayel714@gmail.com";
            const string validPassword = "Ahmed.101";

            System.Console.WriteLine($"Login Attempt: {Email} vs {validEmail} | {Password} vs {validPassword}");

            if (Email == validEmail && Password == validPassword)
            {
                // Set session
                HttpContext.Session.SetString("AdminLoggedIn", "true");
                HttpContext.Session.SetString("AdminEmail", Email);
                
                return RedirectToPage("/Admin/Index");
            }
            else
            {
                TempData["Error"] = "❌ Invalid email or password!";
                return Page();
            }
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Admin/Login");
        }
    }
}
