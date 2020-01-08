using System;
using System.Threading.Tasks;
using Checkout.Services.Services;
using Checkout.Tests.Integration.Helpers;
using Checkout.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace Checkout.Tests.Integration
{
    /// <summary>
    /// Though I dockerized acceptance tests, I did not do the same with integration tests; this means that the LiteDb DB will keep growing with each run of these.
    /// It can simply be deleted in the output debug directory, and it will be recreated automatically. This would be dockerized in the pipeline as well.
    /// </summary>
    public class PaymentGatewayControllerTests
    {
        private Web.Controllers.PaymentGatewayController _sut;
        private ServicesHelper _servicesHelper;

        public PaymentGatewayControllerTests()
        {
            _servicesHelper = new ServicesHelper();
            var serviceProvider = _servicesHelper.ServiceCollection.BuildServiceProvider();

            _sut = new Web.Controllers.PaymentGatewayController(serviceProvider.GetService<IPaymentOrchestrator>(), 
                serviceProvider.GetService<ILogger<Web.Controllers.PaymentGatewayController>>());
        }

        [Fact]
        public async Task GivenASubmitPaymentRequest_WhenValidPayment_ShouldReturn200SuccessCode()
        {
            // given
            var submitPaymentRequest = new SubmitPaymentRequest
            {
                CardDetails = new CardDetails
                {
                    CVV = 123,
                    Currency = "Pound",
                    ExpiryDate = "01/2020",
                    CardNumber = 1234567890123456
                },
                MerchantDetails = new MerchantDetails
                {
                    MerchantId = Guid.NewGuid()
                },
                Amount = 12345
            };

            // when
            var submitPaymentResponse = await _sut.Post(submitPaymentRequest);

            // then
            submitPaymentResponse.ShouldBeOfType<OkObjectResult>();
            var paymentInformation = (submitPaymentResponse as OkObjectResult).Value as SubmitPaymentResponse;

            paymentInformation.PaymentId.ShouldBeOfType<Guid>();
            paymentInformation.PaymentResponseStatus.ShouldBe("Successful");
        }

        [Fact]
        public async Task GivenAPaymentRequest_WhenPaymentExists_ShouldReturn200SuccessCodeAndPaymentInformation()
        {
            // given
            var submitPaymentRequest = new SubmitPaymentRequest
            {
                CardDetails = new CardDetails
                {
                    CVV = 123,
                    Currency = "Pound",
                    ExpiryDate = "01/2020",
                    CardNumber = 1234567890123456
                },
                MerchantDetails = new MerchantDetails
                {
                    MerchantId = Guid.NewGuid()
                },
                Amount = 12345
            };
            var response = await _sut.Post(submitPaymentRequest);
            var submitPaymentResponse = (response as OkObjectResult).Value as SubmitPaymentResponse;
            
            // when
            var retrievePaymentResponse = await _sut.Get(new RetrievePaymentRequest
            {
                PaymentId = submitPaymentResponse.PaymentId,
                MerchantId = submitPaymentRequest.MerchantDetails.MerchantId
            });

            // then
            retrievePaymentResponse.ShouldBeOfType<OkObjectResult>();
            var retrievedPayment = (retrievePaymentResponse as OkObjectResult).Value as RetrievePaymentResponse;

            retrievedPayment.Amount.ShouldBe(submitPaymentRequest.Amount);
            retrievedPayment.CVV.ShouldBe("***");
            retrievedPayment.CardNumber.ShouldBe("1234********3456");
            retrievedPayment.ExpiryDate.ShouldBe(submitPaymentRequest.CardDetails.ExpiryDate);
            retrievedPayment.Currency.ShouldBe(submitPaymentRequest.CardDetails.Currency);
            retrievedPayment.PaymentResponseStatus.ShouldBe("Successful");
        }

        [Fact]
        public async Task GivenAretrievePaymentRequest_WhenPaymentDoesNotExist_ShouldReturn404()
        {
            // given +  when
            var retrievePaymentResponse = await _sut.Get(new RetrievePaymentRequest
            {
                PaymentId = Guid.NewGuid(),
                MerchantId = Guid.NewGuid()
            });

            // then
            retrievePaymentResponse.ShouldBeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GivenASubmitPaymentRequest_WhenUnsuccessful_ShouldReturn400()
        {
            // given
            var submitPaymentRequest = new SubmitPaymentRequest
            {
                CardDetails = new CardDetails
                {
                    CVV = 123,
                    Currency = "Pound",
                    ExpiryDate = "01/2020",
                    CardNumber = 1234567890123456
                },
                MerchantDetails = new MerchantDetails
                {
                    MerchantId = Guid.NewGuid()
                },
                Amount = -10000
            };

            // when
            var submitPaymentResponse = await _sut.Post(submitPaymentRequest);

            // then
            submitPaymentResponse.ShouldBeOfType<BadRequestResult>();
        }
    }
}
