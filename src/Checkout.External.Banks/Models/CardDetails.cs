namespace Checkout.Services.Models
{
    public sealed class CardDetails
    {
        public CardDetails(long cardNumber, string expiryDate, string currency, int cvv)
        {
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            Currency = currency;
            CVV = cvv;
        }

        public long CardNumber { get; }

        public string ExpiryDate { get; }

        public string Currency { get; }

        public int CVV { get; }
    }
}
