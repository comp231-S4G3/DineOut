using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;

namespace DineOut.Controllers
{
    public class OrderDetailsController:Controller
    {
        public ViewResult Order()
        {
            return View();
        }
        public ViewResult EditOrder()
        {
            return View();
        }
    }
}
