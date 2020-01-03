using System;
using Checkout.Repository.LiteDb;
using Checkout.Repository.Models;

namespace Checkout.Repository
{
    public interface IPaymentRepository
    {
        Guid CreatePayment(Payment payment);

        PaymentReadModel RetrievePayment(PaymentRequest paymentRequest);
    }

    public class PaymentRepository : IPaymentRepository
    {
        private readonly ILiteDatabaseWrapper _databaseWrapper;

        public PaymentRepository(ILiteDatabaseWrapper databaseWrapper)
        {
            _databaseWrapper = databaseWrapper;
        }

        public Guid CreatePayment(Payment payment)
        {
            _databaseWrapper.Insert(payment);

            return payment.Id;
        }

        public PaymentReadModel RetrievePayment(PaymentRequest paymentRequest)
        {
            var payment = _databaseWrapper.Get(paymentRequest);

            return PaymentReadModel.BuildPaymentReadModel(payment);
        }
    }

    public class PaymentReadModel
    {
        public long CardNumber { get; private set; }

        public string ExpiryDate { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public int CVV { get; private set; }

        public Guid PaymentId { get; private set; }

        public Guid VendorId{ get; private set; }

        internal static PaymentReadModel BuildPaymentReadModel(Payment payment)
        {
            return new PaymentReadModel
            {
                CardNumber = payment.CardDetails.CardNumber,
                ExpiryDate = payment.CardDetails.ExpiryDate,
                CVV = payment.CardDetails.CVV,
                Amount = payment.Amount,
                VendorId = payment.MerchantDetails.Id,
                PaymentId = payment.Id,
                Currency = payment.CardDetails.Currency
            };
        }
    }
}
