using DineOut.Models;
using DineOut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Controllers
{
    public class RestaurantController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();

        public RestaurantController ()
        {
            
        }


        //This belongs to Order Controller
        //Added here, so we do not forget
        /*public IActionResult CurrentOrders() 
        {
            return View();
        }*/

        //This belongs to Order Controller
        //Added here, so we do not forget
        /*public IActionResult CompletedOrders()
        {
            return View();
        }*/

        public IActionResult Menu(int menuID, DateTime created)
        {
            return View(DineOutContext.Item
                .Where(m => m.MenuId == menuID)
                .OrderBy(i => i.CreatedOn == created));
        }

        //Action Created by Ederson for OrderDetails OwnerSide
        public ViewResult OrderDetails(int orderId)
        {
            orderId = 1; // orderId hard coded fot testing proposes
            OrderDetailsInfo orderDetailsInfo = new OrderDetailsInfo(); // this a new view model 
            List<Item> items = new List<Item>();
            orderDetailsInfo.order = DineOutContext.Order.Find(orderId);
            orderDetailsInfo.OrderItems = DineOutContext.Order_Item.ToList().FindAll(x => x.OrderId == orderId);
            foreach (OrderItem orderItem in orderDetailsInfo.OrderItems)
            {
                Item orderd_item = DineOutContext.Item.Find(orderItem.ItemId);
                items.Add(orderd_item);
            }
            orderDetailsInfo.Items = items;
            return View(orderDetailsInfo);
        }

        //Create Profile Action bellow
    }
}
