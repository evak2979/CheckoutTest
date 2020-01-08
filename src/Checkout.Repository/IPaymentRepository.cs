using System;
using Checkout.Repository.Models;

namespace Checkout.Repository
{
    public interface IPaymentRepository
    {
        Guid CreatePayment(PaymentInformation payment, string correlationId);

        PaymentReadModel RetrievePayment(RetrievePaymentRequest paymentRequest, string correlationId);
    }
}