namespace Checkout.Web.Models
{
    /// <summary>
    /// Representation of a payment's status
    /// </summary>
    public enum PaymentResponseStatus
    {
        /// <summary>
        /// A successful payment
        /// </summary>
        Successful,
        /// <summary>
        /// An unsuccessful payment
        /// </summary>
        Unsuccessful
    }
}