using DineOut.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Controllers
{
    public class RestaurantController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();

        public RestaurantController ()
        {
            
        }


        //This belongs to Order Controller
        //Added here, so we do not forget
        /*public IActionResult CurrentOrders() 
        {
            return View();
        }*/

        //This belongs to Order Controller
        //Added here, so we do not forget
        /*public IActionResult CompletedOrders()
        {
            return View();
        }*/

        public IActionResult Menu(int menuID, DateTime created)
        {
            return View(DineOutContext.Item
                .Where(m => m.MenuId == menuID)
                .OrderBy(i => i.CreatedOn == created));
        }

        //Create Profile Action bellow
    }
}
