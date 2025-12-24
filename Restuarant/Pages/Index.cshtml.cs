using Microsoft.AspNetCore.Mvc.RazorPages;
using Restuarant.Core.Interfaces;
using RestaurantApp;
using System.Collections.Generic;
using System.Linq;

namespace Restuarant.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<MenuItem> MenuItems { get; set; }

        public void OnGet()
        {
            MenuItems = _unitOfWork.MenuItems.GetAll(orderBy: q => q.OrderBy(m => m.Category)).ToList();
        }

        public string GetCategoryClass(string category)
        {
            return category.ToLower() switch
            {
                "seafood" => "fish",
                "sandwiches" => "sandwitches",
                "casseroles" => "meats",
                "pastries" => "specialty",
                _ => "other"
            };
        }
    }
}
