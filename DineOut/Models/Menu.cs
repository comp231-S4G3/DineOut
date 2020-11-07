using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class Menu
    {
        [Key]
        [Column("menu_id")]
        public int MenuId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("restaurant_id")]
        public int RestaurantId { get; set; }
        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
        public override string ToString()
        {
            return Title;
        }
    }
}
