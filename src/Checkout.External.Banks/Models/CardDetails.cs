namespace Checkout.Services.Models
{
    public class CardDetails
    {
        public long CardNumber { get; set; }

        public string ExpiryDate { get; set; }

        public string Currency { get; set; }

        public int CVV { get; set; }
    }
}
