using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class PaymentInformation
    {
        [Required(ErrorMessage ="This field is requried")]
        public string CardOwnerFirstName { get; set; }
        [Required(ErrorMessage = "This field is requried")]
        public string CardOwnerLastName { get; set; }
        [Required(ErrorMessage = "This field is requried")]
        public string CardNumber { get; set; }
        [Required(ErrorMessage = "This field is requried")]
        public long ExpirationYear { get; set; }
        [Required(ErrorMessage = "This field is requried")]
        public long ExpirationMonth { get; set; }
        [Required(ErrorMessage = "This field is requried")]
        [DataType(DataType.Password)]
        public string CVV2 { get; set; }
        [Required(ErrorMessage = "This field is requried")]
        public string Buyer_Email { get; set; }
        [Required(ErrorMessage = "This field is requried")]
        public string Amount { get; set; }
        [Required(ErrorMessage = "This field is requried")]
        public string Currency { get; set; }
    }
}
