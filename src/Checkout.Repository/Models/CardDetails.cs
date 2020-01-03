namespace Checkout.Repository.Models
{
    public class CardDetails
    {
        public long CardNumber { get; set; }

        public string ExpiryDate { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public int CVV { get; set; }
    }
}