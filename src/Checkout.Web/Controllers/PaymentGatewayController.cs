using System.Net;
using System.Threading.Tasks;
using Checkout.Services.Banks;
using Checkout.Services.Services;
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
        public async Task<IActionResult> Post([FromBody]SubmitPaymentRequest submitPaymentRequest, [FromQuery] string correlationId = null)
        {
            submitPaymentRequest.CorrelationId = correlationId;

            var bankPaymentRequest = GenerateBankPaymentRequest(submitPaymentRequest);
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
        public async Task<IActionResult> Get([FromQuery]RetrievePaymentRequest retrievePaymentRequest, [FromQuery] string correlationId = null)
        {
            retrievePaymentRequest.CorrelationId = correlationId;
            
            var orchestratorPaymentRequest = new Services.Models.RetrievePaymentRequest(retrievePaymentRequest.PaymentId, retrievePaymentRequest.MerchantId);

            var payment = _paymentOrchestrator.RetrievePayment(orchestratorPaymentRequest);

            return Ok(new RetrievePaymentResponse
            {
                CVV = payment.CVV,
                Currency = payment.Currency,
                ExpiryDate = payment.ExpiryDate,
                PaymentResponseStatus = payment.PaymentStatus,
                CardNumber = payment.CardNumber,
                Amount = payment.Amount
            });
        }

        private BankPaymentRequest GenerateBankPaymentRequest(SubmitPaymentRequest submitPaymentRequest)
        {
            return new BankPaymentRequest
            {
                CardDetails = new Services.Models.CardDetails(submitPaymentRequest.CardDetails.CardNumber,
                    submitPaymentRequest.CardDetails.ExpiryDate,
                    submitPaymentRequest.CardDetails.Currency,
                    submitPaymentRequest.CardDetails.CVV),
                MerchantDetails = new Services.Models.MerchantDetails(submitPaymentRequest.MerchantDetails.MerchantId),
                Amount = submitPaymentRequest.Amount
            };
        }
    }
}
