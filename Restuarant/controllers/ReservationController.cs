using Microsoft.AspNetCore.Mvc;
using RestaurantApp;
using Restuarant.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservationController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("book")]
    public IActionResult Book([FromBody] Reservation reservation)
    {
        if (reservation == null)
            return BadRequest(new { message = "Invalid reservation data." });

        _unitOfWork.Reservations.Insert(reservation);
        _unitOfWork.Save();

        return Ok(new { message = "Table booked successfully!" });
    }

    [HttpGet("all")]
    public IActionResult GetAll()
    {
        return Ok(_unitOfWork.Reservations.GetAll());
    }
}
