using System;
using System.Threading.Tasks;
using Checkout.Services.Services;
using Checkout.Tests.Integration.Helpers;
using Checkout.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
                serviceProvider.GetService<Logger<Web.Controllers.PaymentGatewayController>>());
        }

        [Fact]
        public async Task GivenASubmitPaymentRequest_WhenValidPayment_ShouldReturn200SuccessCode()
        {
            // given
            var submitPaymentRequest = new SubmitPaymentRequest
            {
                CardDetails = new Web.Models.CardDetails
                {
                    CVV = 123,
                    Currency = "Pound",
                    ExpiryDate = "01/2020",
                    CardNumber = 1234567890123456
                },
                MerchantDetails = new Web.Models.MerchantDetails
                {
                    Id = Guid.NewGuid()
                },
                Amount = 12345
            };

            // when
            var submitPaymentResponse = await _sut.Post(submitPaymentRequest, Guid.NewGuid().ToString());

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
                CardDetails = new Web.Models.CardDetails
                {
                    CVV = 123,
                    Currency = "Pound",
                    ExpiryDate = "01/2020",
                    CardNumber = 1234567890123456
                },
                MerchantDetails = new Web.Models.MerchantDetails
                {
                    Id = Guid.NewGuid()
                },
                Amount = 12345
            };
            var response = await _sut.Post(submitPaymentRequest, Guid.NewGuid().ToString());
            var submitPaymentResponse = (response as OkObjectResult).Value as SubmitPaymentResponse;
            
            // when
            var retrievePaymentResponse = await _sut.Get(new RetrievePaymentRequest
            {
                PaymentId = submitPaymentResponse.PaymentId,
                MerchantId = submitPaymentRequest.MerchantDetails.Id
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
    }
}
