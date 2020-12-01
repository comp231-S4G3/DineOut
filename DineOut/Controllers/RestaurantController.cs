using DineOut.Models;
using DineOut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Claims;
using DineOut.Common;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Security.Cryptography;

namespace DineOut.Controllers
{
    public class RestaurantController : Controller
    {
        AzureConnection AzureConnection = new AzureConnection();
        DineOutContext DineOutContext = new DineOutContext();
        OrderDetailsInfo orderDetailsInfo = new OrderDetailsInfo();  // this a new view model
        List<Order> orders = new List<Order>();

        public RestaurantController()
        {
            
        }

        public IActionResult OwnerLogin() => View();

        public IActionResult OwnerRegistration() => View();


        public IActionResult Orders()
        {
            var profile_id = HttpContext.Session.GetString("restaurant_owner_Id");
            int restautantId = DineOutContext.Restaurant.ToList()
                    .Find(o => o.RestaurantProfileId == Int32.Parse(profile_id))
                    .RestaurantId;
            return View(DineOutContext.Order
                .Where(o => o.RestaurantId == restautantId)
                .OrderBy(o => o.OrderId)
                .ToList());
        }
        public IActionResult CompletedOrders()
        {
            int statusOrder = 5; //A status Order of 5 is considered completed
            var profile_id = HttpContext.Session.GetString("restaurant_owner_Id");
            if (profile_id != null)
            {
                int restautantId = DineOutContext.Restaurant.ToList()
                    .Find(o => o.RestaurantProfileId == Int32.Parse(profile_id))
                    .RestaurantId;
                orders = DineOutContext.Order
                            .Where(o => o.RestaurantId == restautantId)
                            .OrderBy(o => o.OrderId)
                            .ToList()
                            .FindAll(o => o.StatusId == statusOrder);
            }

            return View(orders);
        }
        public IActionResult CurrentOrders()
        {
            var profile_id = HttpContext.Session.GetString("restaurant_owner_Id");
            if (profile_id != null)
            {
                int restautantId = DineOutContext.Restaurant.ToList()
                    .Find(o => o.RestaurantProfileId == Int32.Parse(profile_id))
                    .RestaurantId;
                orders = DineOutContext.Order
                            .Where(o => o.RestaurantId == restautantId)
                            .OrderBy(o => o.OrderId)
                            .ToList()
                            .FindAll(o => o.StatusId != 5);
            }

            //This will populate Orders that are of any statuses but 5
            //Which means Orders that are still open and/or current
            
            return View(orders);
        }

        public IActionResult Profile()
        {
            ProfileViewModel profileInfo = new ProfileViewModel();
            int profile_id = Int32.Parse(HttpContext.Session.GetString("restaurant_owner_Id"));
            int restaurant_id = DineOutContext.Restaurant.ToList().Find(r => r.RestaurantProfileId == profile_id).RestaurantId;
            var menud_id = DineOutContext.Menu.Where(r => r.RestaurantId == restaurant_id).FirstOrDefault().MenuId;


            int restaurantId = restaurant_id;
            int restaurantProfileId = profile_id;
            profileInfo.restaurant = DineOutContext.Restaurant.Find(restaurantId);
            profileInfo.restaurantProfile = DineOutContext.RestaurantProfile.Find(restaurantProfileId);

           
            return View(profileInfo);
        }

        [HttpPost]
        public IActionResult SaveChanges(RestaurantProfile restaurantProfile)
        {
            if (ModelState.IsValid)
            {
                RestaurantProfile profileEntry = DineOutContext.RestaurantProfile
                    .FirstOrDefault(r => r.RestaurantProfileId == restaurantProfile.RestaurantProfileId);
                if (profileEntry != null)
                {
                    profileEntry.Email = restaurantProfile.Email;
                }
                DineOutContext.SaveChanges();
                TempData["message"] = $"Your profile has been updated!";

            }
            return RedirectToAction("Menu");
        }


        public IActionResult SearchString(string searchedString)
        {
            Console.WriteLine(searchedString);
            try
            {
                var orderByID = DineOutContext.Order.Where(r => r.OrderId == Convert.ToInt32(searchedString)).ToList();
                return View("Orders", orderByID);
            }
            catch
            {
                return View("Menu");
            }
        }

        // MENU View
        public IActionResult Menu()
        {
            var profile_id =  HttpContext.Session.GetString("restaurant_owner_Id");
            if (profile_id != null)
            {
                int restaurant_id = DineOutContext.Restaurant.ToList().Find(r => r.RestaurantProfileId == Int32.Parse(profile_id)).RestaurantId;
                var menud_id = DineOutContext.Menu.Where(r => r.RestaurantId == restaurant_id).FirstOrDefault().MenuId;
                var items = DineOutContext.Item.Where(r => r.MenuId == menud_id).ToList();
                return View("Menu", items);
            }
            return RedirectToAction("OwnerLogin");
            

        }

