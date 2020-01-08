using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.Web.Models
{
    /// <summary>
    /// Information regarding a particular Merchant
    /// </summary>
    public sealed class MerchantDetails
    {
        /// <summary>
        /// The Merchant's Unique Identifier
        /// </summary>
        [Required]
        public Guid MerchantId { get; set; }
    }
}