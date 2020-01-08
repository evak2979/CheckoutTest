using System;

namespace Checkout.Repository.Models
{
    public sealed class MerchantDetails
    {
        public MerchantDetails()
        {
            
        }

        public MerchantDetails(Guid merchantId)
        {
            MerchantId = merchantId;
        }

        public Guid MerchantId { get; }
    }
}