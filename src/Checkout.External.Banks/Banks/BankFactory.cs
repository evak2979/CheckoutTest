using System;

namespace Checkout.Services.Banks
{
    public class BankFactory : IBankFactory
    {
        public IBank Create(Guid vendorId)
        {
            return new DummyBank.DummyBank();
        }
    }
}
