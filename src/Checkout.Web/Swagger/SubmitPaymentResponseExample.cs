using System;
using Checkout.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Web.Swagger
{
    public class SubmitPaymentResponseExample : IExamplesProvider<SubmitPaymentResponse>
    {
        public SubmitPaymentResponse GetExamples()
        {
            return new SubmitPaymentResponse
            {
                PaymentId = Guid.NewGuid(),
                MerchantId =  Guid.NewGuid()
            };
        }
    }
}