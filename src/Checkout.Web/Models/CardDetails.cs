using System.ComponentModel.DataAnnotations;

namespace Checkout.Web.Models
{
    /// <summary>
    /// Information regarding a credit card
    /// </summary>
    public sealed class CardDetails
    {
        /// <summary>
        /// The credit card long number (must be 16 digits long)
        /// </summary>
        [Required]
        [DataType(DataType.CreditCard)]
        public long CardNumber { get; set; }

        /// <summary>
        /// The credit card expiration date
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public string ExpiryDate { get; set; }

        /// <summary>
        /// The currency type (e.g. pound)
        /// </summary>
        [Required]
        public string Currency { get; set; }

        /// <summary>
        /// The CVV code of the credit card (must be 3 digits long)
        /// </summary>
        [Required]
        public int CVV { get; set; }
    }
}
