using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCommandProj.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCommandProj.Controllers
{
    public class HomeController : Controller
    {
        private readonly PizzaContext db;

        public HomeController(PizzaContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        private Dish GetDishById(int? id)
        {
            if (id == null)
                return null;
            return db.Find(typeof(Dish), id) as Dish;
        }
        int d_id=-333;
        int d_amount=-333;
        public IActionResult NewOrder(int dishId)
        {
            Dish dish = GetDishById(dishId);
            ViewBag.DishAmount = dish.Price;
            ViewBag.DishName = dish.Name;
            ViewBag.DishId = dish.Id;
            d_amount = dish.Price;
            d_id = dish.Id;
            return View("NewOrder");
        }

        [HttpPost]
        public IActionResult NewOrder(Order @order)
        {
            order.Status = "Confirmed";
            order.DishId = d_id;
            order.Amount = d_amount;
            db.Orders.Add(@order);
            db.SaveChanges();
            return RedirectToAction("OrderSuccess");
        }

        //[HttpGet]
        //public IActionResult DarocoBourse()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult Menu()
        {
            return View("Menu", db.Dishes);
        }
        [HttpGet]
        //public IActionResult Orders()
        //{
        //    return View("Orders", db.Orders);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
