using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class Order
    {
        [Key]
        [Column("order_id")]
        public int OrderId { get; set; }
        [Column("restaurant_id")]
        public int RestaurantId { get; set; }
        [Column("customer_id")]
        public int CustomerId { get; set; }
        [Column("note")]
        public string Note { get; set; }
        [Column("table_num")]
        public string TableNum { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("order_date")]
        public DateTime CreatedOn { get; set; }
        public override string ToString()
        {
            return OrderId.ToString();
        }
    }
}
