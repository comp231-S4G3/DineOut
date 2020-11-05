using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public String OwnerFirstName { get; set; }
        public String OwnerLastName { get; set; }
        public String RestaurantName { get; set; }
        public String QRCode { get; set; }

    }
}
