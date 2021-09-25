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
        private Order AddToCart(int dishId)
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
            if (o.DishesId == "")
                o.DishesId += dishId.ToString();
            else
                o.DishesId += ("~" + dishId.ToString());
            o.Amount = 0;
            List<string> result = o.DishesId.Split('~').ToList();
            foreach (var dishid in result)
            {
                if (!dishid.Contains("~") && dishid.Length != 0)
                {
                    Dish dish = GetDishById(Convert.ToInt32(dishid));
                    o.Amount += dish.Price;
                    ViewBag.DishName = dish.Name;
                }
            }
            CookieOptions userCookieOptions = new CookieOptions();
            userCookieOptions.Expires = new DateTimeOffset(DateTime.Now + TimeSpan.FromHours(8));
            //var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(o);
            Response.Cookies.Append("order", jsonString, userCookieOptions);
            return o;
        }

        private void MinusOneFromCart(int dishId)
        {
            Order o;
            if (!Request.Cookies.ContainsKey("order"))
            {
                o = new Order();
                o.Amount = 0;
                o.DishesId = "";
                return;
            }
            else
                o = CurOrder;
            if (o.DishesId == "")
            {
                return;
            }
            else
            {
                o.Amount = 0;
                List<string> result = o.DishesId.Split('~').ToList();
                int index = result.IndexOf(dishId.ToString());
                if(index>=0)
                {
                    result.RemoveAt(index);
                    if (result[0] == "~")
                        result.RemoveAt(0);
                    else if (result[result.Count-1] == "~")
                        result.RemoveAt(result.Count - 1);
                    else
                    {
                        for(int i=1; i<result.Count-1;i++)
                        {
                            if(result[i]=="~"&&result[i-1]=="~")
                                result.RemoveAt(i);
                            else if(result[i]=="~"&&result[i+1]=="~")
                                result.RemoveAt(i);
                        }
                    }
                }
                o.DishesId = "";
                foreach (var dishid in result)
                {
                    if (!dishid.Contains("~") && dishid.Length != 0)
                    {
                        o.DishesId += dishid + "~";
                        Dish dish = GetDishById(Convert.ToInt32(dishid));
                        o.Amount += dish.Price;
                        ViewBag.DishName = dish.Name;
                    }
                }

                CookieOptions userCookieOptions = new CookieOptions();
                userCookieOptions.Expires = new DateTimeOffset(DateTime.Now + TimeSpan.FromHours(8));
                string jsonString = JsonSerializer.Serialize(o);
                Response.Cookies.Append("order", jsonString, userCookieOptions);
            }
        }
        private void DeleteFromCart(int dishId)
        {
            Order o;
            if (!Request.Cookies.ContainsKey("order"))
            {
                o = new Order();
                o.Amount = 0;
                o.DishesId = "";
                return;
            }
            else
                o = CurOrder;
            if (o.DishesId == "")
            {
                return;
            }
            else
            {
                o.Amount = 0;
                List<string> result = o.DishesId.Split('~').ToList();
                while (result.Contains(dishId.ToString()))
                {
                    int index = result.IndexOf(dishId.ToString());
                    if (index >= 0)
                    {
                        result.RemoveAt(index);
                        if (result[0] == "~")
                            result.RemoveAt(0);
                        else if (result[result.Count - 1] == "~")
                            result.RemoveAt(result.Count - 1);
                        else
                        {
                            for (int i = 1; i < result.Count - 1; i++)
                            {
                                if (result[i] == "~" && result[i - 1] == "~")
                                    result.RemoveAt(i);
                                else if (result[i] == "~" && result[i + 1] == "~")
                                    result.RemoveAt(i);
                            }
                        }
                    }
                }
                o.DishesId = "";
                foreach (var dishid in result)
                {
                    if (!dishid.Contains("~") && dishid.Length != 0)
                    {
                        o.DishesId += dishid + "~";
                        Dish dish = GetDishById(Convert.ToInt32(dishid));
                        o.Amount += dish.Price;
                        ViewBag.DishName = dish.Name;
                    }
                }

                CookieOptions userCookieOptions = new CookieOptions();
                userCookieOptions.Expires = new DateTimeOffset(DateTime.Now + TimeSpan.FromHours(8));
                string jsonString = JsonSerializer.Serialize(o);
                Response.Cookies.Append("order", jsonString, userCookieOptions);
            }
        }


        public IActionResult MinusOneCart(int dishId)
        {
            MinusOneFromCart(dishId);
            return AllCart();
        }

        public IActionResult AddOneCart(int dishId)
        {
            AddToCart(dishId);
            return AllCart();
        }

        public IActionResult DeleteOneCart(int dishId)
        {
            DeleteFromCart(dishId);
            return AllCart();
        }

        public IActionResult AddCart(int dishId)
        {
            AddToCart(dishId);
            return View("Menu", db.Dishes);
        }
        public IActionResult NewOrder(int dishId)
        {
            Order o = CurOrder; 
            if (dishId != -333)
            {
                o = AddToCart(dishId);
            }
            ViewBag.DishAmount = o.Amount;
            return View("NewOrder");
        }

        [HttpPost]
        public IActionResult NewOrder(Order @order)
        {
            order.Amount = CurOrder.Amount;
            order.DishesId = CurOrder.DishesId;
            order.Status = "Confirmed"; 
            order.Amount = CurOrder.Amount;
            db.Orders.Add(@order);
            db.SaveChanges();
            Response.Cookies.Delete("order");
            return RedirectToAction("OrderSuccess");
        }

        [HttpGet]
        public IActionResult Menu()
        {
            return View("Menu", db.Dishes);
        }
        [HttpGet]
        public IActionResult AllCart()
        {
            Order o;
            if (!Request.Cookies.ContainsKey("order"))
            {
                return View("AllCart", new List<CartItem>());
            }
            else
                o = CurOrder;
            List<CartItem> cartList = new List<CartItem>();
            if (o.DishesId == "")
                return View("AllCart", new List<CartItem>());
            else
            {
                List<string> result = o.DishesId.Split('~').ToList();
                foreach (var dishid in result)
                {
                    if (!dishid.Contains("~") && dishid.Length != 0)
                    {
                        Dish dish = GetDishById(Convert.ToInt32(dishid));
                        bool exist = false;
                        foreach (CartItem ci in cartList)
                        {
                            if (ci.DishId == dish.Id)
                            {
                                ci.Quantity++;
                                ci.Amount += dish.Price;
                                exist = true;
                            }
                        }
                        if (!exist)
                        {
                            CartItem c = new CartItem();
                            c.DishId = dish.Id;
                            c.Category = dish.Category;
                            c.Name = dish.Name;
                            c.Amount = dish.Price;
                            c.Price = dish.Price;
                            c.Quantity = 1;
                            cartList.Add(c);
                        }
                    }
                }
                ViewBag.Amount = o.Amount;
                return View("AllCart", cartList);
            }
        } 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
