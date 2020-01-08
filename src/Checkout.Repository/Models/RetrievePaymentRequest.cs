using System;
using Checkout.Repository.Models.Base;

namespace Checkout.Repository.Models
{
    public sealed class RetrievePaymentRequest : BaseRequest
    {
        public RetrievePaymentRequest(Guid paymentId, Guid merchantId)
        {
            PaymentId = paymentId;
            MerchantId = merchantId;
        }

        public Guid PaymentId { get; private set; }

        public Guid MerchantId { get; private set; }
    }
}