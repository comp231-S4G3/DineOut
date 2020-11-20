using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DineOut.Models;

namespace DineOut.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CustomerLogin() => View();
        public IActionResult CustomerRegistration() => View();

        [HttpPost]
        public IActionResult CustomerLogin(Customer customer)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            return View();
        }

    }
}