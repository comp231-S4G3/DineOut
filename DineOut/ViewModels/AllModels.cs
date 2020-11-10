using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class AllModels
    {
        public List<Customer> Customers { get; set; }
        public List<Item> Items { get; set; }
        public List<Menu> Menus { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderItem> Order_Items { get; set; }
        public List<OrderStatus> OrderStatuses { get; set; }
        public List<Restaurant> Restaurants { get; set; }
    }
}
