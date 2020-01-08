using Checkout.Web.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Web.Swagger
{
    public sealed class RetrievePaymentResponseExample : IExamplesProvider<RetrievePaymentResponse>
    {
        public RetrievePaymentResponse GetExamples()
        {
            return new RetrievePaymentResponse
            {
                ExpiryDate = "01/1010",
                Currency = "Pound",
                CVV = "123",
                CardNumber = "1234567890123456",
                PaymentResponseStatus = "Successful",
                Amount = 12345
            };
        }
    }
}