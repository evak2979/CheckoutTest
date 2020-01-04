using System;
using System.Threading.Tasks;
using Checkout.Services;
using Checkout.Tests.Integration.Helpers;
using Checkout.Web.Controllers;
using Checkout.Web.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace Checkout.Tests.Integration
{
    public class PaymentGatewayController
    {
        private Web.Controllers.PaymentGatewayController _sut;
        private ServicesHelper _servicesHelper;

        public PaymentGatewayController()
        {
            _servicesHelper = new ServicesHelper();
            var serviceProvider = _servicesHelper.ServiceCollection.BuildServiceProvider();

            _sut = new Web.Controllers.PaymentGatewayController(serviceProvider.GetService<IPaymentOrchestrator>(), 
                serviceProvider.GetService<Logger<Web.Controllers.PaymentGatewayController>>());
        }

        [Fact]
        public async Task GivenAPaymentRequest_WhenValidPayment_ShouldReturn200SuccessCode()
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
            var result = await _sut.Post(submitPaymentRequest);

            // then
            result.ShouldBeOfType<OkObjectResult>();
        }
    }
}
