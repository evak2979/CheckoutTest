using Checkout.Repository.Models.Base;
using Checkout.Services.Models;

namespace Checkout.Services.Banks
{
    public class BankPaymentRequest : BaseRequest
    {
        public BankPaymentRequest(CardDetails cardDetails, MerchantDetails merchantDetails, decimal amount, string correlationId)
        {
            base.CorrelationId = correlationId;
            Amount = amount;
            CardDetails = cardDetails;
            MerchantDetails = merchantDetails;
        }

        public decimal Amount { get; }

        public MerchantDetails MerchantDetails { get; }

        public CardDetails CardDetails { get; }
    }
}