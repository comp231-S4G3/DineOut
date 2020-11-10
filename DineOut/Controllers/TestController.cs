using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DineOut.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DineOut.Controllers
{
    public class TestController : Controller
    {
        DineOutContext dineOutContext = new DineOutContext();
        // GET: /<controller>/
        public IActionResult Index()
        {
           var res =  dineOutContext.Restaurant.ToList();
            return View();
        }
    }
}
