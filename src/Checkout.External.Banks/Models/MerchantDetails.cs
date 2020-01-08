using System;

namespace Checkout.Services.Models
{
    public sealed class MerchantDetails
    {
        public MerchantDetails(Guid merchantId)
        {
            MerchantId = merchantId;
        }

        public Guid MerchantId { get; private set; }
    }
}