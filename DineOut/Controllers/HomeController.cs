using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;
namespace DineOut.Controllers
{
    public class HomeController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();
        
        public IActionResult Index()
        {
            var restaurants = DineOutContext.Restaurant.ToList();
            var customers = DineOutContext.Customer.ToList();
            var orders = DineOutContext.Order.ToList();
            var orderStatus = DineOutContext.OrderStatus.ToList();
            var menus = DineOutContext.Menu.ToList();
            var items = DineOutContext.Item.ToList();
            var order_Items = DineOutContext.Order_Item.ToList();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
