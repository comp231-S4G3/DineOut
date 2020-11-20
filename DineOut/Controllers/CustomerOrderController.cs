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

        public IActionResult OrderDetails(int menuId, int customerId, int restaurantId)
        {
            //menu_id and customer_id is hard coded for testing purpose
            menuId = 1;
            customerId = 1;
            restaurantId = 1;

            Order nOrder = new Order
            {
                CustomerId = customerId, //newOrder.Customer.CustomerId
                RestaurantId = restaurantId,
                StatusId = 1
            };
            DineOutContext.Add(nOrder);
            DineOutContext.SaveChanges();

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
            Order nOrder = new Order
            {
                OrderId = orderId,
                CustomerId = customerId
            };

            OrderItem orderItem = new OrderItem
            {
                OrderId = orderId,
                ItemId = order.Item.ItemId
            };

            if (ModelState.IsValid)
            {
                //Check if the same orderId already exists
                if (DineOutContext.Order_Item
                    .Any(x => x.OrderId == orderItem.OrderId))
                {
                    if (DineOutContext.Order_Item
                        .Any(x => x.OrderItemId == orderItem.OrderItemId)
                        )
                    {
                        orderItem.Quantity += quantity;
                        DineOutContext.Order_Item.Update(orderItem);
                        DineOutContext.SaveChanges();
                    }
                    else
                    {
                        orderItem.Quantity = quantity;
                        DineOutContext.Order_Item.Add(orderItem);
                        DineOutContext.SaveChanges();
                    }
                }
                else
                {
                    orderItem.Quantity = quantity;
                    DineOutContext.Order_Item.Add(orderItem);
                    DineOutContext.SaveChanges();
                }

                orderData.Order = DineOutContext.Order.Find(nOrder.OrderId);
                orderData.Customer
                    = DineOutContext.Customer.Find(nOrder.CustomerId);
                orderData.Menu = DineOutContext.Menu.Find(order.Item.MenuId);
                orderData.Items = DineOutContext.Item
                    .ToList().FindAll(x => x.MenuId == order.Item.MenuId);
                orderData.Restaurant = DineOutContext.Restaurant
                    .Find(orderData.Menu.RestaurantId);

                TempData["message"] = "Item is added";
                return View("OrderDetails", orderData);
            }
            else
            {
                orderData.Order = DineOutContext.Order.Find(nOrder.OrderId);
                orderData.Customer
                    = DineOutContext.Customer.Find(nOrder.CustomerId);
                orderData.Menu = DineOutContext.Menu.Find(order.Item.MenuId);
                orderData.Items = DineOutContext.Item
                    .ToList().FindAll(x => x.MenuId == order.Item.MenuId);
                orderData.Restaurant = DineOutContext.Restaurant
                    .Find(orderData.Menu.RestaurantId);

                TempData["message"] = "Sorry, there is an error. Plese try it again.";
                return View("OrderDetails", orderData);
            }
        }

        //Display order summary
        public IActionResult OrderSummary(int orderId, int restaurantId, int customerId)
        {
            List<Item> items = new List<Item>();
            Order nOrder = new Order
            {
                OrderId = orderId,
                RestaurantId = restaurantId,
                CustomerId = customerId
            };

            orderData.Order = DineOutContext.Order.Find(nOrder.OrderId);

            orderData.OrderItem = DineOutContext.Order_Item.Find(nOrder.OrderId);
            orderData.OrderItems = DineOutContext.Order_Item
                .ToList().FindAll(x => x.OrderId == nOrder.OrderId);
            orderData.Restaurant = DineOutContext.Restaurant
                .Find(nOrder.RestaurantId);
            orderData.Restaurant = DineOutContext.Restaurant
                .Find(nOrder.RestaurantId);
            orderData.Customer = DineOutContext.Customer
                .Find(nOrder.CustomerId);
            foreach (OrderItem orderItem in orderData.OrderItems)
            {
                orderItem.Item = DineOutContext.Item.Find(orderItem.ItemId);
            }

            return View(orderData);
        }
    }
}
