using Checkout.Repository.Helpers;

namespace Checkout.Repository.Models
{
    public sealed class CardDetails
    {
        public CardDetails()
        {
            
        }

        public CardDetails(string cardNumber, string expiryDate, string currency, string cvv)
        {
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            Currency = currency;
            CVV = cvv;
        }

        public string CardNumber { get; private set; }

        public string ExpiryDate { get; private set; }

        public string Currency { get; private set; }

        public string CVV { get; private set; }

        public void Obfuscate(ISensitiveDataObfuscator dataObfuscator)
        {
            CVV = dataObfuscator.ObfuscateCvv(CVV);
            CardNumber = dataObfuscator.ObfuscateLongCardNumber(CardNumber);
        }
    }
}