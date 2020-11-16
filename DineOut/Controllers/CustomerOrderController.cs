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
        CustomerOrderViewModel CustomerOrderViewModel = new CustomerOrderViewModel();

        public IActionResult OrderDetails(/*int menuId, int customerId*/)
        {
            //menu_id and customer_id is hard coded for testing purpose
            int menuId = 1;
            int customerId = 1;

            CustomerOrderViewModel customerOrderView = new CustomerOrderViewModel();
            Order nOrder = new Order();
            nOrder.CustomerId = customerId; //newOrder.Customer.CustomerId

            DineOutContext.Order.Add(nOrder);
            customerOrderView.Order = DineOutContext.Order.Find(customerId);
            customerOrderView.Menu = DineOutContext.Menu.Find(menuId);
            customerOrderView.Items = DineOutContext.Item
                .ToList().FindAll(x => x.MenuId == menuId);
            customerOrderView.Restaurant = DineOutContext.Restaurant
                .Find(customerOrderView.Menu.RestaurantId);
            foreach (Item item in customerOrderView.Items)
            {
                customerOrderView.Item = DineOutContext.Item.Find(item.ItemId);
            }
            return View(customerOrderView);
        }

        //Create a new order
        [HttpPost]
        public IActionResult AddItem(CustomerOrderViewModel order)
        {
            //Demo customerId (Should be relpased with newOrder.Customer.CustomerId later)
            int customerId = 1;
            OrderItem orderItem = new OrderItem();
            CustomerOrderViewModel customerOrderView
                = new CustomerOrderViewModel();

            orderItem.ItemId = order.Item.ItemId;//set ItemId
            if(order.OrderItem.OrderItemId != 0)
                orderItem.OrderItemId = order.OrderItem.OrderItemId;//set OrderItemId

            if (ModelState.IsValid)
            {
                Order nOrder = new Order();
                nOrder.CustomerId = customerId; //newOrder.Customer.CustomerId

                if (order.Order.OrderId == 0)
                {
                    DineOutContext.Order.Add(nOrder);//pass customerId
                    orderItem.OrderId = nOrder.OrderId;
                    orderItem.Quantity = order.OrderItem.Quantity;
                    DineOutContext.Order_Item.Add(orderItem);
                }
                else
                {
                    orderItem.OrderId = nOrder.OrderId;
                    if (order.OrderItem.Quantity == 0)
                        orderItem.Quantity = order.OrderItem.Quantity;
                    orderItem.Quantity += order.OrderItem.Quantity;
                    DineOutContext.Order_Item.Update(orderItem);
                }

                customerOrderView.Order.OrderId = orderItem.OrderId;
                customerOrderView.Menu = DineOutContext.Menu
                    .Find(order.Item.MenuId);
                customerOrderView.Items = DineOutContext.Item
                    .ToList().FindAll(x => x.MenuId == order.Item.MenuId);
                customerOrderView.Restaurant = DineOutContext.Restaurant
                    .Find(customerOrderView.Menu.RestaurantId);
                foreach (Item item in customerOrderView.Items)
                {
                    customerOrderView.Item = DineOutContext.Item
                        .Find(item.ItemId);
                }

                TempData["message"] = "Item is added";
                return View(customerOrderView);
            }
            else
            {
                TempData["message"] = "Sorry, there is an error. Plese try it again.";
                return View(order.Item.MenuId);
            }
        }

        //Add to CustomerOrder Model
        public IActionResult AddToCustomerOrder(int itemId)
        {
            Item item = DineOutContext.Item
                .FirstOrDefault(p => p.ItemId == itemId);

            if (item != null)
            {
                CustomerOrder customerOrder = GetCustomerOrder();
                customerOrder.AddItem(item, 1);
                SaveCustomerOrder(customerOrder);
            }

            //How do I return a view with reflecting current order status (quentity)?
            return View("OrderSummary");

        }

        public IActionResult RemoveFromCustomerOrder(int itemId)
        {
            Item item = DineOutContext.Item
                .FirstOrDefault(p => p.ItemId == itemId);

            if (item != null)
            {
                CustomerOrder customerOrder = GetCustomerOrder();
                customerOrder.RemoveLine(item);
                SaveCustomerOrder(customerOrder);
            }

            //How do I return a view with reflecting current order status (quentity)?
            return View("OrderSummary");
        }

        private void SaveCustomerOrder(CustomerOrder customerOrder)
        {
            HttpContext.Session.SetJson("CustomerOrder", customerOrder);

        }

        private CustomerOrder GetCustomerOrder()
        {
            CustomerOrder customerOrder =
                HttpContext.Session.GetJson<CustomerOrder>("CustomerOrder")
                ?? new CustomerOrder();

            return customerOrder;
        }

        //Display order summary
        public IActionResult OrderSummary()
        {
            //return View(DineOutContext.Order.Where(p => p.OrderId == orderId));
            return View(new CustomerOrderViewModel
            {
                CustomerOrder = GetCustomerOrder()
            });
        }



    }
}
