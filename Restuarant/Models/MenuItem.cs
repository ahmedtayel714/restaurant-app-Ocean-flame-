using System.ComponentModel.DataAnnotations;

namespace RestaurantApp
{
    public class MenuItem
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Range(0, 10000)]
        public decimal Price { get; set; }
        
        [Range(0, 10000)]
        public int StockQuantity { get; set; } = 20;
        
        public string ImageUrl { get; set; }
        
        public string Category { get; set; } = "Other";
    }
}
