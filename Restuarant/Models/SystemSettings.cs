using System.ComponentModel.DataAnnotations;

namespace RestaurantApp
{
    public class SystemSettings
    {
        [Key]
        public int Id { get; set; }
        public decimal TaxRate { get; set; }
        public string Currency { get; set; }
        public bool IsOpen { get; set; }
    }
}