        // MENU CRUD
        public IActionResult Add_Menu(int restaurant_id, string menu_title)
        {
            var profile_id = HttpContext.Session.GetString("restaurant_owner_Id");
            if (profile_id != null)
            {
                var menu_row = DineOutContext.Menu.Where(r => r.RestaurantId == restaurant_id).FirstOrDefault();

                if (menu_row != null)
                {
                    TempData["message"] = $"You already have a menu created!";
                    return RedirectToAction("Index");
                }
                else
                {
                    Menu menu_object = new Menu();
                    menu_object.RestaurantId = restaurant_id;
                    var datetime = DateTime.Now.ToString("yyyy-MM-dd");
                    menu_object.CreatedOn = DateTime.Today;
                    menu_object.Title = menu_title;
                    Console.WriteLine(menu_object);

                    DineOutContext.Add(menu_object);
                    DineOutContext.SaveChanges();

                    TempData["message"] = $"Welcome! Your menu has been created!";
                    return RedirectToAction("Menu");
                }
            }
            return RedirectToAction("OwnerLogin");
        }

        public IActionResult Update_Menu(int restaurant_id, int menu_id, string menu_title)
        {
            var profile_id = HttpContext.Session.GetString("restaurant_owner_Id");
            if (profile_id != null)
            {
                var menu_row = DineOutContext.Menu.Where(r => r.RestaurantId == restaurant_id)
                .Where(r => r.MenuId == menu_id)
                .FirstOrDefault();

                Menu menu_object = menu_row;

                menu_object.Title = menu_title;
                Console.WriteLine(menu_object);

                DineOutContext.Update(menu_object);
                DineOutContext.SaveChanges();

                TempData["message"] = $"Title updated!";
                return RedirectToAction("Menu");
            }
            return RedirectToAction("OwnerLogin");
        }
        [ValidateAntiForgeryToken]
        public IActionResult Add_Update_Item(ItemViewModel itemViewModel)
        {
            if (itemViewModel.ItemId == 0)
            {
                var profile_id = HttpContext.Session.GetString("restaurant_owner_Id");
                int restaurant_id = DineOutContext.Restaurant.ToList().Find(r => r.RestaurantProfileId == Int32.Parse(profile_id)).RestaurantId;
                var menud_id = DineOutContext.Menu.Where(r => r.RestaurantId == restaurant_id ).FirstOrDefault().MenuId;


                itemViewModel.ImagePath = uploadImage(itemViewModel.Image);
                Console.WriteLine(itemViewModel);
                Item item = new Item() { MenuId = menud_id, ItemName = itemViewModel.ItemName, Description = itemViewModel.Description, Ingredients = itemViewModel.Ingredients, Price = itemViewModel.Price, Image = itemViewModel.ImagePath, Availability = itemViewModel.Availability, CreatedOn = itemViewModel.CreatedOn };
                DineOutContext.Add(item);
                DineOutContext.SaveChanges();
            }
            else
            {
                Item item = new Item()
                {
                    ItemId = itemViewModel.ItemId,
                    MenuId = itemViewModel.MenuId,
                    ItemName = itemViewModel.ItemName,
                    Description = itemViewModel.Description,
                    Ingredients = itemViewModel.Ingredients,
                    Price = itemViewModel.Price,
                    Availability = itemViewModel.Availability,
                    CreatedOn = itemViewModel.CreatedOn
             
                };
                if (itemViewModel.Image != null)
                {
                    itemViewModel.ImagePath = uploadImage(itemViewModel.Image);
                    item.Image = itemViewModel.ImagePath;
                    DineOutContext.Update(item);
                }
                else
                {
                    var entry = DineOutContext.Item.First(e => e.ItemId == itemViewModel.ItemId);
                    item.Image = entry.Image;
                    DineOutContext.Entry(entry).CurrentValues.SetValues(item);

                }
            DineOutContext.SaveChanges();


        }
            return RedirectToAction("Menu");
        }


        private string uploadImage(IFormFile image)
        {


            string url = "https://dineout5.blob.core.windows.net/images";
            string extension = Path.GetExtension(image.FileName);
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string nowTime = DateTime.Now.ToString("yymmssffff");

            string path = $@"{year}\{month}\{day}\";
            string fileName = nowTime + image.FileName;
            string imagePathAndFileName = path + fileName;


            using (var memoryStream = new MemoryStream())
            {
                var task = image.CopyToAsync(memoryStream);
                task.Wait();
                memoryStream.Position = 0;
                AzureConnection.UploadImageMemoryStream(imagePathAndFileName, memoryStream);
            }

            var imageUrlPath = $@"{url}/{year}/{month}/{day}/{fileName}";
            return imageUrlPath;

        }


