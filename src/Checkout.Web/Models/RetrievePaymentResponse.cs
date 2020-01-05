namespace Checkout.Web.Models
{
    /// <summary>
    /// Information on an existing payment
    /// </summary>
    public sealed class RetrievePaymentResponse
    {
        public string CardNumber { get; set; }

        /// <summary>
        /// The credit card expiration date
        /// </summary>
        public string ExpiryDate { get; set; }

        /// <summary>
        /// The currency type (e.g. pound)
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// The CVV code of the credit card (must be 3 digits long)
        /// </summary>
        public string CVV { get; set; }
        
        /// <summary>
        /// Whether a payment was successful
        /// </summary>
        public string PaymentResponseStatus { get; set; }

        /// <summary>
        /// Payment amount
        /// </summary>
        public decimal Amount { get; set; }
    }
}