using Checkout.Repository.Models;

namespace Checkout.Services.Services
{
    public interface ISensitiveDataObfuscator
    {
        void Obfuscate(PaymentInformation bankPaymentResponse);
    }
}