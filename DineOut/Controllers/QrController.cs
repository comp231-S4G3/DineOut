using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DineOut.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace DineOut.Controllers
{
    public class QrController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();
        public IActionResult QrGenerator()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QrGenerator(String menu)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // after complete authentication, we will get resturant_id from database  and send it to getMenuId as paramater.
               // string resturanr_id 


                string url = "http://dineout.com/Restaurant/Menu" + "/" + getMenuId(3);
                QRCodeGenerator oQRCodeGenerator = new QRCodeGenerator();
                QRCodeData oQRCodeData = oQRCodeGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
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
