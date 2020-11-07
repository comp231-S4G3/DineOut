using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class OrderItem
    {
        [Key]
        [Column("order_item_id")]
        public int OrderItemId { get; set; }
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
    }
}
