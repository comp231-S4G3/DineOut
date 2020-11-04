using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public int CustomerId { get; set; }
        public String Note { get; set; }
        public DateTime Date { get; set; }
        public String TableNum { get; set; }
        public int StatusId { get; set; }
        
    }
}
