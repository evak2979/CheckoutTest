using System;
using Checkout.Services.Models.Base;


namespace Checkout.Services.Models
{
    public sealed class RetrievePaymentRequest : BaseRequest
    {
        public RetrievePaymentRequest(Guid paymentId, Guid merchantId, string correlationId)
        {
            PaymentId = paymentId;
            MerchantId = merchantId;
            CorrelationId = correlationId;
        }

        public Guid PaymentId { get; }

        public Guid MerchantId { get; }
    }
}