using Checkout.Repository;
using Checkout.Repository.Helpers;
using Checkout.Repository.Models;
using Checkout.Services.Banks;
using Microsoft.Extensions.Logging;


namespace Checkout.Services.Services
{
    public sealed class PaymentOrchestrator : IPaymentOrchestrator
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankFactory _bankFactory;
        private readonly ISensitiveDataObfuscator _dataObfuscator;
        private readonly ILogger<PaymentOrchestrator> _logger;

        public PaymentOrchestrator(IPaymentRepository paymentRepository, IBankFactory bankFactory, ISensitiveDataObfuscator dataObfuscator, ILogger<PaymentOrchestrator> logger)
        {
            _paymentRepository = paymentRepository;
            _bankFactory = bankFactory;
            _dataObfuscator = dataObfuscator;
            _logger = logger;
        }

        public BankPaymentResponse ProcessPayment(BankPaymentRequest bankPaymentRequest)
        {
            _logger.LogInformation($"Sending Payment Request with correlationId {bankPaymentRequest.CorrelationId} to bank ");
            var vendorBank = _bankFactory.Create(bankPaymentRequest.MerchantDetails.MerchantId);
            var bankPaymentResponse = vendorBank.ProcessPayment(bankPaymentRequest);

            _paymentRepository.CreatePayment(GenerateRepositoryPayment(bankPaymentRequest, bankPaymentResponse), bankPaymentRequest.CorrelationId);

            return bankPaymentResponse;
        }

        public Models.PaymentReadModel RetrievePayment(Models.RetrievePaymentRequest paymentRequest)
        {
            var repoPaymentRequest =
                new RetrievePaymentRequest(paymentRequest.PaymentId, paymentRequest.MerchantId, paymentRequest.CorrelationId);

            var paymentRequestResponse = _paymentRepository.RetrievePayment(repoPaymentRequest, paymentRequest.CorrelationId);

            if (paymentRequestResponse == null)
                return null;

            return Models.PaymentReadModel.BuildPaymentReadModel(paymentRequestResponse);
        }

        private PaymentInformation GenerateRepositoryPayment(BankPaymentRequest bankPaymentRequest, BankPaymentResponse bankPaymentResponse)
        {
            var payment = new PaymentInformation
            {
                Amount = bankPaymentRequest.Amount,
                Id = bankPaymentResponse.PaymentId,
                CardDetails = new CardDetails(bankPaymentRequest.CardDetails.CardNumber.ToString(), bankPaymentRequest.CardDetails.ExpiryDate,
                    bankPaymentRequest.CardDetails.Currency, bankPaymentRequest.CardDetails.CVV.ToString()),
                MerchantDetails = new MerchantDetails(bankPaymentRequest.MerchantDetails.MerchantId),
                PaymentStatus = bankPaymentResponse.BankPaymentResponseStatus == BankPaymentResponseStatus.Unsuccessful ? "Unsuccessful" : "Successful"
            };

            payment.CardDetails.Obfuscate(_dataObfuscator);

            return payment;
        }
    }
}