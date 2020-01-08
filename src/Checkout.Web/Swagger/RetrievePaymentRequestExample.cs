using System;
using Checkout.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Web.Swagger
{
    public sealed class RetrievePaymentRequestExample : IExamplesProvider<RetrievePaymentRequest>
    {
        public RetrievePaymentRequest GetExamples()
        {
            return new RetrievePaymentRequest
            {
                PaymentId = Guid.NewGuid(),
                MerchantId = Guid.NewGuid()
            };
        }
    }
}