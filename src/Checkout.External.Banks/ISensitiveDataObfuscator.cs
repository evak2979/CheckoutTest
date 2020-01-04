using Checkout.Repository.Models;

namespace Checkout.Services
{
    public interface ISensitiveDataObfuscator
    {
        void Obfuscate(Payment bankPaymentResponse);
    }
}