﻿using Checkout.Repository;
using Checkout.Repository.Helpers;
using Checkout.Repository.Models;
using Checkout.Services.Banks;


namespace Checkout.Services.Services
{
    public sealed class PaymentOrchestrator : IPaymentOrchestrator
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankFactory _bankFactory;
        private readonly ISensitiveDataObfuscator _dataObfuscator;

        public PaymentOrchestrator(IPaymentRepository paymentRepository, IBankFactory bankFactory, ISensitiveDataObfuscator dataObfuscator)
        {
            _paymentRepository = paymentRepository;
            _bankFactory = bankFactory;
            _dataObfuscator = dataObfuscator;
        }

        public BankPaymentResponse ProcessPayment(BankPaymentRequest bankPaymentRequest)
        {
            var vendorBank = _bankFactory.Create(bankPaymentRequest.MerchantDetails.MerchantId);
            var bankPaymentResponse = vendorBank.ProcessPayment(bankPaymentRequest);

            _paymentRepository.CreatePayment(GenerateRepositoryPayment(bankPaymentRequest, bankPaymentResponse));

            return bankPaymentResponse;
        }

        public Models.PaymentReadModel RetrievePayment(Models.RetrievePaymentRequest paymentRequest)
        {
            var repoPaymentRequest =
                new Repository.Models.RetrievePaymentRequest(paymentRequest.PaymentId, paymentRequest.MerchantId);

            var paymentRequestResponse = _paymentRepository.RetrievePayment(repoPaymentRequest);

            return Models.PaymentReadModel.BuildPaymentReadModel(paymentRequestResponse);
        }

        private PaymentInformation GenerateRepositoryPayment(BankPaymentRequest bankPaymentRequest, BankPaymentResponse bankPaymentResponse)
        {
            var payment = new PaymentInformation
            {
                Amount = bankPaymentRequest.Amount,
                Id = bankPaymentResponse.PaymentId,
                CardDetails = new Repository.Models.CardDetails(bankPaymentRequest.CardDetails.CardNumber.ToString(), bankPaymentRequest.CardDetails.ExpiryDate,
                    bankPaymentRequest.CardDetails.Currency, bankPaymentRequest.CardDetails.CVV.ToString()),
                MerchantDetails = new Repository.Models.MerchantDetails(bankPaymentRequest.MerchantDetails.MerchantId),
                PaymentStatus = bankPaymentResponse.BankPaymentResponseStatus == BankPaymentResponseStatus.Unsuccessful ? "Unsuccessful" : "Successful"
            };

            payment.CardDetails.Obfuscate(_dataObfuscator);

            return payment;
        }
    }
}