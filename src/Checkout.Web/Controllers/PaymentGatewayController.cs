using System.Net;
using Checkout.Repository;
using Checkout.Web.Infrastructure;
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
        private readonly IPaymentRepository _paymentRepository;

        public PaymentGatewayController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        private readonly ILogger<PaymentGatewayController> _logger;

        public PaymentGatewayController(ILogger<PaymentGatewayController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SubmitPaymentResponse), (int)HttpStatusCode.OK)]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(SubmitPaymentResponseExample))]
        [SwaggerRequestExample(typeof(SubmitPaymentRequest), typeof(SubmitPaymentRequestExample))]
        public IActionResult Post(SubmitPaymentRequest paymentRequest)
        {
            return null;
        }

        [HttpGet]
        [ProducesResponseType(typeof(RetrievePaymentResponse), (int)HttpStatusCode.OK)]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(RetrievePaymentResponseExample))]
        [SwaggerRequestExample(typeof(RetrievePaymentRequest), typeof(RetrievePaymentRequestExample))]
        public IActionResult Get(RetrievePaymentRequest paymentRequest)
        {
            return null;
        }
    }
}
