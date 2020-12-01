using System;
using System.Collections.Generic;
using System.Linq;
using DineOut.Controllers;
using DineOut.Models;
using DineOut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DineOutTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetMenuIdMethod()
        {
            var controller = new QrController();
            DineOutContext context = new DineOutContext();
            var rest = context.Restaurant.Find(20);
            var result = controller.getMenuId(rest.RestaurantId);
            Assert.AreEqual("23", result.ToString());
        }

        [TestMethod]
        public void TestGenerateHash()
        {
            DineOutContext context = new DineOutContext();

            var loggedInOwner = context.RestaurantProfile.Find(41);
            var controller = new RestaurantController();
            String myPassword = "123";

            // Check to see if password matches
            string[] salt = loggedInOwner.PasswordHash.Split(":");
            string newHashedPin = controller.GenerateHash(myPassword, salt[0]);
            Assert.AreEqual(salt[1], newHashedPin);

        }

        [TestMethod]
        public void TestIndexView()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }


        //TempData should be null if the method can find a search result whose name is the same as a parameter(string) 
        [TestMethod]
        public void TestSearchStringTempDataResult()
        {
            var controller = new CustomerController();

            var result = controller.SearchString("Diallo") as ViewResult;

            Assert.AreEqual(null, result.TempData);
        }

        [TestMethod]
        public void TestGetEmail()
        {
            var context = new DineOutContext();
            var controller = new RestaurantController();
            string testEmail;

            foreach (var customer in context.Customer)
            {
                testEmail = customer.Email;
                Assert.AreEqual(testEmail, controller.getEmail(customer.CustomerId));
            }

            
        }

        [TestMethod]
        public void TestRestaurantLoginAction()
        {
            var controller = new RestaurantController();
            DineOutContext context = new DineOutContext();
            var id = context.RestaurantProfile.Find(41).RestaurantProfileId;
            var email = context.RestaurantProfile.Find(41).Email;
            var pass = context.RestaurantProfile .Find(41).PasswordHash;
            RestaurantProfile testProfile = new RestaurantProfile();
            testProfile.RestaurantProfileId = id;
            testProfile.Email = email;
            testProfile.PasswordHash = pass;
            var result = controller.RestaurantLogin(testProfile);
            Assert.AreEqual("RestaurantLogin", result);
        }

        public void


    }
}
