using System;
using AutoFixture;
using Checkout.Repository;
using Checkout.Repository.Helpers;
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
        public void GivenAProcessPaymentRequest_WhenProcessingAProcessPayment_ShouldRetrieveTheVendorsBankDetails()
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
        public void GivenAProcessPaymentRequest_WhenProcessingAProcessPayment_ShouldAskTheVendorBankToProcessThePayment()
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
        public void GivenAProcessPaymentRequest_WhenProcessingAProcessPayment_ShouldPersistThePaymentInformationToOurDatastore()
        {
            // given
            var returnedCVV = "*";
            var returnedCardNumber = "**";
            var paymentRequest = _fixture.Create<BankPaymentRequest>();
            var paymentResponse = new BankPaymentResponse
            {
                PaymentId = Guid.NewGuid()
            };
            _mockBank.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentRequest>()))
                .Returns(paymentResponse);
            _mockObfuscator.Setup(x => x.ObfuscateCvv(paymentRequest.CardDetails.CVV.ToString())).Returns(returnedCVV);
            _mockObfuscator.Setup(x => x.ObfuscateLongCardNumber(paymentRequest.CardDetails.CardNumber.ToString())).Returns(returnedCardNumber);

            // when
            _sut.ProcessPayment(paymentRequest);

            // then
            _mockPaymentRepository.Verify(x => x.CreatePayment(It.Is<PaymentInformation>(y => 
                y.Id == paymentResponse.PaymentId && 
                y.Amount == paymentRequest.Amount && 
                y.CardDetails.CVV == returnedCVV && 
                y.CardDetails.CardNumber == returnedCardNumber && 
                y.CardDetails.ExpiryDate == paymentRequest.CardDetails.ExpiryDate && 
                y.CardDetails.Currency == paymentRequest.CardDetails.Currency)));
        }

        [Fact]
        public void GivenARetrievePaymentRequest_WhenRetrievingPayment_ShouldCallRepository()
        {
            // given
            var payment =
                PaymentReadModel.BuildPaymentReadModel(_fixture.Create<PaymentInformation>());

            var paymentRequest = _fixture.Create<Checkout.Services.Models.RetrievePaymentRequest>();
            _mockPaymentRepository.Setup(x => x.RetrievePayment(It.IsAny<RetrievePaymentRequest>()))
                .Returns(payment);

            // when
            _sut.RetrievePayment(paymentRequest);

            // then
            _mockPaymentRepository.Verify(x => x.RetrievePayment(It.Is<RetrievePaymentRequest>(y => y.PaymentId == paymentRequest.PaymentId &&
                                                                                            y.MerchantId == paymentRequest.MerchantId)));
        }
    }
}
