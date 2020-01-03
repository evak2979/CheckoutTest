using System;

namespace Checkout.Web.Models
{
    /// <summary>
    /// Response after a payment has been submitted
    /// </summary>
    public class SubmitPaymentResponse
    {
        /// <summary>
        /// The payment's unique identifier
        /// </summary>
        public Guid PaymentId { get; set; }

        /// <summary>
        /// The merchant's unique identifier
        /// </summary>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// Whether the payment has been made successfully
        /// </summary>
        public PaymentResponseStatus PaymentResponseStatus { get; set; }
    }
}