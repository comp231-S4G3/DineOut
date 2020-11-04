using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public int MenuId { get; set; }
        public String ItemName { get; set; }
        public String Description { get; set; }
        public String Ingredients { get; set; }
        public double Price { get; set; }
        public String Image { get; set; }
        public Boolean Availability { get; set; }
    }
}
