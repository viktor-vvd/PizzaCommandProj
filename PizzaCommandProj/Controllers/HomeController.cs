using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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

        private Order CurOrder => JsonSerializer.Deserialize<Order>(Request.Cookies["order"]);
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
        public IActionResult AddCart(int dishId)
        {
            Order o;
            if (!Request.Cookies.ContainsKey("order"))
            {
                o = new Order();
                o.Amount = 0;
                o.DishesId = "";
            }
            else
                o = CurOrder;
            if(o.DishesId=="")
                o.DishesId+=dishId.ToString();
            else
                o.DishesId += ("~"+dishId.ToString());
            CookieOptions userCookieOptions = new CookieOptions();
            userCookieOptions.Expires = new DateTimeOffset(DateTime.Now + TimeSpan.FromHours(8));
            //var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(o);
            Response.Cookies.Append("order", jsonString, userCookieOptions);

            return View("Menu", db.Dishes);
        }
        public IActionResult NewOrder(int dishId)
        {
            Order o;
            if (!Request.Cookies.ContainsKey("order"))
            {
                o = new Order();
                o.Amount = 0;
                o.DishesId = "";
            }
            else
                o = CurOrder;
            if (o.DishesId.Length==0)
                o.DishesId += dishId.ToString();
            else
                o.DishesId += ("~" + dishId.ToString());
            List<string> result = o.DishesId.Split('~').ToList();
            foreach (var dishid in result)
            {
                if(!dishid.Contains("~")&& dishid.Length!=0)
                {
                    Dish dish = GetDishById(Convert.ToInt32(dishid));
                    o.Amount += dish.Price;
                    ViewBag.DishName = dish.Name;
                }
            }
                ViewBag.DishAmount = o.Amount;////////////////////////////////////////////////////////////
            CookieOptions userCookieOptions = new CookieOptions();
            userCookieOptions.Expires = new DateTimeOffset(DateTime.Now + TimeSpan.FromHours(8));
            //var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(o);
            Response.Cookies.Append("order", jsonString, userCookieOptions);
            return View("NewOrder");
        }

        [HttpPost]
        public IActionResult NewOrder(Order @order)
        {
            order.Amount=CurOrder.Amount;
            //order.DishesId=CurOrder.DishesId;
            order.DishesId=CurOrder.DishesId;
            order.Status = "Confirmed";
            order.Amount = CurOrder.Amount;
            db.Orders.Add(@order);
            db.SaveChanges();
            Response.Cookies.Delete("order");
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
        //public IActionResult AllCart()
        //{
        //    Order o;
        //    if (!Request.Cookies.ContainsKey("order"))
        //    {
        //        o = new Order();
        //        o.Amount = 0;
        //        o.DishesId = "";
        //    }
        //    else
        //        o = CurOrder;
        //    if (o.DishesId == "")
        //        o.DishesId += dishId.ToString();
        //    else
        //        o.DishesId += ("~" + dishId.ToString());
        //    CookieOptions userCookieOptions = new CookieOptions();
        //    userCookieOptions.Expires = new DateTimeOffset(DateTime.Now + TimeSpan.FromHours(8));
        //    //var options = new JsonSerializerOptions { WriteIndented = true };
        //    string jsonString = JsonSerializer.Serialize(o);
        //    Response.Cookies.Append("order", jsonString, userCookieOptions);

        //    return View("Menu", db.Dishes);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
