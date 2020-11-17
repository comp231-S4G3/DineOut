﻿using System;
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

        public IActionResult OrderDetails(int menuId, int customerId)
        {
            //menu_id and customer_id is hard coded for testing purpose
            menuId = 1;
            customerId = 1;

            //CustomerOrderViewModel customerOrderView = new CustomerOrderViewModel();
            Order nOrder = new Order();
            nOrder.CustomerId = customerId; //newOrder.Customer.CustomerId

            DineOutContext.Order.Add(nOrder);
            CustomerOrderViewModel.Order = DineOutContext.Order.Find(customerId);
            CustomerOrderViewModel.Menu = DineOutContext.Menu.Find(menuId);
            CustomerOrderViewModel.Items = DineOutContext.Item
                .ToList().FindAll(x => x.MenuId == menuId);
            CustomerOrderViewModel.Restaurant = DineOutContext.Restaurant
                .Find(CustomerOrderViewModel.Menu.RestaurantId);
            foreach (Item item in CustomerOrderViewModel.Items)
            {
                CustomerOrderViewModel.Item = DineOutContext.Item.Find(item.ItemId);
            }
            return View(CustomerOrderViewModel);
        }

        //Create a new order
        [HttpPost]
        public IActionResult AddItem(CustomerOrderViewModel order)
        {
            //Demo customerId (Should be relpased with newOrder.Customer.CustomerId later)
            int customerId = 1;
            CustomerOrderViewModel orderData = new CustomerOrderViewModel();
            OrderItem orderItem = new OrderItem();
            //CustomerOrderViewModel customerOrderView= new CustomerOrderViewModel();

            orderItem.ItemId = order.Item.ItemId;//set ItemId
            if (order.OrderItem.OrderItemId != 0)
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

                orderData.Order.OrderId = orderItem.OrderId;
                orderData.Menu = DineOutContext.Menu
                    .Find(order.Item.MenuId);
                orderData.Items = DineOutContext.Item
                    .ToList().FindAll(x => x.MenuId == order.Item.MenuId);
                orderData.Restaurant = DineOutContext.Restaurant
                    .Find(orderData.Menu.RestaurantId);
                foreach (Item item in orderData.Items)
                {
                    orderData.Item = DineOutContext.Item
                        .Find(item.ItemId);
                }

                TempData["message"] = "Item is added";
                return View(orderData);
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
        public IActionResult OrderSummary(int orderId)
        {
            //For testing purpose
            orderId = 2;
            List<Item> items = new List<Item>();
            CustomerOrderViewModel orderData = new CustomerOrderViewModel();

            orderData.Order = DineOutContext.Order.Find(orderId);
            orderData.OrderItems = DineOutContext.Order_Item
                .ToList().FindAll(x => x.OrderId == orderId);
            orderData.Restaurant = DineOutContext.Restaurant
                    .Find(orderData.Order.RestaurantId);
            foreach (OrderItem orderItem in orderData.OrderItems)
            {
                orderItem.Item = DineOutContext.Item
                    .Find(orderItem.ItemId);
            }

            return View(orderData);
            //return View(DineOutContext.Order.Where(p => p.OrderId == orderId));
            /*return View(new CustomerOrderViewModel
            {
                CustomerOrder = GetCustomerOrder()
            });*/
        }



    }
}
