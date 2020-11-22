using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;
using DineOut.ViewModels;

namespace DineOut.Controllers
{
    public class CustomerOrderController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();
        CustomerOrderViewModel orderData = new CustomerOrderViewModel();

        public IActionResult OrderDetails(
            int menuId, int customerId, int restaurantId)
        {
            //Those parameters are hard coded for testing purpose
            menuId = 1;
            customerId = 1;
            restaurantId = 1;

            Order nOrder = new Order
            {
                CustomerId = customerId,
                RestaurantId = restaurantId,
                StatusId = 1
            };
            DineOutContext.Add(nOrder);
            DineOutContext.SaveChanges();

            orderData.Order
                = DineOutContext.Order.Find(nOrder.OrderId);
            orderData.Menu = DineOutContext.Menu.Find(menuId);
            orderData.Items = DineOutContext.Item
                .ToList().FindAll(x => x.MenuId == menuId);
            orderData.Restaurant = DineOutContext.Restaurant
                .Find(nOrder.RestaurantId);

            return View(orderData);
        }

        //Create a new order
        [HttpPost]
        public IActionResult AddItem(CustomerOrderViewModel order,
            int quantity, int orderId, int customerId, List<OrderItem> orderItems)
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
                    .Any(x => x.OrderId == orderItem.OrderId) == true)
                {
                    OrderItem checkItem = new OrderItem();
                    checkItem = orderItems
                        .Find(x => x.ItemId == orderItem.ItemId);

                    if (orderItem.ItemId == checkItem.ItemId)
                    {
                        checkItem.Quantity += quantity;
                        DineOutContext.Order_Item.Update(checkItem);
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
                orderData.Item = DineOutContext.Item.Find(order.Item.ItemId);
                orderData.OrderItems = DineOutContext.Order_Item
                    .ToList().FindAll(x => x.OrderId == nOrder.OrderId);

                TempData["message"] = $"{orderData.Item.ItemName} is added";
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
                orderData.Item = DineOutContext.Item.Find(order.Item.ItemId);

                TempData["message"] = "Sorry, there is an error. Plese try it again.";
                return View("OrderDetails", orderData);
            }
        }

        //Display order summary
        public IActionResult OrderSummary(int orderId, int restaurantId, int customerId)
        {
            //for testing purpose
            orderId = 1;
            restaurantId = 1;
            customerId = 1;

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

        public IActionResult DeleteItem(int itemId, int orderItemId)
        {
            var item_delete = DineOutContext.Order_Item
                .Where(r => r.OrderItemId == orderItemId)
                .Where(r => r.ItemId == itemId).FirstOrDefault();
            DineOutContext.Remove(item_delete);
            DineOutContext.SaveChanges();

            return RedirectToAction("OrderSummary");
        }

        public IActionResult ChangeQuantity(int itemId, int orderItemId, int quantity)
        {
            var itemUpdate = DineOutContext.Order_Item
                .Where(r => r.OrderItemId == orderItemId)
                .Where(r => r.ItemId == itemId).FirstOrDefault();
            itemUpdate.Quantity = quantity;
            DineOutContext.Update(itemUpdate);
            DineOutContext.SaveChanges(); 

            return RedirectToAction("OrderSummary");
        }

        public IActionResult BackToMenu(int orderId, string orderNote)
        {
            var order = DineOutContext.Order.Find(orderId);
            order.Note = orderNote;
            DineOutContext.Update(order);
            DineOutContext.SaveChanges();

            orderData.Order
                = DineOutContext.Order.Find(order.OrderId);
            orderData.Customer
                = DineOutContext.Customer.Find(order.CustomerId);
            orderData.Menu = DineOutContext.Menu.Find(order.RestaurantId);
            orderData.Items = DineOutContext.Item
                .ToList().FindAll(x => x.MenuId == orderData.Menu.MenuId);
            orderData.Restaurant = DineOutContext.Restaurant
                .Find(order.RestaurantId);
            orderData.OrderItems = DineOutContext.Order_Item
                .ToList().FindAll(x => x.OrderId == order.OrderId);

            return View("OrderDetails", orderData);
        }

        public IActionResult Checkout(int customerId, double totalPrice)
        {
            var customerEmail = DineOutContext.Customer.Find(customerId).Email;
            
            //Set the payment function

            return RedirectToAction();
        }

        public IActionResult CancelOrder(int orderId)
        {
            var order = DineOutContext.Order.Find(orderId);
            //List<OrderItem> orderItems = new List<OrderItem>();
            var orderItems = DineOutContext.Order_Item
                .Where(x => x.OrderId == orderId).ToList();

            //DineOutContext.Remove(order);
            //foreach(var orderItem in orderItems)
            //{
            //    DineOutContext.Remove(orderItem);
            //}
            //DineOutContext.SaveChanges();

            return RedirectToAction("Index", "Customer");
        }
    }
}
