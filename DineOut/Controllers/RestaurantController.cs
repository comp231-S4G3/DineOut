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
        private IRestaurantRepository repository;

        public RestaurantController (IRestaurantRepository repo)
        {
            repository = repo;
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

        public IActionResult List(string menuID, string created)
        {
            return View(repository.Item
                .Where(m => m.MenuId == menuID)
                .OrderBy(i => i.CreatedOn == created));
        }

        //Create Profile Action bellow
    }
}
