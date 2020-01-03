using System;
using System.ComponentModel.DataAnnotations;

namespace Checkout.Web.Models
{
    /// <summary>
    /// Information regarding a particular Merchant
    /// </summary>
    public class MerchantDetails
    {
        /// <summary>
        /// The Merchant's Unique Identifier
        /// </summary>
        [Required]
        public Guid Id { get; set; }
    }
}