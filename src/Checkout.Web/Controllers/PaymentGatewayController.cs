using System;
using System.Net;
using Checkout.Repository;
using Checkout.Web.Infrastructure;
using Checkout.Web.Models;
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

    public class RetrievePaymentRequestExample : IExamplesProvider<RetrievePaymentRequest>
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
