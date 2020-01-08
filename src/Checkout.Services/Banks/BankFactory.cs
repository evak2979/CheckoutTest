using System;

namespace Checkout.Services.Banks
{
    public sealed class BankFactory : IBankFactory
    {
        public IBank Create(Guid vendorId)
        {
            return new DummyBank.DummyBank();
        }
    }
}
