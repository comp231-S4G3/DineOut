using DineOut.Controllers;
using DineOut.Models;
using DineOut.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DineOutTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCompletedOrdersView()
        {
            int statusComplete = 5;
            //var controller = new RestaurantController();
            var context = new DineOutContext();
            var order = new Order();
            order.OrderId = 1;
            order.RestaurantId = 1;
            order.StatusId = statusComplete;

            //var orders = context.Restaurant.Find(statusComplete).RestaurantId;

            //var completeOrders = orders.Add(new Order())
            var result = order;
            Assert.AreEqual("CompletedOrders", result);
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

            var result = controller.SearchString("The Keg") as ViewResult;

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
    }
}
