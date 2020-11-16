using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Models
{
    public class CustomerOrder
    {
        private List<OrderItem> itemCollection = new List<OrderItem>();

        public void AddItem(Item item, int quantity)
        {
            //Check if the item is already exist
            OrderItem line = itemCollection
                .Where(p => p.Item.ItemId == item.ItemId)
                .FirstOrDefault();

            //Add a new OrderItem if line is null. If it's not null, just add quantity
            if (line == null)
            {
                itemCollection.Add(new OrderItem
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

        public void RemoveLine(Item product) =>
            itemCollection.RemoveAll(l => l.Item.ItemId == product.ItemId);

        public double ComputeTotalValue() =>
            itemCollection.Sum(e => e.Item.Price * e.Quantity);

        public void Clear() => itemCollection.Clear();

        //Return all OrderItems
        public IEnumerable<OrderItem> Lines => itemCollection;

    }
}
