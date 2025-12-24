using Microsoft.AspNetCore.Mvc;
using RestaurantApp;
using Restuarant;
using Restuarant.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("confirm")]
    public IActionResult ConfirmOrder([FromBody] List<OrderItem> orderItems)
    {
        Console.WriteLine("Received order: " + System.Text.Json.JsonSerializer.Serialize(orderItems)); // logging
        if (orderItems == null || orderItems.Count == 0)
            return BadRequest("No items in the order.");

        decimal totalPrice = 0;
        
        // Check stock first
        foreach (var item in orderItems)
        {
            var menuItem = _unitOfWork.MenuItems.GetAll(m => m.Name == item.Name).FirstOrDefault();
            if (menuItem != null)
            {
                if (menuItem.StockQuantity < item.Quantity)
                {
                    return BadRequest($"Sorry, only {menuItem.StockQuantity} left of {item.Name}.");
                }
            }
            totalPrice += item.Total;
        }

        // Deduct stock
        foreach (var item in orderItems)
        {
            var menuItem = _unitOfWork.MenuItems.GetAll(m => m.Name == item.Name).FirstOrDefault();
            if (menuItem != null)
            {
                menuItem.StockQuantity -= item.Quantity;
                _unitOfWork.MenuItems.Update(menuItem);
            }
        }

        var order = new Order
        {
            Items = orderItems,
            TotalPrice = totalPrice,
            Status = "Pending"
        };

        _unitOfWork.Orders.Insert(order);
        _unitOfWork.Save();

        Console.WriteLine("Saved order with ID: " + order.Id); // logging
        return Ok(new { message = "Order confirmed successfully!", total = totalPrice });
    }

    [HttpGet("all")]
    public IActionResult GetAllOrders()
    {
        var orders = _unitOfWork.Orders.GetAll(includeProperties: "Items");
        return Ok(orders); // إرجاع جميع الطلبات المحفوظة كـ JSON
    }
}