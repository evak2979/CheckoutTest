using System.ComponentModel.DataAnnotations;
using Checkout.Web.Models.Base;

namespace Checkout.Web.Models
{
    /// <summary>
    /// Request for a new payment to be made
    /// </summary>
    public sealed class SubmitPaymentRequest : BaseRequest
    {
        public MerchantDetails MerchantDetails { get; set; }

        public CardDetails CardDetails { get; set; }

        /// <summary>
        /// The amount to be paid
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}