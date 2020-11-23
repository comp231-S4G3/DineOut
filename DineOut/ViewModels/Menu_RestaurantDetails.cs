using DineOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.ViewModels
{
    public class Menu_RestaurantDetails
    {
        public List<Item> Items { get; set; }
        public Restaurant Restaurant { get; set; }

    }
}