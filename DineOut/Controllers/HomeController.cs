using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;
using System.IO;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace DineOut.Controllers
{
    public class HomeController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();
        
        public IActionResult Index()
        {
            var restaurants = DineOutContext.Restaurant.ToList();
            var customers = DineOutContext.Customer.ToList();
            var orders = DineOutContext.Order.ToList();
            var orderStatus = DineOutContext.OrderStatus.ToList();
            var menus = DineOutContext.Menu.ToList();
            var items = DineOutContext.Item.ToList();
            var order_Items = DineOutContext.Order_Item.ToList();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            AllModels allModels = new AllModels();
            allModels.Restaurants = DineOutContext.Restaurant.ToList();
            allModels.Customers = DineOutContext.Customer.ToList();
            allModels.Orders = DineOutContext.Order.ToList();
            allModels.OrderStatuses = DineOutContext.OrderStatus.ToList();
            allModels.Menus = DineOutContext.Menu.ToList();
            allModels.Items = DineOutContext.Item.ToList();
            allModels.Order_Items = DineOutContext.Order_Item.ToList();
            return View(allModels);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            var something = DineOutContext.Customer.ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult QrGenerator()
        {
            return View();
        }


        [HttpPost]
        public ActionResult QrGenerator(String inputText)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator oQRCodeGenerator = new QRCodeGenerator();
                QRCodeData oQRCodeData = oQRCodeGenerator.CreateQrCode(inputText, QRCodeGenerator.ECCLevel.Q);
                QRCode oQRCode = new QRCode(oQRCodeData);

                using (Bitmap oBitmap = oQRCode.GetGraphic(20))
                {
                    oBitmap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

                }

            }
            return View();
        }
    }
}
