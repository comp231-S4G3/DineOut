using DineOut.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

namespace DineOut.Controllers
{
    public class RestaurantController : Controller
    {
        DineOutContext DineOutContext = new DineOutContext();

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







        // Test View

        public IActionResult Index()
        {
            var menus = DineOutContext.Menu.ToList();
            var items = DineOutContext.Item.ToList();
            return View("Menu", DineOutContext.Item);
        }

        // MENU CRUD
        public IActionResult Add_Menu(int restaurant_id, string menu_title)
        {
            var menu_row = DineOutContext.Menu.Where(r => r.RestaurantId == restaurant_id).FirstOrDefault();

            if (menu_row != null)
            {
                TempData["message"] = $"You already have a menu created!";
                return RedirectToAction("Index");
            }
            else
            {
                Menu menu_object = new Menu();
                menu_object.RestaurantId = restaurant_id;
                var datetime = DateTime.Now.ToString("yyyy-MM-dd");
                menu_object.CreatedOn = DateTime.Today;
                menu_object.Title = menu_title;
                Console.WriteLine(menu_object);

                DineOutContext.Add(menu_object);
                DineOutContext.SaveChanges();

                TempData["message"] = $"Welcome! Your menu has been created!";
                return RedirectToAction("Index");
            }
        }
        public IActionResult Update_Menu(int restaurant_id, int menu_id, string menu_title)
        {
            var menu_row = DineOutContext.Menu.Where(r => r.RestaurantId == restaurant_id)
                .Where(r => r.MenuId == menu_id)
                .FirstOrDefault();

            Menu menu_object = menu_row;

            menu_object.Title = menu_title;
            Console.WriteLine(menu_object);

            DineOutContext.Update(menu_object);
            DineOutContext.SaveChanges();

            TempData["message"] = $"Title updated!";
            return RedirectToAction("Index");
        }
        public IActionResult Add_Item()
        {
            return RedirectToAction("Index");
        }
        public IActionResult Delete_Item()
        {
            return RedirectToAction("Index");
        }
        public IActionResult Update_Item()
        {
            return RedirectToAction("Index");
        }

        //public IActionResult DeleteMenu(int menu_id, int restaurant_id)
        //{
        //    var menus = DineOutContext.Menu.Where(r => r.RestaurantId == menu_id).Where(r => r.RestaurantId == restaurant_id).FirstOrDefault();

        //    if (menus != null)
        //    {
        //        DineOutContext.Remove(menus);
        //        DineOutContext.SaveChanges();
        //    }
        //    TempData["message"] = $"Your menu has been delete!";
        //    return RedirectToAction("Index");
        //}
    }
}
