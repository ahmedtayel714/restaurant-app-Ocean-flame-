using System;
using System.Collections.Generic;
using Restuarant;

namespace RestaurantApp
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending"; // Pending, Accepted, Completed, Cancelled
    }
}
