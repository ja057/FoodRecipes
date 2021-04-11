using System;
using System.Collections.Generic;
using System.Text;
using FoodRecipes.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodRecipes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<FoodCategory> FoodCategories { get; set; }

        public DbSet<FoodRecipe> FoodRecipe { get; set; }

        public DbSet<RecipeReview> RecipeReview { get; set; }

        
    }
}
