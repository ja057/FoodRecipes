using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodRecipes.Models
{
    public class FoodCategory
    {
        [Key]                          //Primary Key
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]                     //String length is 100
        public string CategoryName { get; set; }                //Name of category

        public virtual ICollection<FoodRecipe> FoodRecipes { get; set; }

    }
}
