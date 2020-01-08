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

        public long CardNumber { get; private set; }

        public string ExpiryDate { get; private set; }

        public string Currency { get; private set; }

        public int CVV { get; private set; }
    }
}
