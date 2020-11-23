using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineOut.Models;

namespace DineOut.ViewModels
{
    public class ProfileViewModel
    {
        public RestaurantProfile restaurantProfile { get; set; }

        public Restaurant restaurant { get; set; }
    }
}
