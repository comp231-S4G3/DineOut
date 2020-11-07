using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class OrderStatus
    {
        [Key]
        [Column("orderStatus_id")]
        public int OrderStatusId { get; set; }
        [Column("status")]
        public string Status { get; set; }
    }
}
