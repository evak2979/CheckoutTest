namespace Checkout.Repository.Models
{
    public class CardDetails
    {
        public string CardNumber { get; set; }

        public string ExpiryDate { get; set; }

        public string Currency { get; set; }

        public string CVV { get; set; }
    }
}