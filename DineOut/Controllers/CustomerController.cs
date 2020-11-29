using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;
using DineOut.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Security.Cryptography;

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

        [HttpGet]
        public IActionResult CustomerLogin(int menuId, int restaurantId)
        {
            return View();
        }
        public IActionResult CustomerRegistration() => View();

        public string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        [HttpPost]
        public IActionResult CustomerLogin(CustomerLoginViewModel customerLoginViewModel)
        {
            var loggedInCustomer = DineOutContext.Customer.Where(r => r.Email == customerLoginViewModel.Customer.Email).FirstOrDefault();

            if (loggedInCustomer != null)
            {
                // Check to see if password matches
                string[] salt = loggedInCustomer.PasswordHash.Split(":");
                string newHashedPin = GenerateHash(customerLoginViewModel.Customer.PasswordHash, salt[0]);
                bool isValid = newHashedPin.Equals(salt[1]);

                if (isValid == true)
                {
                    HttpContext.Session.SetString("customer_id", loggedInCustomer.ToString());
                    TempData["message"] = "Successfully Logged In!";
                    if (customerLoginViewModel.RestaurantId != 0 && customerLoginViewModel.MenuId != 0)
                    {
                        return RedirectToAction("OrderDetails", "CustomerOrder",
                       new
                       {
                           customerId = loggedInCustomer.CustomerId,
                           menuId = customerLoginViewModel.MenuId,
                           restaurantId = customerLoginViewModel.RestaurantId
                       });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    // Password does not match
                    TempData["message"] = "Password does not match!";
                    return View();
                }
            }
            else
            {
                TempData["message"] = "User Does not Exist!";
                return View();
            }
        }

        public IActionResult CustomerLogout()
        {
            HttpContext.Session.Remove("customer_id");
            TempData["message"] = "Successfully Logged Out!";
            return RedirectToAction("CustomerLogin");
        }
        [HttpPost]
        public IActionResult Register(Customer customer, string firstPassword)
        {
            if (firstPassword != customer.PasswordHash)
            {
                // Passwords don't match
                TempData["message"] = "Passwords don't match!";
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
                TempData["message"] = "Successfully Registed!";
                return RedirectToAction("CustomerLogin");
            }
            catch
            {
                // Return to same view if cannot save to database
                TempData["message"] = "Couldn't create user!";
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
                TempData["message"] = "Password don't match!";
                return View();
            }
            if (isCustomer != null)
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
                    TempData["message"] = "Old Password is not correct!";
                    return View();

                }
            }
            else
            {
                // Customer does not exist
                TempData["message"] = "User does not exist";
                return View();
            }
        }

        //Menu action created by ederson

        public IActionResult Menu(int menuId, int restaurantId)//default parameter just for testing porpuse
        {
            var customer_id = HttpContext.Session.GetString("customer_id");
            if (customer_id != null)
            {
                return RedirectToAction("OrderDetails", "CustomerOrder",
                    new { customerId = customer_id, menuId = menuId,
                        restaurantId = restaurantId});
            }
            else
            {
                itemsOfRestaurant.Items = DineOutContext.Item.ToList().FindAll(x => x.MenuId == menuId && x.Availability == true);// displaying only available items
                itemsOfRestaurant.Restaurant = DineOutContext.Restaurant.Find(restaurantId);
                return View(itemsOfRestaurant);
            }
        }


    }
}