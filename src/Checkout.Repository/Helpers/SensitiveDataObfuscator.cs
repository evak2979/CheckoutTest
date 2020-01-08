using System.Text;

namespace Checkout.Repository.Helpers
{
    public sealed class SensitiveDataObfuscator : ISensitiveDataObfuscator
    {
        public string ObfuscateCvv(string cvv)
        {
            return "***";
        }

        public string ObfuscateLongCardNumber(string cardNumber)
        {
            var obfuscatedNumber = new StringBuilder();

            obfuscatedNumber.Append(cardNumber.Substring(0, 4));
            obfuscatedNumber.Append("********");
            obfuscatedNumber.Append(cardNumber.Substring(12, 4));

            return obfuscatedNumber.ToString();
        }
    }
}