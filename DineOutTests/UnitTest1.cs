using System;
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
        public void TestCompletedOrdersView()
        {
            var controller = new RestaurantController();

            var result = controller.CompletedOrders() as ViewResult;



            Assert.AreEqual("CompletedOrders", result.ViewName);
        }

        //TempData should be null if the method can find a search result whose name is the same as a parameter(string) 
        [TestMethod]
        public void TestSearchStringTempDataResult()
        {
            var controller = new CustomerController();

            var result = controller.SearchString("The Keg") as ViewResult;

            Assert.AreEqual(null, result.TempData);
        }

        [TestMethod]
        public void TestGetEmail()
        {
            DineOutContext context = new DineOutContext();
            var cus = context.Customer.Find(1);
            var controller = new RestaurantController();
            string testEmail;

            foreach (var customer in context.Customer)
            {
                testEmail = customer.Email;
                Assert.AreEqual(testEmail, controller.getEmail(customer.CustomerId));
            }

            
        }

        
    }
}
