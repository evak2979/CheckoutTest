namespace Checkout.Repository.Helpers
{
    public interface ISensitiveDataObfuscator
    {
        string ObfuscateCvv(string cvv);

        string ObfuscateLongCardNumber(string cardNumber);
    }
}