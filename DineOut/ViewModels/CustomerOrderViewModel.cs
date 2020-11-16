﻿using DineOut.Models;
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
        public Restaurant Restaurant { get; set; }
        public Menu Menu { get; set; }
        public List<Customer> Customers { get; set; }
        public List<CustomerOrder> CustomerOrders { get; set; }
        public CustomerOrder CustomerOrder { get; set; }


    }
}