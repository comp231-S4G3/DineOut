using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;
using DineOut.Infrastructure;

namespace DineOut.Controllers
{
    public class CustomerOrderController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();

        //Display selected orderable menu 
        public ViewResult OrderDetails(int menu_id)
        {
            return View(DineOutContext.Menu
                .Where(p => p.MenuId == menu_id));
        }

        //Create a new order
        /*[HttpPost]
        public IActionResult CreateOrder(Order order)
        {
            //Define how do we get customer Id when we get  to the controller
            // Define how do we get restaurant Id as well
            int customerId =  1;
            int restaurantId = 1;

            if (ModelState.IsValid)
            {
                Order nOrder = new Order();
                nOrder.CustomerId = customerId;
                nOrder.RestaurantId = restaurantId;

                DineOutContext.Order.Add(order);

                Order newOrder = DineOutContext.Order.Find(order);
                //Is it the right way to set an opening page and the value at the same time?
                return View("OrderSummary", newOrder.OrderId);
            }
            else
            {
                TempData["message"] = "Sorry, there is an error. Plese try it again.";
                return View("OrderDetails");
            }
        }*/

        //Add to CustomerOrder Model
        public IActionResult AddToOrderItem(int itemId)
        {
            Item item = DineOutContext.Item
                .FirstOrDefault(p => p.ItemId == itemId);

            if (item != null)
            {
                CustomerOrder orderItem = GetOrderItem();
                orderItem.AddItem(item, 1);
                SaveOrderItem(orderItem);
            }

            //How do I return a view with reflecting current order status (quentity)?
            return View("OrderDetails");

        }

        public IActionResult RemoveFromOrderItem(int itemId)
        {
            Item item = DineOutContext.Item
                .FirstOrDefault(p => p.ItemId == itemId);

            if (item != null)
            {
                CustomerOrder orderItem = GetOrderItem();
                orderItem.RemoveLine(item);
                SaveOrderItem(orderItem);
            }

            //How do I return a view with reflecting current order status (quentity)?
            return View("OrderDetails");
        }

        private void SaveOrderItem(CustomerOrder item)
        {
            HttpContext.Session.SetJson("OrderItem", item);

        }

        private CustomerOrder GetOrderItem()
        {
            CustomerOrder order =
                HttpContext.Session.GetJson<CustomerOrder>("OrderItem")
                ?? new CustomerOrder();

            return order;
        }

        //Display order summary
        /*public IActionResult OrderSummary(int orderId)
        {
            return View(dineOutContext.Order
                .Where(p => p.OrderId == orderId));
        }*/



    }
}
