using System;
namespace RestaurantApp
{
    public class Reservation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int People { get; set; }
        public string Message { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
    }
}