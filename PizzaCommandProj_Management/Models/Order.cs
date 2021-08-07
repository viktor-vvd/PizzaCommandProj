using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCommandProj_Management.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public string DishesId { get; set; }
        //public List<Int32> DishesId { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Adress { get; set; }

        [Required]
        public string Payment { get; set; }

        [Required]
        public string Status { get; set; }

    }
}
