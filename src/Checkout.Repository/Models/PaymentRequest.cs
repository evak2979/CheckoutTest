using System;

namespace Checkout.Repository.Models
{
    public class PaymentRequest
    {
        public Guid PaymentId { get; private set; }

        public Guid VendorId { get; private set; }
    }
}