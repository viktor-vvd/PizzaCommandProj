using System.Linq;

namespace PizzaCommandProj_Management.Models
{
    public class SampleData
    {
        public static void Initialize(PizzaContext context)
        {
            //Add data
            if (!context.Dishes.Any())
            {
                context.Dishes.AddRange(
                    new Dish
                    {
                        Name = "Arancini, Mushrooms and Mozzarella",
                        Category = "Aperitivi",
                        Price = 8
                    },
                    new Dish
                    {
                        Name = "Parma's ham",
                        Category = "Aperitivi",
                        Price = 14
                    },
                    new Dish
                    {
                        Name = "Veal tartare, Capers, Parmesan cream",
                        Category = "Appetizers",
                        Price = 14
                    },
                    new Dish
                    {
                        Name = "Linguine alla Vongole",
                        Category = "Pasta",
                        Price = 23
                    },
                    new Dish
                    {
                        Name = "Caponakchouka",
                        Category = "Secondi & Piatti freddi",
                        Price = 23
                    },
                    new Dish
                    {
                        Name = "Margherita",
                        Category = "Pizzas",
                        Contain = "Tomato Sauce, Milk Flower, Basil",
                        Price = 12
                    },
                    new Dish
                    {
                        Name = "Pizza Nocciolatta",
                        Category = "Dolci",
                        Price = 10
                    }
                );
                context.SaveChanges();
            }
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(
                    new Order
                    {
                        Amount = 10,
                        Phone = "+380999999999",
                        Adress = "Lutsk, Voli, 11",//qwerty
                        Payment = "Card",
                        Status = "Delivered",
                        DishesId = "2~5"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}