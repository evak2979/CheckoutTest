using AutoFixture;
using Checkout.Services;
using Checkout.Services.Banks;
using Checkout.Web.Controllers;
using Checkout.Web.Models;
using Moq;
using Xunit;

namespace Checkout.Tests.Unit.Controllers
{
    public class PaymentGatewayControllerTests
    {
        private PaymentGatewayController _sut;
        private Mock<IPaymentOrchestrator> _mockOrchestator;
        private Fixture _fixture;

        public PaymentGatewayControllerTests()
        {
            _fixture = new Fixture();
            _mockOrchestator = new Mock<IPaymentOrchestrator>();

            _sut = new PaymentGatewayController(_mockOrchestator.Object);
        }

        [Fact]
        public void GivenAPaymentRequest_WhenProcessingIt_ShouldPassItToThePaymentOrchestrator()
        {
            // given
            var submitPaymentRequest = _fixture.Create<SubmitPaymentRequest>();
            var bankPaymentResponse = _fixture.Create<BankPaymentResponse>();
            _mockOrchestator.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                .Returns(bankPaymentResponse);

            // when
            _sut.Post(submitPaymentRequest);

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
