using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restuarant.Core.Interfaces;
using RestaurantApp;
using System.Collections.Generic;
using System.Linq;

namespace Restuarant.Pages.Admin
{
    public class ReservationsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Reservation> Reservations { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") != "true")
            {
                return RedirectToPage("/Admin/Login");
            }
            Reservations = _unitOfWork.Reservations.GetAll(orderBy: q => q.OrderByDescending(r => r.CreatedAt)).ToList();
            return Page();
        }

        public IActionResult OnPost(int id, string status)
        {
            var reservation = _unitOfWork.Reservations.GetById(id);
            if (reservation != null)
            {
                reservation.Status = status;
                _unitOfWork.Reservations.Update(reservation);
                _unitOfWork.Save();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostReset()
        {
            var allReservations = _unitOfWork.Reservations.GetAll().ToList();
            _unitOfWork.Reservations.DeleteRange(allReservations);
            _unitOfWork.Save();
            return RedirectToPage();
        }
    }
}
