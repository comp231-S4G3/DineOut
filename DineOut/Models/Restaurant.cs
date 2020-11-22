using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class Restaurant
    {
        [Key]
        [Column("restaurant_id")]
        public int RestaurantId { get; set; }
        [Column("restaurant_name")]
        public string RestaurantName { get; set; }
        [Column("QR_Code")]
        public string QRCode { get; set; }
        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
        public override string ToString()
        {
            return RestaurantName;
        }
        [Column("restaurant_profile_id")]
        public int RestaurantProfileId { get; set; }

        public Menu Menu { get; set; }
    }
}
