using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;

namespace DineOut.Controllers
{
    public class OrderDetailsController:Controller
    {
        DineOutContext DineOutContext = new DineOutContext();
        
        public ViewResult Order( Order orderId)
        {
            AllModels allModels = new AllModels();

            int your_order_id = 1; //OrderId
            List<OrderItem> orderItems = DineOutContext.Order_Item.ToList().FindAll(x => x.OrderId == your_order_id);
            List<Item> items = new List<Item>();
            foreach (OrderItem orderItem in orderItems)
            {
                Item orderd_item = DineOutContext.Item.Find(orderItem.ItemId);
                items.Add(orderd_item);
            }
            allModels.Items = items;
            allModels.Orders[0] = DineOutContext.Order.Find(your_order_id);

            return View(allModels);
        }
        public ViewResult ChangeStatus(Order orderId)
        {
            return View();
            
        }
    }
}
