using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PizzaCommandProj_Management.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCommandProj_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly PizzaContext db;

        //Login: "admin" Password: "odmenotboga"
        //Login: "worker" Password: "bestworkerever"

        public HomeController(PizzaContext context)
        {
            db = context;
        }
        public IActionResult Index(ErrorViewModel errors)
        {
            if (CurUser != null)
            {
                return RedirectToAction("AllDishes");
            }
            ViewBag.ERRORR = errors.RequestId;
            return View();
        }
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("user");
            return RedirectToAction("Index");
        }
        private Dish GetDishById(int? id)
        {
            if (id == null)
                return null;
            return db.Find(typeof(Dish), id) as Dish;
        }
        private Order GetOrderById(int? id)
        {
            if (id == null)
                return null;
            return db.Find(typeof(Order), id) as Order;
        }
        private void DeleteDishById(int dishId)
        {
            db.Dishes.Remove(GetDishById(dishId));
            db.SaveChanges();
        }
        private void DeleteOrderById(int orderId)
        {
            db.Orders.Remove(GetOrderById(orderId));
            db.SaveChanges();
        }
        public IActionResult Logination(Admin @odmen)
        {
            if (odmen.Login == "admin" && odmen.Password == "odmenotboga")
            {
                CookieOptions userCookieOptions = new CookieOptions();
                userCookieOptions.Expires = new DateTimeOffset(DateTime.Now + TimeSpan.FromHours(8));
                Response.Cookies.Append("user", odmen.Login, userCookieOptions);
                return RedirectToAction("AllDishes");
            }
            else if (odmen.Login == "worker" && odmen.Password == "bestworkerever")
            {
                CookieOptions userCookieOptions = new CookieOptions();
                userCookieOptions.Expires = new DateTimeOffset(DateTime.Now + TimeSpan.FromHours(8));
                Response.Cookies.Append("user", odmen.Login, userCookieOptions);
                return RedirectToAction("AllDishes");
            }
            else
            {
                ErrorViewModel errors = new ErrorViewModel();
                errors.RequestId="Wrong login or password";
                return RedirectToAction("Index", errors);
            }
        }

        private string CurUser => Request.Cookies["user"];

        [HttpGet]
        public IActionResult AllDishes()
        {
            if (CurUser==null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.User = CurUser;
            return View("AllDishes", db.Dishes);
        }

        public IActionResult NewDish()
        {
            if (CurUser == null)
            {
                return RedirectToAction("Index");
            }
            if (CurUser != "admin")
            {
                return RedirectToAction("AllDishes");
            }
            return View("NewDish");
        }

        [HttpPost]
        public IActionResult NewDish(Dish @dish)
        {
            if (CurUser == null)
            {
                return RedirectToAction("Index");
            }
            if (CurUser != "admin")
            {
                return RedirectToAction("AllDishes");
            }
            db.Dishes.Add(@dish);
            db.SaveChanges();
            return RedirectToAction("AllDishes", db.Dishes);
        }

        public IActionResult EditDish(int dishId)
        {
            if (CurUser == null)
            {
                return RedirectToAction("Index");
            }
            if (CurUser != "admin")
            {
                return RedirectToAction("AllDishes");
            }
            Dish dish = GetDishById(dishId);
            DeleteDishById(dishId);
            return View(dish);
        }

        [HttpPost]
        public IActionResult EditDish(Dish dish)
        {
            if (CurUser == null)
            {
                return RedirectToAction("Index");
            }
            if (CurUser != "admin")
            {
                return RedirectToAction("AllDishes");
            }
            
            db.Dishes.Update(dish);
            db.SaveChanges();
            return RedirectToAction("AllDishes", db.Dishes);
        }

        public IActionResult DeleteDish(int dishId)
        {
            if (CurUser == null)
            {
                return RedirectToAction("Index");
            }
            if (CurUser != "admin")
            {
                return RedirectToAction("AllDishes");
            }
            DeleteDishById(dishId);
            return RedirectToAction("AllDishes", db.Dishes);
        }

        public IActionResult AllOrders()
        {
            if (CurUser == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.User = CurUser; 
            return View("AllOrders", db.Orders);
        }

        public IActionResult CanceledOrder(int orderId)
        {
            if (CurUser == null)
            {
                return RedirectToAction("Index");
            }
            Order order = GetOrderById(orderId);
            order.Status = "Canceled";
            db.Orders.Update(order);
            db.SaveChanges();
            return RedirectToAction("AllOrders", db.Orders);
        }
        public IActionResult InProcessOrder(int orderId)
        {
            if (CurUser == null)
            {
                return RedirectToAction("Index");
            }
            Order order = GetOrderById(orderId);
            order.Status = "In process";
            db.Orders.Update(order);
            db.SaveChanges();
            return RedirectToAction("AllOrders", db.Orders);
        }
        public IActionResult DeliveredOrder(int orderId)
        {
            if (CurUser == null)
            {
                return RedirectToAction("Index");
            }
            Order order = GetOrderById(orderId);
            order.Status = "Delivered";
            db.Orders.Update(order);
            db.SaveChanges();
            return RedirectToAction("AllOrders", db.Orders);
        }

        public IActionResult DeleteOrder(int orderId)
        {
            if (CurUser == null)
            {
                return RedirectToAction("Index");
            }
            if (CurUser != "admin")
            {
                return RedirectToAction("AllOrders");
            }
            DeleteOrderById(orderId);
            return RedirectToAction("AllOrders", db.Orders);
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
    }
}
