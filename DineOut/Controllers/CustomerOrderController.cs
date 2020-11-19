using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;
using DineOut.ViewModels;
using DineOut.Infrastructure;

namespace DineOut.Controllers
{
    public class CustomerOrderController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();
        CustomerOrderViewModel orderData = new CustomerOrderViewModel();
        OrderDetailsInfo orderDetailsInfo = new OrderDetailsInfo();

        public IActionResult OrderDetails(int menuId, int customerId)
        {
            //menu_id and customer_id is hard coded for testing purpose
            menuId = 1;
            customerId = 1;

            //CustomerOrderViewModel customerOrderView = new CustomerOrderViewModel();
            Order nOrder = new Order();
            nOrder.CustomerId = customerId; //newOrder.Customer.CustomerId

            DineOutContext.Order.Add(nOrder);
            //CustomerOrderViewModel.Order = DineOutContext.Order.Find(customerId);
            orderData.Order
                = DineOutContext.Order.Find(nOrder.OrderId);
            orderData.Customer
               = DineOutContext.Customer.Find(nOrder.CustomerId);
            orderData.Menu = DineOutContext.Menu.Find(menuId);
            orderData.Items = DineOutContext.Item
                .ToList().FindAll(x => x.MenuId == menuId);
            orderData.Restaurant = DineOutContext.Restaurant
                .Find(orderData.Menu.RestaurantId);
            foreach (Item item in orderData.Items)
            {
                orderData.Item = DineOutContext.Item.Find(item.ItemId);
            }
            return View(orderData);
        }

        //Create a new order
        [HttpPost]
        public IActionResult AddItem(CustomerOrderViewModel order,
            int quantity, int orderId, int customerId)
        {
            OrderItem orderItem = new OrderItem
            {
                OrderId = orderId,
                ItemId = order.Item.ItemId
            };

            Order nOrder = new Order
            {
                OrderId = orderId,
                CustomerId = customerId
            };
            DineOutContext.Order.Update(nOrder);

            if (ModelState.IsValid)
            {
                //To check if the item already exists in the order
                CustomerOrderViewModel checkItem = new CustomerOrderViewModel();

                //Check if the same orderId already exists
                if (DineOutContext.Order_Item
                    .Any(x => x.OrderId == orderId))
                {
                    checkItem.OrderItem = DineOutContext.Order_Item.Find(orderItem.OrderId);

                    if (checkItem.OrderItem.ItemId == order.Item.ItemId)
                    {
                        checkItem.OrderItem.Quantity += quantity;
                        DineOutContext.Order_Item.Update(checkItem.OrderItem);
                        DineOutContext.SaveChanges();
                    }
                    else
                    {
                        orderItem.Quantity = quantity;
                        DineOutContext.Order_Item.Add(orderItem);
                    }
                }
                else
                {
                    orderItem.Quantity = quantity;
                    DineOutContext.Order_Item.Add(orderItem);
                }

                orderData.Order = DineOutContext.Order.Find(orderItem.OrderId);
                orderData.Customer
                    = DineOutContext.Customer.Find(nOrder.CustomerId);
                orderData.Menu = DineOutContext.Menu.Find(order.Item.MenuId);
                orderData.Items = DineOutContext.Item
                    .ToList().FindAll(x => x.MenuId == order.Item.MenuId);
                orderData.Restaurant = DineOutContext.Restaurant
                    .Find(orderData.Menu.RestaurantId);

                TempData["message"] = "Item is added";
                return View("OrderSummary", orderData);
            }
            else
            {
                orderData.Order
               = DineOutContext.Order.Find(nOrder.OrderId);
                orderData.Customer
                   = DineOutContext.Customer.Find(nOrder.CustomerId);
                orderData.Menu = DineOutContext.Menu.Find(orderData.Menu.RestaurantId);
                orderData.Items = DineOutContext.Item
                    .ToList().FindAll(x => x.MenuId == orderData.Menu.RestaurantId);
                orderData.Restaurant = DineOutContext.Restaurant
                    .Find(orderData.Menu.RestaurantId);
                foreach (Item item in orderData.Items)
                {
                    orderData.Item = DineOutContext.Item.Find(item.ItemId);
                }

                TempData["message"] = "Sorry, there is an error. Plese try it again.";
                return View("OrderDetails", orderData);
            }
        }

        //Display order summary
        public IActionResult OrderSummary(int orderId)
        {
            List<Item> items = new List<Item>();
            //CustomerOrderViewModel orderData = new CustomerOrderViewModel();
            Order nOrder = new Order
            {
                OrderId = orderId
            };
            DineOutContext.Order.Update(nOrder);

            orderData.Order = DineOutContext.Order.Find(nOrder.OrderId);
            orderData.OrderItems = DineOutContext.Order_Item
                .ToList().FindAll(x => x.OrderId == orderId);

            orderData.OrderItem = DineOutContext.Order_Item.Find(nOrder.OrderId);
            //OrderItem is not found and added even though it can be found in above
            orderData.Restaurant = DineOutContext.Restaurant
                .Find(orderData.Order.RestaurantId);
            foreach (OrderItem orderItem in orderData.OrderItems)
            {
                orderItem.Item = DineOutContext.Item.Find(orderItem.ItemId);
            }

            return View(orderData);
        }



    }
}
