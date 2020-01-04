using System;

namespace Checkout.Repository.Models
{
    public class RetrievePaymentRequest
    {
        public Guid PaymentId { get; private set; }

        public Guid VendorId { get; private set; }
    }
}