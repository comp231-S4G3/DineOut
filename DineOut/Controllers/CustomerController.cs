using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;
using DineOut.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace DineOut.Controllers
{
    public class CustomerController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();

        Menu_RestaurantDetails itemsOfRestaurant = new Menu_RestaurantDetails();
     

        public IActionResult Index()
        {
            var Restaurants = DineOutContext.Restaurant.ToList();
            foreach (Restaurant restaurant in Restaurants)
            {
               
                    restaurant.Menu = DineOutContext.Menu.ToList().Find( menu => menu.RestaurantId == restaurant.RestaurantId);
                
            }
            return View("Home", Restaurants);
        }

        public IActionResult SearchString(string searchedString)
        {
            var Restaurants = DineOutContext.Restaurant
                .Where(r => r.RestaurantName.ToLower().Contains(searchedString.ToLower())).ToList();

            foreach ( Restaurant restaurant in Restaurants)
            {
                restaurant.Menu = DineOutContext.Menu.Find(restaurant.RestaurantId);
            }

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
            var loggedInCustomer = DineOutContext.Customer.ToList().Find(c => c.Email == customer.Email);
            if(loggedInCustomer != null)
            {
                if (loggedInCustomer.PasswordHash.Equals(customer.PasswordHash))
                {

                    HttpContext.Session.SetString("customer_id", loggedInCustomer.ToString());
                    TempData["message"] = "Successfully Logged In!";
                    return RedirectToAction("Index");
                }
            }
            TempData["message"] = "Invalid User Login!";
            return View();
        }

        public IActionResult CustomerLogout()
        {
            HttpContext.Session.Remove("customer_id");
            TempData["message"] = "Successfully Logged Out!";
            return RedirectToAction("CustomerLogin");
        }
        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                DineOutContext.Customer.Add(customer);
                DineOutContext.SaveChanges();
                TempData["message"] = "Successfully Registed!";
                return RedirectToAction("CustomerLogin");
            }
                return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(Customer customer)
        {
            Customer user = DineOutContext.Customer.ToList().Find(x => x.Email == customer.Email);
            
            if(user != null)
            {
                //reset password
                user.PasswordHash = customer.PasswordHash;
                DineOutContext.Customer.Update(user);
                DineOutContext.SaveChanges();
                return RedirectToAction("CustomerLogin");
                
            }
            return View();
        }

        //Menu action created by ederson

        public IActionResult Menu(int menuId = 1, int restaurantId = 1)//default parameter just for testing porpuse
        {
            itemsOfRestaurant.Items = DineOutContext.Item.ToList().FindAll(x => x.MenuId == menuId && x.Availability == true);// displaying only available items
            itemsOfRestaurant.Restaurant = DineOutContext.Restaurant.Find(restaurantId);
            return View(itemsOfRestaurant);
        }


    }
}