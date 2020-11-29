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
using Microsoft.EntityFrameworkCore;

namespace DineOut.Controllers
{
    public class HomeController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();

        public IActionResult Index()
        {
           
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


        public IActionResult QrGeneratorFromMenu()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QrGeneratorFromMenu(String menu)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                string url = "http://localhost:50682/Restaurant/Menu" + "/" + getMenuId(3);
                QRCodeGenerator oQRCodeGenerator = new QRCodeGenerator();
                QRCodeData oQRCodeData = oQRCodeGenerator.CreateQrCode(url ,QRCodeGenerator.ECCLevel.Q);
                QRCode oQRCode = new QRCode(oQRCodeData);

                using (Bitmap oBitmap = oQRCode.GetGraphic(20))
                {
                    oBitmap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());

                }

            }
            return View();
        }

        public string getMenuId(int resturant_id)
        {
     
            DbSet<Restaurant> Restaurant = DineOutContext.Restaurant;
            DbSet<Menu> Menu = DineOutContext.Menu;
            var menud_id = DineOutContext.Menu.Where(r => r.RestaurantId == resturant_id).FirstOrDefault().MenuId;
            return menud_id.ToString();
        }
    }
}
