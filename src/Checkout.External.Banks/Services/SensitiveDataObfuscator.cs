using System.Text;
using Checkout.Repository.Models;

namespace Checkout.Services.Services
{
    public class SensitiveDataObfuscator : ISensitiveDataObfuscator
    {
        public void Obfuscate(PaymentInformation payment)
        {
            ObfuscateLongNumber(payment);
            ObfuscateCvv(payment);
        }

        private void ObfuscateCvv(PaymentInformation payment)
        {
            payment.CardDetails.CVV = "***";
        }

        private void ObfuscateLongNumber(PaymentInformation payment)
        {
            var obfuscatedNumber = new StringBuilder();

            obfuscatedNumber.Append(payment.CardDetails.CardNumber.Substring(0, 4));
            obfuscatedNumber.Append("********");
            obfuscatedNumber.Append(payment.CardDetails.CardNumber.Substring(12, 4));

            payment.CardDetails.CardNumber = obfuscatedNumber.ToString();
        }
    }
}