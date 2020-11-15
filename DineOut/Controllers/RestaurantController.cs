﻿using DineOut.Models;
using DineOut.ViewModels;
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
        OrderDetailsInfo orderDetailsInfo = new OrderDetailsInfo();  // this a new view model
        List<Order> orders = new List<Order>();

        public RestaurantController ()
        {
            
        }

        public IActionResult Orders()
        {
            return View(DineOutContext.Order.OrderBy(o => o.OrderId));
        }
        
        public IActionResult CompletedOrders(int statusOrder) 
        {
            statusOrder = 5; //A status Order of 5 is considered completed
            orders = DineOutContext.Order
                .OrderBy(o => o.OrderId)
                .ToList()
                .FindAll(o => o.StatusId == statusOrder);
            return View(orders);
        }

        public IActionResult CurrentOrders()
        {
            //This will populate Orders that are of any statuses but 5
            //Which means Orders that are still open and/or current
            orders = DineOutContext.Order
                .OrderBy(o => o.OrderId)
                .ToList()
                .FindAll(o => o.StatusId != 5); 
            return View(orders);
        }

        // Test View
        public IActionResult Menu()
        {
            int restaurant_id = 3;
            var menud_id = DineOutContext.Menu.Where(r => r.RestaurantId == restaurant_id).FirstOrDefault().MenuId;
            var items = DineOutContext.Item.Where(r => r.MenuId == menud_id).ToList();
            return View("Menu", items);
            //return View("Menu", DineOutContext.Item);

        }
        public IActionResult Edit()
        {
            return View();
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
                return RedirectToAction("Menu");
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
            return RedirectToAction("Menu");
        }

        public IActionResult Add_Update_Item(Item item)
        {
            if (item.ItemId == 0)
            {
                Console.WriteLine(item);
                DineOutContext.Add(item);
                DineOutContext.SaveChanges();
            }
            else
            {
                Console.WriteLine(item);
                DineOutContext.Update(item);
                DineOutContext.SaveChanges();
                
            }
            return RedirectToAction("Menu");
        }
        public IActionResult Delete_Item(int item_id, int menu_id)
        {
            var item_delete = DineOutContext.Item.Where(r => r.MenuId == menu_id).Where(r => r.ItemId == item_id).FirstOrDefault();
            DineOutContext.Remove(item_delete);
            DineOutContext.SaveChanges();
            return RedirectToAction("Menu");
        }
        
        public IActionResult Update_Item(int itemId, int menuId)
        {
            var item = DineOutContext.Item.Where(r => r.MenuId == menuId).Where(r => r.ItemId == itemId).FirstOrDefault();
            Console.WriteLine(item);
            return View("Edit", item);
        }
        //Action Created by Ederson for OrderDetails OwnerSide
        public ViewResult OrderDetails(int orderId)
        {
            orderId = 2; // orderId hard coded fot testing proposes
            
            List<Item> items = new List<Item>();
            orderDetailsInfo.order = DineOutContext.Order.Find(orderId);
            orderDetailsInfo.OrderItems = DineOutContext.Order_Item.ToList().FindAll(x => x.OrderId == orderId);
            orderDetailsInfo.orderStatus = DineOutContext.OrderStatus.Find(orderDetailsInfo.order.StatusId);

            foreach (OrderItem orderItem in orderDetailsInfo.OrderItems)
            {
                orderItem.Item = DineOutContext.Item.Find(orderItem.ItemId);
            }
            return View(orderDetailsInfo);


        }

        //Update the order status
        [HttpPost]
        public IActionResult ChangeStatus(Order order)
        {
            const int COMPLETED = 5;

            orderDetailsInfo.order = DineOutContext.Order.Find(order.OrderId);
            orderDetailsInfo.order.StatusId = order.StatusId;

            if (ModelState.IsValid)
            {
                //If the status moved to "Completed", invoke payment method
                if (orderDetailsInfo.order.StatusId == COMPLETED)
                {
                    //Insert Invoking payment method here
                }

                DineOutContext.Update(orderDetailsInfo.order);
                DineOutContext.SaveChanges();

                TempData["message"] = "Order Status is updated.";
                return RedirectToAction("OrderDetails");
            }
            else
            {
                return View(orderDetailsInfo);
            }
        }

        //Create Profile Action bellow
    }
}
