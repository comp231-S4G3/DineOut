using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;
using DineOut.ViewModels;
using Microsoft.AspNetCore.Http;

namespace DineOut.Controllers
{
    public class CustomerOrderController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();
        CustomerOrderViewModel orderData = new CustomerOrderViewModel();

        public IActionResult OrderDetails(
            int menuId, int customerId, int restaurantId)
        {
            ///The action that returns the orderable menu. this allows the customer to build their order
            ///@param int menuId
            ///@param int restaurantId
            ///@param int customerId
            ///
            

            var customer_id = HttpContext.Session.GetString("customer_id");
            if (customer_id != null)
            {
               
                Order nOrder = new Order
                {
                    CustomerId = Int32.Parse(customer_id),
                    RestaurantId = restaurantId,
                    StatusId = 1,
                    CreatedOn = DateTime.Now
                };
                DineOutContext.Order.Add(nOrder);
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
            return RedirectToAction("CustomerLogin", "Customer",
                   new
                   {
                       menuId = menuId,
                       restaurantId = restaurantId
                   });

        }

        //Create a new order
        [HttpPost]
        public IActionResult AddItem(CustomerOrderViewModel order,
            int quantity, int orderId, int customerId, string tableNum)
        {
            ///The action adds an item to the current order
            ///@param int quantity
            ///@param int orderId
            ///@param int customerId
            ///@param CustomerOrderViewModel order
            Order nOrder = new Order
            {
                OrderId = orderId,
                CustomerId = customerId,
                TableNum = tableNum
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
                    var checkItems = DineOutContext.Order_Item
                        .Where(x => x.OrderId == orderItem.OrderId)
                        .Where(x => x.ItemId == orderItem.ItemId);

                    OrderItem checkItem = new OrderItem();

                    foreach(var i in checkItems)
                    {
                        if (orderItem.ItemId == i.ItemId)
                        {
                            checkItem = i;
                        }
                    }

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

        public IActionResult OrderSummary(int orderId, string tableNum)
        {
            ///Action that directs customer to a summary of their order where thay can remove, update items on their order or submit their order
            ///@param int orderId

            var order = DineOutContext.Order.Find(orderId);
            order.TableNum = tableNum;
            DineOutContext.Order.Update(order);
            DineOutContext.SaveChanges();

            orderData.Order = DineOutContext.Order.Find(orderId);
            orderData.OrderItem = DineOutContext.Order_Item.Find(orderId);
            orderData.OrderItems = DineOutContext.Order_Item
                .ToList().FindAll(x => x.OrderId == orderId);
            orderData.Restaurant = DineOutContext.Restaurant
                .Find(orderData.Order.RestaurantId);
            orderData.Restaurant = DineOutContext.Restaurant
                .Find(orderData.Order.RestaurantId);
            orderData.Customer = DineOutContext.Customer
                .Find(orderData.Order.CustomerId);
            foreach (OrderItem orderItem in orderData.OrderItems)
            {
                orderItem.Item = DineOutContext.Item.Find(orderItem.ItemId);
            }

            return View("OrderSummary", orderData);
        }

        public IActionResult DeleteItem(int orderId, int itemId, int orderItemId)
        {
            /// action called when an item is to be removed from an order
            /// @param int orderId
            /// @param int itemId
            /// @param int orderItemIs
            var item_delete = DineOutContext.Order_Item
                .Where(r => r.OrderItemId == orderItemId)
                .Where(r => r.ItemId == itemId).FirstOrDefault();
            DineOutContext.Remove(item_delete);
            DineOutContext.SaveChanges();

            return RedirectToAction("OrderSummary",
                new { orderId = orderId });
        }

        public IActionResult ChangeQuantity(int orderId, int itemId, int orderItemId, int quantity)
        {

            /// action called when an item's quantity is being changed by the customer
            /// @param int orderId
            /// @param int itemId
            /// @param int orderItemId
            /// @param int quantity
            var itemUpdate = DineOutContext.Order_Item
                .Where(r => r.OrderItemId == orderItemId)
                .Where(r => r.ItemId == itemId).FirstOrDefault();
            itemUpdate.Quantity = quantity;
            DineOutContext.Update(itemUpdate);
            DineOutContext.SaveChanges();

            return RedirectToAction("OrderSummary",
                new { orderId = orderId });
        }

        public IActionResult BackToMenu(int orderId, string orderNote)
        {
            ///actiont that takes the customer back from the order summary back to the orderable menu
            ///@param int orderId
            ///@param string orderNote
            var order = DineOutContext.Order.Find(orderId);
            order.Note = orderNote;
            DineOutContext.Update(order);
            DineOutContext.SaveChanges();

            orderData.Order
                = DineOutContext.Order.Find(order.OrderId);
            orderData.Customer
                = DineOutContext.Customer.Find(order.CustomerId);
            orderData.Menu = DineOutContext.Menu.ToList().Find(x=> x.RestaurantId == order.RestaurantId);
            orderData.Items = DineOutContext.Item
                .ToList().FindAll(x => x.MenuId == orderData.Menu.MenuId);
            orderData.Restaurant = DineOutContext.Restaurant
                .Find(order.RestaurantId);
            orderData.OrderItems = DineOutContext.Order_Item
                .ToList().FindAll(x => x.OrderId == order.OrderId);

            return View("OrderDetails", orderData);
        }

        public IActionResult Checkout(int orderId, int customerId,
            double totalPrice, string orderNote)
        {
            /// the checkout action sends the customer to the payment gateway along with the order id, customer id, the total order price and custom notes
            /// @param int orderId
            /// @param int customerId
            /// @param double totalPrice
            /// @param string orderNote
            var order = DineOutContext.Order.Find(orderId);
            order.Note = orderNote;
            DineOutContext.Update(order);
            DineOutContext.SaveChanges();

            var customerEmail = DineOutContext.Customer.Find(customerId).Email;

            return RedirectToAction("index", "Payment",
                new { totalPrice = totalPrice });
        }

        public IActionResult CancelOrder(int orderId)
        {
            /// the action that cancels the current order and returns the customer to the customer index page
            /// @param int orderId
            var order = DineOutContext.Order.Find(orderId);
            
            var orderItems = DineOutContext.Order_Item
                .Where(x => x.OrderId == orderId).ToList();


            return RedirectToAction("Index", "Customer");
        }
    }
}
