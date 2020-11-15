using DineOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.ViewModels
{
    public class CustomerOrderViewModel
    {
        public List<Item> Items { get; set; }
        public Order Orders { get; set; }
        public Restaurant Restaurants { get; set; }
    }
}
