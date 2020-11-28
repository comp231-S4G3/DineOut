using DineOut.Controllers;
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

        
    }
}
