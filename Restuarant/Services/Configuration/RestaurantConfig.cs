namespace RestaurantApp.Services.Configuration
{
    public sealed class RestaurantConfig
    {
        private static RestaurantConfig _instance = null;
        private static readonly object _lock = new object();

        public decimal TaxRate { get; set; } = 0.14m; // 14% VAT
        public string Currency { get; set; } = "EGP";
        public bool IsOpen { get; set; } = true;

        private RestaurantConfig()
        {
            // Private constructor to prevent instantiation
        }

        public static RestaurantConfig Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new RestaurantConfig();
                    }
                    return _instance;
                }
            }
        }
    }
}
