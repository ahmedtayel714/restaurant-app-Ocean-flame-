using System;
using System.Threading.Tasks;

namespace RestaurantApp.Services.Adapter
{
    // Simplified Payment Implementation to support Factory Method
    public class PayPalAdapter : IPaymentProcessor
    {
        private readonly string _email;

        public PayPalAdapter(string email)
        {
            _email = email;
        }

        public bool ProcessPayment(decimal amount)
        {
             return ProcessPaymentAsync(amount).GetAwaiter().GetResult();
        }

        public async Task<bool> ProcessPaymentAsync(decimal amount)
        {
            try
            {
                // Simulate Async Work
                await Task.Delay(600); 

                if (string.IsNullOrEmpty(_email)) throw new ArgumentException("Email is invalid.");

                Console.WriteLine($"[PayPal] Successfully processed {amount} for {_email}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PayPal Error] {ex.Message}");
                return false;
            }
        }
    }
}
