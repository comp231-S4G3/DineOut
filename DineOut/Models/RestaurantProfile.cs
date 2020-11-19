using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class RestaurantProfile
    {
        [Key]
        [Column("restaurant_profile_id")]
        public int CustomerId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("passwordhash")]
        public string PasswordHash { get; set; }
        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
