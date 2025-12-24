using System.Threading.Tasks;

namespace RestaurantApp.Services.Adapter
{
    public interface IPaymentProcessor
    {
        bool ProcessPayment(decimal amount);
        Task<bool> ProcessPaymentAsync(decimal amount);
    }
}
