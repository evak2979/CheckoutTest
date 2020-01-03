namespace Checkout.Web.Models
{
    /// <summary>
    /// Information on an existing payment
    /// </summary>
    public class RetrievePaymentResponse
    {
        public CardDetails CardDetails { get; set; }

        public PaymentResponseStatus PaymentResponseStatus { get; set; }

        /// <summary>
        /// Payment amount
        /// </summary>
        public decimal Amount { get; set; }
    }
}