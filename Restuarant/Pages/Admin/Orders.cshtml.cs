using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restuarant.Core.Interfaces;
using RestaurantApp;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Restuarant.Pages.Admin
{
    public class OrdersModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Order> Orders { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToPage("/Admin/Login");
            }
            Orders = _unitOfWork.Orders.GetAll(includeProperties: "Items", orderBy: q => q.OrderByDescending(o => o.CreatedAt)).ToList();
            return Page();
        }

        public IActionResult OnPost(int id, string status)
        {
            var order = _unitOfWork.Orders.GetById(id);
            if (order != null)
            {
                order.Status = status;
                _unitOfWork.Orders.Update(order);
                _unitOfWork.Save();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostReset()
        {
            var allOrders = _unitOfWork.Orders.GetAll().ToList();
            _unitOfWork.Orders.DeleteRange(allOrders);
            _unitOfWork.Save();
            return RedirectToPage();
        }
    }
}
