using System;
using System.Threading.Tasks;

namespace RestaurantApp.Services.Adapter
{
    // Simplified Payment Implementation to support Factory Method
    public class StripeAdapter : IPaymentProcessor
    {
        private readonly string _cardNumber;

        public StripeAdapter(string cardNumber)
        {
            _cardNumber = cardNumber;
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
                await Task.Delay(500); 
                
                if (amount <= 0) throw new ArgumentException("Amount must be positive.");
                
                Console.WriteLine($"[Stripe] Successfully charged {amount} to card ending in {_cardNumber.Substring(_cardNumber.Length - 4)}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Stripe Error] {ex.Message}");
                return false;
            }
        }
    }
}
