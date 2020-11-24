using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;
using DineOut.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace DineOut.Controllers
{
    public class CustomerController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();

        Menu_RestaurantDetails itemsOfRestaurant = new Menu_RestaurantDetails();

        public IActionResult Index()
        {
            ///an index page for customer to land on, redirects to "Home" View with a list of all restaurants on Dine out
            ///

            var Restaurants = DineOutContext.Restaurant.ToList();
            foreach (Restaurant restaurant in Restaurants)
            {
               
                    restaurant.Menu = DineOutContext.Menu.Find(restaurant.RestaurantId);
                
            }
            return View("Home", Restaurants);
        }

        public IActionResult SearchString(string searchedString)
        {
            /// The action responsible for producing a search query, takes a string parameter to be used in filtering results
            /// @param string searchedString
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
        public string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        [HttpPost]
        public IActionResult CustomerLogin(Customer customer)
        {
            // Check if customner exist
            var isCustomer = DineOutContext.Customer.Where(r => r.Email == customer.Email).FirstOrDefault();

            if (isCustomer != null)
            {
                // Check to see if password matches
                string[] salt = isCustomer.PasswordHash.Split(":");
                string newHashedPin = GenerateHash(customer.PasswordHash, salt[0]);
                bool isValid = newHashedPin.Equals(salt[1]);

                if (isValid == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Password does not match
                    return View();
                }
                
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public IActionResult Register(Customer customer, string firstPassword)
        {
            if (firstPassword != customer.PasswordHash)
            {
                // Passwords don't match
                return RedirectToAction("CustomerRegistration");
            }
            // Generate Salt
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[31];
            rng.GetBytes(buff);
            string salt = Convert.ToBase64String(buff);

            // Generate Hash
            string hashed = GenerateHash(customer.PasswordHash, salt);
            // Overwrite to delete the string passsword
            customer.PasswordHash = String.Format("{0}:{1}", salt, hashed);

            try
            {
                // Try to save new customer
                DineOutContext.Add(customer);
                DineOutContext.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                // Return to same view if cannot save to database
                return RedirectToAction("CustomerRegistration");
            }

        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(Customer customer, string oldPassword, string firstPassword)
        {
            var isCustomer = DineOutContext.Customer.Where(r => r.Email == customer.Email).FirstOrDefault();


            if (firstPassword != customer.PasswordHash)
            {
                // New Password does not match
                return View();
            }
            if (isCustomer !=  null)
            {
                // Customer exist
                string[] salt = isCustomer.PasswordHash.Split(":");
                string newHashedPin = GenerateHash(oldPassword, salt[0]);
                bool isValid = newHashedPin.Equals(salt[1]);
                if (isValid == true)
                {
                    // Password match and will be updated
                    string newSashed = GenerateHash(customer.PasswordHash, salt[0]);
                    // Overwrite to delete the string passsword
                    isCustomer.PasswordHash = String.Format("{0}:{1}", salt[0], newSashed);
                    DineOutContext.Update(isCustomer);
                    DineOutContext.SaveChanges();
                    return RedirectToAction("CustomerLogin");
                }
                else
                {
                    // Old password does not match
                    return View();
                    
                }
            }
            else
            {
                // Customer does not exist
                return View();
            }
        }
        


        //Menu action created by ederson

        public IActionResult Menu(int menuId = 1, int restaurantId = 1)//default parameter just for testing porpuse
        {
            ///Action to generate a restaurant's list for the customer to browse. needs a menu Id and Restaurant Id
            ///@param int menuId
            ///@param int restaurantId
            itemsOfRestaurant.Items = DineOutContext.Item.ToList().FindAll(x => x.MenuId == menuId && x.Availability == true);// displaying only available items
            itemsOfRestaurant.Restaurant = DineOutContext.Restaurant.Find(restaurantId);
            return View(itemsOfRestaurant);
        }


    }
}