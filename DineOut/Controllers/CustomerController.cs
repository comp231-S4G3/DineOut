using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;

namespace DineOut.Controllers
{
    public class CustomerController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();

        public IActionResult Index()
        {
            var restaurants = DineOutContext.Restaurant.ToList();
            return View("Home", restaurants);
        }

        public IActionResult SearchString(string searchedString)
        {
            var Restaurants = DineOutContext.Restaurant
                .Where(r => r.RestaurantName.ToLower() == searchedString.ToLower()).ToList();

            if (Restaurants.Count() == 0)
            {
                TempData["message"] = $"Sorry, \"{searchedString}\" was not found.";
                return View("Home", Restaurants);
            }
            else
                return View("Home", Restaurants);

        }

        public IActionResult CustomerLogin() => View();
        public IActionResult CustomerRegistration() => View();

        [HttpPost]
        public IActionResult CustomerLogin(Customer customer)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            return View();
        }

    }
}