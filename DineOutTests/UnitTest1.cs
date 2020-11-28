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

        [TestMethod]
        public void TestOrderDetails()
        {
            var controller = new CustomerOrderController();

            var result = controller.OrderDetails(1, 1, 1);

            Assert.AreEqual("IActionResult", result.GetType());
        }
    }
}
