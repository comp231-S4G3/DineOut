using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineOut.Models;

namespace DineOut.ViewModels
{
    public class OwnerMenuViewModel
    {
        public IEnumerable<Item> items { get; set; }

        public IEnumerable<Restaurant> restaurants { get; set; }

        public Item Item { get; set; }

        public Restaurant restaurant { get; set; }

        public static implicit operator List<object>(OwnerMenuViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