        public IActionResult Delete_Item(int item_id, int menu_id)
        {
            var item_delete = DineOutContext.Item
                .Where(r => r.ItemId == item_id)
                .Where(r => r.MenuId == menu_id)
                .FirstOrDefault();
            DineOutContext.Remove(item_delete);
            DineOutContext.SaveChanges();
            return RedirectToAction("Menu");
        }

        public IActionResult Update_Item(int itemId, int menuId)
        {
            
            var item = DineOutContext.Item.Where(r => r.MenuId == menuId).Where(r => r.ItemId == itemId).FirstOrDefault();
           
            Console.WriteLine(item);
            return View("Edit", item);
        }


        //Action Created by Ederson for OrderDetails OwnerSide
        public ViewResult OrderDetails(int orderId) // optional parameter just for testing purpose
        {
           

            orderDetailsInfo.order = DineOutContext.Order.Find(orderId);
            orderDetailsInfo.OrderItems = DineOutContext.Order_Item.ToList().FindAll(x => x.OrderId == orderId);
            orderDetailsInfo.orderStatus = DineOutContext.OrderStatus.Find(orderDetailsInfo.order.StatusId);

            foreach (OrderItem orderItem in orderDetailsInfo.OrderItems)
            {
                orderItem.Item = DineOutContext.Item.Find(orderItem.ItemId);
            }
            return View(orderDetailsInfo);


        }

        //Update the order status
        [HttpPost]
        public IActionResult ChangeStatus(Order order)
        {
            orderDetailsInfo.order = DineOutContext.Order.Find(order.OrderId);

            if (ModelState.IsValid)
            {
                orderDetailsInfo.order.StatusId = order.StatusId;
                DineOutContext.Update(orderDetailsInfo.order);
                DineOutContext.SaveChanges();
                SendMail(order, orderDetailsInfo.order.CustomerId);
            }

            UriBuilder uriBuilder = new UriBuilder(Request.GetTypedHeaders().Referer);
            NameValueCollection queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
            queryString["message"] = "Order Status is updated.";
            uriBuilder.Query = queryString.ToString();
            return Redirect(uriBuilder.ToString());
        }

        private void SendMail(Order order, int customerId)
        {
            string subject = "";
            string body = "";
            
            
            switch (order.StatusId)
            {
                case 1:
                    subject = "Your Order Is Accepted.";
                    body = "Please wait for your order be process.";
                    break;
                case 2:
                    subject = "Your Food Is Processing";
                    body = "Please wait for your food be ready.";
                    break;
                case 3:
                    subject = "Your Food Is Ready For Pick Up";
                    body = "Please pick it up at service table.";
                    break;
                case 4:
                    subject = "Your Food Is Received";
                    body = "Have a nice meal.";
                    break;
                case 5:
                    subject = "Your Order Is Completed";
                    body = "Welcome next time.";
                    break;
                default:
                    break;
            }
            string to = getEmail(customerId);
            SendMail(body, subject, to);
        }

        public string getEmail(int customerID)
        {
            string customerEmail;

            Customer customer = DineOutContext.Customer
                                .Where(c => c.CustomerId == customerID)
                                .FirstOrDefault();
            customerEmail = customer.Email;
            return customerEmail;
        }

        private void SendMail(string body, string subject, string to)
        {
            string from = "Dineout2021@gmail.com";
            MailMessage message = new MailMessage(from, to);
            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = body;
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("Dineout2021@gmail.com", "l23456789@");
            client.Port = 587;
            client.EnableSsl = true;
            client.Send(message);
        }


        public string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(RestaurantProfile restaurantProfile, string firstPassword)
        {
            if (firstPassword != restaurantProfile.PasswordHash)
            {
                // New Password does not match
                TempData["message"] = "Password does not match!";
                return View();
            }

            var isCustomer = DineOutContext.RestaurantProfile.Where(r => r.Email == restaurantProfile.Email).FirstOrDefault();
            
            if (isCustomer != null)
            {
                // Customer exist
                string[] salt = isCustomer.PasswordHash.Split(":");

                // Password match and will be updated
                string newSashed = GenerateHash(restaurantProfile.PasswordHash, salt[0]);
                // Overwrite to delete the string passsword
                isCustomer.PasswordHash = String.Format("{0}:{1}", salt[0], newSashed);
                DineOutContext.Update(isCustomer);
                DineOutContext.SaveChanges();
                return RedirectToAction("OwnerLogin");
 
            }
            else
            {
                // Customer does not exist
                TempData["message"] = "User does not exit!";
                return View();
            }
        }

