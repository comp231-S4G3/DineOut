using DineOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.ViewModels
{
    public class OrderDetailsInfo
    {
        public List<Item> Items { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Order order { get; set; }

    }
}
