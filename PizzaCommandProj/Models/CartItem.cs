using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCommandProj.Models
{
    public class CartItem
    {
        Dish Dish { get; set; }
        int Quantity { get; set; }
        int Amount { get; set; }
    }
}
