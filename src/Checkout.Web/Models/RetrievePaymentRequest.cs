using System;
using System.ComponentModel.DataAnnotations;
using Checkout.Web.Models.Base;

namespace Checkout.Web.Models
{
    /// <summary>
    /// Request to retrieve an existing payment
    /// </summary>
    public sealed class RetrievePaymentRequest : BaseRequest
    {
        /// <summary>
        /// A payment's unique identifier
        /// </summary>
        [Required]
        public Guid PaymentId { get; set; }

        /// <summary>
        /// A vendor's unique identifier
        /// </summary>
        [Required]
        public Guid MerchantId { get; set; }
    }
}