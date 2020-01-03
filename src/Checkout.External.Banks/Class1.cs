using System;

namespace Checkout.External.Banks
{
    public interface IBankFactory
    {
        IBank Create(Guid vendorId);
    }

    public class BankFactory : IBankFactory
    {
        public IBank Create(Guid vendorId)
        {
            return new DummyBank();
        }
    }

    public interface IBank
    {
        BankPaymentResponse SubmitPayment(BankPaymentRequest bankPaymentRequest);
    }

    public class DummyBank : IBank
    {
        public BankPaymentResponse SubmitPayment(BankPaymentRequest bankPaymentRequest)
        {
            throw new NotImplementedException();
        }
    }

    public class BankPaymentRequest
    {
        public MerchantDetails MerchantDetails { get; set; }

        public CardDetails CardDetails { get; set; }
    }

    public class BankPaymentResponse
    {
        public string Guid { get; set; }

        public BankPaymentResponseStatus BankPaymentResponseStatus { get; set; }
    }

    public enum BankPaymentResponseStatus
    {
        Successful,
        Unsuccessful
    }
}
