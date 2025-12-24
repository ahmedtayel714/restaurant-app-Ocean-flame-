using RestaurantApp.Services.Adapter;

namespace RestaurantApp.Services.Factory
{
    public static class PaymentFactory
    {
        public static IPaymentProcessor CreatePaymentProcessor(string type, string identifier)
        {
            switch (type.ToLower())
            {
                case "paypal":
                    return new PayPalAdapter(identifier); // identifier = email
                case "stripe":
                case "creditcard":
                    return new StripeAdapter(identifier); // identifier = card number
                default:
                    throw new System.ArgumentException("Invalid payment type");
            }
        }
    }
}
