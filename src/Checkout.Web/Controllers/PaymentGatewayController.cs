using System;
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

        /// [FromHeader] not working properly on a property of a model (see https://stackoverflow.com/questions/58038884/fromheaderattribute-doesnt-work-for-properties)
        /// As such, binding as a parameter and then assigning to request property.
        [HttpPost]
        [ProducesResponseType(typeof(SubmitPaymentResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(SubmitPaymentResponseExample))]
        [SwaggerRequestExample(typeof(SubmitPaymentRequest), typeof(SubmitPaymentRequestExample))]
        public async Task<IActionResult> Post(SubmitPaymentRequest submitPaymentRequest, [FromHeader] string correlationId = null)
        {
            _logger.LogInformation($"Submit Payment request with correlationId {correlationId} received");

            var bankPaymentRequest = GenerateBankPaymentRequest(submitPaymentRequest, correlationId);
            var bankPaymentResponse = _paymentOrchestrator.ProcessPayment(bankPaymentRequest);

            if (bankPaymentResponse.BankPaymentResponseStatus == BankPaymentResponseStatus.Successful)
            {
                return Ok(new SubmitPaymentResponse
                {
                    PaymentId = bankPaymentResponse.PaymentId,
                    PaymentResponseStatus = bankPaymentResponse.BankPaymentResponseStatus.ToString()
                });
            }
            else
            {
                return BadRequest();
            }
        }

        /// [FromHeader] not working properly on a property of a model (see https://stackoverflow.com/questions/58038884/fromheaderattribute-doesnt-work-for-properties)
        /// As such, binding as a parameter and then assigning to request property.
        [HttpGet]
        [ProducesResponseType(typeof(RetrievePaymentResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(RetrievePaymentResponseExample))]
        [SwaggerRequestExample(typeof(RetrievePaymentRequest), typeof(RetrievePaymentRequestExample))]
        public async Task<IActionResult> Get([FromQuery]RetrievePaymentRequest retrievePaymentRequest, [FromHeader] string correlationId = null)
        {
            _logger.LogInformation($"Retrieve Payment request with correlationId {correlationId} received");
            var orchestratorPaymentRequest = new Services.Models.RetrievePaymentRequest(retrievePaymentRequest.PaymentId, retrievePaymentRequest.MerchantId, correlationId);

            var payment = _paymentOrchestrator.RetrievePayment(orchestratorPaymentRequest);

            if (payment != null)
            {
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
            else
            {
                return NotFound();
            }
        }

        private BankPaymentRequest GenerateBankPaymentRequest(SubmitPaymentRequest submitPaymentRequest, string correlationId)
        {
            var cardDetails = new Services.Models.CardDetails(submitPaymentRequest.CardDetails.CardNumber,
                submitPaymentRequest.CardDetails.ExpiryDate,
                submitPaymentRequest.CardDetails.Currency,
                submitPaymentRequest.CardDetails.CVV);

            var merchantDetails =
                new Services.Models.MerchantDetails(submitPaymentRequest.MerchantDetails.MerchantId);
            return new BankPaymentRequest(cardDetails, merchantDetails, submitPaymentRequest.Amount, correlationId);
        }
    }
}
