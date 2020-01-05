using System;
using Checkout.Repository.LiteDb;
using Checkout.Repository.Models;

namespace Checkout.Repository
{
    public interface IPaymentRepository
    {
        Guid CreatePayment(PaymentInformation payment);

        PaymentReadModel RetrievePayment(RetrievePaymentRequest paymentRequest);
    }

    public class PaymentRepository : IPaymentRepository
    {
        private readonly ILiteDatabaseWrapper _databaseWrapper;

        public PaymentRepository(ILiteDatabaseWrapper databaseWrapper)
        {
            _databaseWrapper = databaseWrapper;
        }

        public Guid CreatePayment(PaymentInformation payment)
        {
            _databaseWrapper.Insert(payment);

            return payment.Id;
        }

        public PaymentReadModel RetrievePayment(RetrievePaymentRequest paymentRequest)
        {
            var payment = _databaseWrapper.Get(paymentRequest);

            return PaymentReadModel.BuildPaymentReadModel(payment);
        }
    }
}
