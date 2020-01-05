using System;
using System.Threading.Tasks;
using AutoFixture;
using Checkout.Services.Banks;
using Checkout.Services.Services;
using Checkout.Web.Controllers;
using Checkout.Web.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Checkout.Tests.Unit.Controllers
{
    public class PaymentGatewayControllerTests
    {
        private PaymentGatewayController _sut;
        private Mock<IPaymentOrchestrator> _mockOrchestator;
        private Mock<ILogger<PaymentGatewayController>> _mockLogger;
        private Fixture _fixture;

        public PaymentGatewayControllerTests()
        {
            _fixture = new Fixture();
            _mockOrchestator = new Mock<IPaymentOrchestrator>();
            _mockLogger = new Mock<ILogger<PaymentGatewayController>>();

            _sut = new PaymentGatewayController(_mockOrchestator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GivenARetrievePaymentRequest_WhenProcessingIt_ShouldPassItToThePaymentOrchestrator()
        {
            // given
            var submitPaymentRequest = _fixture.Create<RetrievePaymentRequest>();
            var retrievePaymentResponse = _fixture.Create<Checkout.Services.Models.PaymentReadModel>();
            _mockOrchestator.Setup(x => x.RetrievePayment(It.IsAny<Checkout.Services.Models.RetrievePaymentRequest>()))
                .Returns(retrievePaymentResponse);

            // when
            await _sut.Get(submitPaymentRequest, Guid.NewGuid().ToString());

            // then
            _mockOrchestator.Verify(x => x.RetrievePayment(It.Is<Checkout.Services.Models.RetrievePaymentRequest>(y =>
                y.PaymentId == submitPaymentRequest.PaymentId &&
                y.MerchantId == submitPaymentRequest.MerchantId
            )));
        }

        [Fact]
        public async Task GivenAsubmitPaymentRequest_WhenProcessingIt_ShouldPassItToThePaymentOrchestrator()
        {
            // given
            var submitPaymentRequest = _fixture.Create<SubmitPaymentRequest>();
            var bankPaymentResponse = _fixture.Create<BankPaymentResponse>();
            _mockOrchestator.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                .Returns(bankPaymentResponse);

            // when
            await _sut.Post(submitPaymentRequest, Guid.NewGuid().ToString());

            // then
            _mockOrchestator.Verify(x => x.ProcessPayment(It.Is<BankPaymentRequest>(y =>
                y.Amount == submitPaymentRequest.Amount && 
                y.CardDetails.CVV == submitPaymentRequest.CardDetails.CVV &&
                y.CardDetails.CardNumber == submitPaymentRequest.CardDetails.CardNumber &&
                y.CardDetails.ExpiryDate == submitPaymentRequest.CardDetails.ExpiryDate &&
                y.CardDetails.Currency == submitPaymentRequest.CardDetails.Currency &&
                y.MerchantDetails.MerchantId == submitPaymentRequest.MerchantDetails.Id
            )));
        }
    }
}
