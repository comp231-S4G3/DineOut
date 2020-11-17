using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.ViewModels
{
    public class ItemViewModel
    {
        public int ItemId { get; set; }
        public int MenuId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public double Price { get; set; }
        public IFormFile Image { get; set; }
        public string ImagePath { get; set; }
        public Boolean Availability { get; set; }
        public DateTime CreatedOn { get; set; }
        public override string ToString()
        {
            return ItemName;
        }
    }
}
