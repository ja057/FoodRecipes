using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodRecipes.Models;
using FoodRecipes.Data;
using Microsoft.EntityFrameworkCore;

namespace GSMArena_Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.FoodCategories.OrderBy(x => Guid.NewGuid()).ToListAsync());
        }

        public async Task<IActionResult> AllCategories()
        {
            return View(await _context.FoodCategories.OrderBy(x => Guid.NewGuid()).ToListAsync());
        }

        public async Task<IActionResult> ViewRecipeByCategory(int? id)
        {
            var applicationDbContext = _context.FoodRecipe
            .Include(b => b.FoodCategories).Where(m => m.CategoryID== id);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> ViewRecipeID(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobile = await _context.FoodRecipe
                .Include(b => b.FoodCategories)
                .FirstOrDefaultAsync(m => m.RecipeID == id);
            if (mobile == null)
            {
                return NotFound();
            }

            return View(mobile);
        }
        public async Task<IActionResult> ViewReviews(int? id)
        {
            var applicationDbContext = _context.RecipeReview
            .Include(b => b.FoodRecipe).Where(m => m.RecipeID == id);
            return View(await applicationDbContext.ToListAsync());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
