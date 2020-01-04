using System;

namespace Checkout.Services.Banks
{
    public interface IBankFactory
    {
        IBank Create(Guid vendorId);
    }
}