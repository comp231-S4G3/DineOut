using DineOut.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public virtual void AddItem(Item item, int quantity)
        {
            CartLine line = lineCollection
            .Where(i => i.Item.ItemId == item.ItemId)
            .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Item = item,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Item item) =>
        lineCollection.RemoveAll(l => l.Item.ItemId == item.ItemId);
        public virtual double ComputeTotalValue() =>
        lineCollection.Sum(e => e.Item.Price * e.Quantity);
        public virtual void Clear() => lineCollection.Clear();
        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}

