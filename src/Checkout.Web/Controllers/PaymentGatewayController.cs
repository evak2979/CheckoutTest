using System.Net;
using System.Threading.Tasks;
using Checkout.Services;
using Checkout.Services.Banks;
using Checkout.Web.Models;
using Checkout.Web.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IPaymentOrchestrator _paymentOrchestrator;
        private readonly ILogger<PaymentGatewayController> _logger;

        public PaymentGatewayController(IPaymentOrchestrator paymentOrchestrator, ILogger<PaymentGatewayController> logger)
        {
            _paymentOrchestrator = paymentOrchestrator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SubmitPaymentResponse), (int)HttpStatusCode.OK)]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(SubmitPaymentResponseExample))]
        [SwaggerRequestExample(typeof(SubmitPaymentRequest), typeof(SubmitPaymentRequestExample))]
        public async Task<IActionResult> Post(SubmitPaymentRequest paymentRequest)
        {
            var bankPaymentRequest = GenerateBankPaymentRequest(paymentRequest);
            var bankPaymentResponse = _paymentOrchestrator.ProcessPayment(bankPaymentRequest);

            return Ok(new SubmitPaymentResponse
            {
                PaymentId = bankPaymentResponse.PaymentId,
                PaymentResponseStatus = bankPaymentResponse.BankPaymentResponseStatus.ToString()
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(RetrievePaymentResponse), (int)HttpStatusCode.OK)]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(RetrievePaymentResponseExample))]
        [SwaggerRequestExample(typeof(RetrievePaymentRequest), typeof(RetrievePaymentRequestExample))]
        public async Task<IActionResult> Get(RetrievePaymentRequest paymentRequest)
        {
            return null;
        }

        private BankPaymentRequest GenerateBankPaymentRequest(SubmitPaymentRequest submitPaymentRequest)
        {
            return new BankPaymentRequest
            {
                CardDetails = new Services.CardDetails
                {
                    CardNumber = submitPaymentRequest.CardDetails.CardNumber,
                    CVV = submitPaymentRequest.CardDetails.CVV,
                    ExpiryDate = submitPaymentRequest.CardDetails.ExpiryDate,
                    Currency = submitPaymentRequest.CardDetails.Currency
                },
                MerchantDetails = new Services.MerchantDetails
                {
                    MerchantId = submitPaymentRequest.MerchantDetails.Id
                },
                Amount = submitPaymentRequest.Amount
            };
        }
    }
}
