using Checkout.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Web.Swagger
{
    public class RetrievePaymentResponseExample : IExamplesProvider<RetrievePaymentResponse>
    {
        public RetrievePaymentResponse GetExamples()
        {
            return new RetrievePaymentResponse
            {
                CardDetails = new CardDetails
                {
                    ExpiryDate = "01/1010",
                    Currency = "Pound",
                    CVV = 123,
                    CardNumber = 1234567890123456
                },
                PaymentResponseStatus = PaymentResponseStatus.Successful
            };
        }
    }
}