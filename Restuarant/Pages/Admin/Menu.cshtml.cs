using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restuarant.Core.Interfaces;
using RestaurantApp;
using System.Collections.Generic;
using System.Linq;

namespace Restuarant.Pages.Admin
{
    public class MenuModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<MenuItem> MenuItems { get; set; }

        [BindProperty]
        public MenuItem NewItem { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToPage("/Admin/Login");
            }
            MenuItems = _unitOfWork.MenuItems.GetAll().ToList();
            return Page();
        }

        public IActionResult OnPostAdd(MenuItem NewItem)
        {
            // Simple validation since we are not using full tag helpers
            if (!string.IsNullOrEmpty(NewItem.Name))
            {
                _unitOfWork.MenuItems.Insert(NewItem);
                _unitOfWork.Save();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = _unitOfWork.MenuItems.GetById(id);
            if (item != null)
            {
                _unitOfWork.MenuItems.Delete(item);
                _unitOfWork.Save();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostUpdateStock(int id, int change)
        {
            var item = _unitOfWork.MenuItems.GetById(id);
            if (item != null)
            {
                item.StockQuantity += change;
                if (item.StockQuantity < 0) item.StockQuantity = 0;
                _unitOfWork.MenuItems.Update(item);
                _unitOfWork.Save();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostEdit(int id, string Name, string Description, string Category, decimal Price, string ImageUrl)
        {
            var item = _unitOfWork.MenuItems.GetById(id);
            if (item != null)
            {
                item.Name = Name;
                item.Description = Description;
                item.Category = Category;
                item.Price = Price;
                item.ImageUrl = ImageUrl;
                
                _unitOfWork.MenuItems.Update(item);
                _unitOfWork.Save();
            }
            return RedirectToPage();
        }
    }
}
