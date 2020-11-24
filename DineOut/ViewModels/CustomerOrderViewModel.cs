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
        public Item Item { get; set; }
        public Order Order { get; set; }
        public OrderItem OrderItem { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Restaurant Restaurant { get; set; }
        public Menu Menu { get; set; }
        public Customer Customer { get; set; }
    }
}
