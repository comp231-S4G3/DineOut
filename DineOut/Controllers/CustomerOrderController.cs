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

        //Display selected orderable menu 
        /*public ViewResult OrderDetails(int menu_id)
        {
            return View(DineOutContext.Menu
                .Where(p => p.MenuId == menu_id));
        }*/

        public IActionResult OrderDetails(/*int menu_id, int customer_id*/)
        {
            //menu_id and customer_id is hard coded for testing purpose
            int menu_id = 1;
            //customer_id = 1;

            CustomerOrderViewModel customerOrderView = new CustomerOrderViewModel();
            
            customerOrderView.Menu = DineOutContext.Menu.Find(menu_id);
            customerOrderView.Items = DineOutContext.Item
                .ToList().FindAll(x => x.MenuId == menu_id);
            customerOrderView.Restaurant = DineOutContext.Restaurant
                .Find(customerOrderView.Menu.RestaurantId);
            foreach (Item item in customerOrderView.Items)
            {
                customerOrderView.Item = DineOutContext.Item.Find(item.ItemId);
            }
            return View(customerOrderView);
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
            CustomerOrder order =
                HttpContext.Session.GetJson<CustomerOrder>("CustomerOrder")
                ?? new CustomerOrder();

            return order;
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
