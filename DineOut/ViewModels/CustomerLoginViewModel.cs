using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineOut.Models;

namespace DineOut.ViewModels
{
    public class CustomerLoginViewModel
    {
        public Customer Customer { get; set; }
        public int MenuId { get; set; }
        public int RestaurantId { get;set; }
    }
}
