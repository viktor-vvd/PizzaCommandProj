using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaCommandProj_Management.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        public string Contain { get; set; }

    }
}
