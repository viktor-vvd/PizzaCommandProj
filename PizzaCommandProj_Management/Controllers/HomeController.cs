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
        private bool logged;
        private string errors;

        //Login: "admin" Password: "odmenotboga"

        public HomeController(PizzaContext context)
        {
            logged = false;
            errors = "";
            db = context;
        }
        public IActionResult Index()
        {
            //if (logged)
            //{
                //return RedirectToAction("AllDishes");
            //}
                ViewBag.ERRORR = errors;
                return View();
        }
        public IActionResult LogOut()
        {
            logged = false;
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
                logged = true;
                return RedirectToAction("AllDishes");
            }
            else
            {
                errors = "Wrong login or password";
                return RedirectToAction("Index");
            }
        }
        public IActionResult AllDishes()
        {
            //if (logged==false)
            //{
            //    return RedirectToAction("Index");
            //}
            errors = "";
            return View("AllDishes", db.Dishes);
        }

        public IActionResult NewDish()
        {
            //if (!logged)
            //{
            //    return RedirectToAction("Index");
            //}
            return View("NewDish");
        }

        [HttpPost]
        public IActionResult NewDish(Dish @dish)
        {
            //if (!logged)
            //{
            //    return RedirectToAction("Index");
            //}
            db.Dishes.Add(@dish);
            db.SaveChanges();
            return RedirectToAction("AllDishes", db.Dishes);
        }

        public IActionResult EditDish(int id)
        {
            //if (!logged)
            //{
            //    return RedirectToAction("Index");
            //}
            Dish dish = GetDishById(id);
            return View(dish);
        }

        [HttpPost]
        public IActionResult EditDish(Dish dish)
        {
            //if (!logged)
            //{
            //    return RedirectToAction("Index");
            //}
            db.Dishes.Update(dish);
            db.SaveChanges();
            return RedirectToAction("AllDishes", db.Dishes);
        }

        public IActionResult DeleteDish(int dishId)
        {
            //if (!logged)
            //{
            //    return RedirectToAction("Index");
            //}
            DeleteDishById(dishId);
            return RedirectToAction("AllDishes", db.Dishes);
        }

        public IActionResult AllOrders()
        {
            //if (!logged)
            //{
            //    return RedirectToAction("Index");
            //}
            return View("AllOrders", db.Orders);
        }

        public IActionResult CanceledOrder(int orderId)
        {
            //if (!logged)
            //{
            //    return RedirectToAction("Index");
            //}
            Order order = GetOrderById(orderId);
            order.Status = "Canceled";
            db.Orders.Update(order);
            db.SaveChanges();
            return RedirectToAction("AllOrders", db.Orders);
        }
        public IActionResult InProcessOrder(int orderId)
        {
            //if (!logged)
            //{
            //    return RedirectToAction("Index");
            //}
            Order order = GetOrderById(orderId);
            order.Status = "In process";
            db.Orders.Update(order);
            db.SaveChanges();
            return RedirectToAction("AllOrders", db.Orders);
        }
        public IActionResult DeliveredOrder(int orderId)
        {
            //if (!logged)
            //{
            //    return RedirectToAction("Index");
            //}
            Order order = GetOrderById(orderId);
            order.Status = "Delivered";
            db.Orders.Update(order);
            db.SaveChanges();
            return RedirectToAction("AllOrders", db.Orders);
        }

        public IActionResult DeleteOrder(int orderId)
        {
            //if (!logged)
            //{
            //    return RedirectToAction("Index");
            //}
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
