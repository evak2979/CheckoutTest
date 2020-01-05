using System;
using AutoFixture;
using Checkout.Repository;
using Checkout.Repository.Models;
using Checkout.Services.Banks;
using Checkout.Services.Services;
using Moq;
using Xunit;

namespace Checkout.Tests.Unit.Services
{
    public class PaymentOrchestratorTests
    {
        private Mock<IPaymentRepository> _mockPaymentRepository;
        private Mock<IBankFactory> _mockBankFactory;
        private Mock<IBank> _mockBank;
        private Mock<ISensitiveDataObfuscator> _mockObfuscator;
        private PaymentOrchestrator _sut;
        private Fixture _fixture;

        public PaymentOrchestratorTests()
        {
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _mockBankFactory = new Mock<IBankFactory>();
            _mockBank = new Mock<IBank>();
            _mockObfuscator = new Mock<ISensitiveDataObfuscator>();

            _fixture = new Fixture();

            _mockBankFactory.Setup(x => x.Create(It.IsAny<Guid>()))
                .Returns(_mockBank.Object);

            _sut = new PaymentOrchestrator(_mockPaymentRepository.Object, _mockBankFactory.Object,
                _mockObfuscator.Object);
        }

        [Fact]
        public void GivenAPaymentRequest_WhenProcessingAPayment_ShouldRetrieveTheVendorsBankDetails()
        {
            // given
            var paymentRequest = _fixture.Create<BankPaymentRequest>();
            var paymentResponse = new BankPaymentResponse
            {
                PaymentId = Guid.NewGuid()
            };
            _mockBank.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                .Returns(paymentResponse);

            // when
            _sut.ProcessPayment(paymentRequest);

            // then
            _mockBankFactory.Verify(x => x.Create(paymentRequest.MerchantDetails.MerchantId));
        }

        [Fact]
        public void GivenAPaymentRequest_WhenProcessingAPayment_ShouldAskTheVendorBankToProcessThePayment()
        {
            // given
            var paymentRequest = _fixture.Create<BankPaymentRequest>();
            var paymentResponse = new BankPaymentResponse
            {
                PaymentId = Guid.NewGuid()
            };
            _mockBank.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                .Returns(paymentResponse);

            // when
            _sut.ProcessPayment(paymentRequest);

            // then
            _mockBank.Verify(x => x.ProcessPayment(paymentRequest));
        }

        [Fact]
        public void GivenAPaymentRequest_WhenProcessingAPayment_ShouldPersistThePaymentInformationToOurDatastore()
        {
            // given
            var paymentRequest = _fixture.Create<BankPaymentRequest>();
            var paymentResponse = new BankPaymentResponse
            {
                PaymentId = Guid.NewGuid()
            };
            _mockBank.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                .Returns(paymentResponse);

            // when
            _sut.ProcessPayment(paymentRequest);

            // then
            _mockPaymentRepository.Verify(x => x.CreatePayment(It.Is<Payment>(y => 
                y.Id == paymentResponse.PaymentId && 
                y.Amount == paymentRequest.Amount && 
                y.CardDetails.CVV == paymentRequest.CardDetails.CVV.ToString() && 
                y.CardDetails.CardNumber == paymentRequest.CardDetails.CardNumber.ToString() && 
                y.CardDetails.ExpiryDate == paymentRequest.CardDetails.ExpiryDate && 
                y.CardDetails.Currency == paymentRequest.CardDetails.Currency)));
        }
    }
}