        [HttpPost]
        public IActionResult RestaurantLogin(RestaurantProfile restaurantProfile)
        {
            // Check if customner exist
            var loggedInOwner = DineOutContext.RestaurantProfile.ToList().Find(c => c.Email == restaurantProfile.Email);
            if (loggedInOwner != null)
            {
                // Check to see if password matches
                string[] salt = loggedInOwner.PasswordHash.Split(":");
                string newHashedPin = GenerateHash(restaurantProfile.PasswordHash, salt[0]);
                bool isValid = newHashedPin.Equals(salt[1]);

                if (isValid == true)
                {
                    HttpContext.Session.SetString("restaurant_owner_Id", loggedInOwner.ToString());
                    TempData["message"] = "Successfully Logged In!";
                    return RedirectToAction("Menu");
                }
                else
                {
                    // Password does not match
                    TempData["message"] = "Invalid Login!";
                    return View("OwnerLogin");
                }
            }
            else
            {
                TempData["message"] = "User does not exist!";
                return View("OwnerLogin");
            }
        }

        public IActionResult RestaurantLogout()
        {
            HttpContext.Session.Remove("restaurant_owner_Id");
            TempData["message"] = "Successfully Logged Out!";
            return RedirectToAction("OwnerLogin");
        }


        [HttpPost]
        public IActionResult Register(ProfileViewModel profileViewModel, string firstPassword)
        {
            if (firstPassword != profileViewModel.restaurantProfile.PasswordHash)
            {
                // Passwords don't match
                TempData["message"] = "Passwords don't match!";
                return View("OwnerRegistration");
            }

            // Generate Salt
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[31];
            rng.GetBytes(buff);
            string salt = Convert.ToBase64String(buff);

            // Generate Hash
            string hashed = GenerateHash(profileViewModel.restaurantProfile.PasswordHash, salt);
            // Overwrite to delete the string passsword
            profileViewModel.restaurantProfile.PasswordHash = String.Format("{0}:{1}", salt, hashed);

            try
            {

                //Try to save new customer
                profileViewModel.restaurantProfile.CreatedOn = DateTime.Now;
                var profile = DineOutContext.RestaurantProfile.Add(profileViewModel.restaurantProfile);
                DineOutContext.SaveChanges();

                Restaurant restaurant = new Restaurant();
                restaurant.RestaurantName = profileViewModel.restaurant.RestaurantName;
                restaurant.RestaurantProfileId = profile.Entity.RestaurantProfileId;
                restaurant.CreatedOn = DateTime.Now;
                DineOutContext.Restaurant.Add(restaurant);
                DineOutContext.SaveChanges();
                Menu menu = new Menu();
                menu.Title = restaurant.RestaurantName + " Menu";
                menu.RestaurantId = restaurant.RestaurantId;
                menu.CreatedOn = DateTime.Now;
                DineOutContext.Menu.Add(menu);
                DineOutContext.SaveChanges();
                TempData["message"] = "Successfully Registered!";
                return RedirectToAction("OwnerLogin");
            }
            catch
            {
                // Return to same view if cannot save to database1
                TempData["message"] = "Couldn't create user!";
                return RedirectToAction("OwnerRegistration");
            }
        }

        [HttpGet]
        public IActionResult Notification()
        {
            ViewBag.RestaurantNames = DineOutContext.Restaurant.ToList().Select(r => r.RestaurantName);
            return View(DineOutContext.Notification.ToList());
        }

        [HttpPost]
        public IActionResult Notification(string content, string template)
        {
            var subject = "recommended dishes";
            if (template.Equals("covid"))
            {
                subject = "covid alert";
            }

            var failedInfo = "";
            var customerList = DineOutContext.Customer.Where(customer => customer.Email != null && customer.Email.Trim().Length > 0);
            foreach (var customer in customerList)
            {
                try
                {
                    SendMail(content, subject, customer.Email);
                }
                catch (Exception e)
                {
                    failedInfo += customer.Email + ": " + e.Message;
                }
            }

            var notification = new Notification
            {
                Content = content,
                Subject = subject,
                SendTime = DateTime.Now,
                FailedInfo = failedInfo
            };
            DineOutContext.Notification.Add(notification);
            DineOutContext.SaveChanges();
            ViewBag.RestaurantNames = DineOutContext.Restaurant.ToList().Select(r => r.RestaurantName);
            return View(DineOutContext.Notification.ToList());
        }

    }


}