using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public String Title { get; set; }
        public int RestaurantId { get; set; }
    }
}
