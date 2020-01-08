using System;
using Checkout.Repository.LiteDb;
using Checkout.Repository.Models;
using Microsoft.Extensions.Logging;

namespace Checkout.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ILiteDatabaseWrapper _databaseWrapper;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(ILiteDatabaseWrapper databaseWrapper, ILogger<PaymentRepository> logger)
        {
            _databaseWrapper = databaseWrapper;
            _logger = logger;
        }

        public Guid CreatePayment(PaymentInformation payment, string correlationId)
        {
            _logger.LogInformation($"Persisting payment with correlationId {correlationId} locally");
            _databaseWrapper.Insert(payment);

            return payment.Id;
        }
        
        public PaymentReadModel RetrievePayment(RetrievePaymentRequest paymentRequest, string correlationId)
        {
            _logger.LogInformation($"retrieving payment payment with correlationId {correlationId} locally");

            var payment = _databaseWrapper.Get(paymentRequest);

            _logger.LogWarning($"Payment with Id {paymentRequest.PaymentId} && merchant Id {paymentRequest.MerchantId} not found!");
            
            if (payment == null)
                return null;

            return PaymentReadModel.BuildPaymentReadModel(payment);
        }
    }
}
