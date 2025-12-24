using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restuarant.Data;
using System.Linq;

namespace Restuarant.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantContext _context;

        public IndexModel(RestaurantContext context)
        {
            _context = context;
        }

        public int TotalOrders { get; set; }
        public int PendingReservations { get; set; }
        public int LowStockItems { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToPage("/Admin/Login");
            }

            TotalOrders = _context.Orders.Count();
            PendingReservations = _context.Reservations.Count(r => r.Status == "Pending");
            LowStockItems = _context.MenuItems.Count(m => m.StockQuantity < 10);
            
            return Page();
        }
    }
}
