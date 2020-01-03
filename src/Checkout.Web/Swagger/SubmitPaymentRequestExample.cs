using System;
using Checkout.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Web.Swagger
{
    public class SubmitPaymentRequestExample : IExamplesProvider<SubmitPaymentRequest>
    {
        public SubmitPaymentRequest GetExamples()
        {
            return new SubmitPaymentRequest
            {
                CardDetails = new CardDetails()
                {
                    ExpiryDate = "01/2023",
                    Currency = "Pound",
                    CVV = 123,
                    CardNumber = 1234567890123456
                },
                Amount = 12345,
                MerchantDetails = new MerchantDetails
                {
                    Id = Guid.NewGuid()
                }
            };
        }
    }
}