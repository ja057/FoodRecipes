using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodRecipes.Models
{
    public class RecipeReview
    {
        [Key]                       //Primary key
        public int ReviewID { get; set; }

        [Required]
        [StringLength(100)]                 //Lenth used for string is 100
        public string Name { get; set; }            //Name of string

        [Required]                                              //It is manadatory
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string ReviewText { get; set; }

        [Required]
        public int RecipeID { get; set; }                       //Recipe Id is used

        public FoodRecipe FoodRecipe { get; set; }

        public DateTime ReviewDate { get; set; }

    }
}
