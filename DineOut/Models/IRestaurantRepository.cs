using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    interface IRestaurantRepository
    {
        IQueryable<Restaurant> Restaurants { get; }

    }
}
