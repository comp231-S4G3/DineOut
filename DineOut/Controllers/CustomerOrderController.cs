using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;
using Microsoft.AspNetCore.Authentication;

namespace DineOut.Controllers
{
    public class CustomerOrderController : Controller
    {
        DineOutContext dineOutContext = new DineOutContext();

        //Display selected orderable menu 
        public ViewResult Order(int menu_id)
        {
            AllModels allModels = new AllModels();

            return View(dineOutContext.Menu
                .Where(p => p.MenuId == menu_id));
        }

        //Create a new order
        [HttpPost]
        public IActionResult CreateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                dineOutContext.Order.Add(order);

                Order newOrder = dineOutContext.Order.Find(order);
                //Is it the right way to set an opening page and the value at the same time?
                return View("OrderSummary", newOrder.OrderId);
            }
            else
            {
                TempData["message"] = "Sorry, there is an error. Plese try it again.";
                return View("OrderDetails");
            }
        }

        //Display order summary
        /*public IActionResult OrderSummary(int orderId)
        {
            return View(dineOutContext.Order
                .Where(p => p.OrderId == orderId));
        }*/



    }
}
