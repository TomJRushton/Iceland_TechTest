using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Icelane_TechTest.Models
{
   // public class FoodModel
    //{
        public class FoodItem
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public int? SellinValue { get; set; }
            [Required]
            public int? Quality { get; set; }
            public string TextAreaString { get; set; }
            public List<FoodItem> FoodItems { get; set; }
        }

        public class FoodList
        {
            public int AmountOfItems { get; set; }
            
            //public int[] FoodItems { get; set; }


        }
        
///    }

    public class FoodItemString
    {
        public string TextAreaString { get; set; }
    }
}
