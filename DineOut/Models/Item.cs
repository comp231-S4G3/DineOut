using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class Item
    {
        [Key]
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("menu_id")]
        public int MenuId { get; set; }
        [Column("item_name")]
        public string ItemName { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("Ingredients")]
        public string Ingredients { get; set; }
        [Column("price")]
        public double Price { get; set; }
        [Column("image")]
        public string Image { get; set; }
        [Column("availability")]
        public Boolean Availability { get; set; }
        [Column("created_on")]
        public DateTime CreatedOn { get; set; }
        public override string ToString()
        {
            return ItemName;
        }
    }
}
