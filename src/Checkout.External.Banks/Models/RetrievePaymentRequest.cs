using System;
using Checkout.Repository.Models.Base;

namespace Checkout.Services.Models
{
    public class RetrievePaymentRequest : BaseRequest
    {
        public Guid PaymentId { get; set; }

        public Guid MerchantId { get; set; }
    }
}