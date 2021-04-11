using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodRecipes.Data;
using FoodRecipes.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace FoodRecipes.Controllers
{
   
    public class FoodRecipesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public FoodRecipesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        // GET: FoodRecipes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.FoodRecipe.Include(f => f.FoodCategories);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: FoodRecipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodRecipe = await _context.FoodRecipe
                .Include(f => f.FoodCategories)
                .FirstOrDefaultAsync(m => m.RecipeID == id);
            if (foodRecipe == null)
            {
                return NotFound();
            }

            return View(foodRecipe);
        }

        [Authorize]
        // GET: FoodRecipes/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.FoodCategories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: FoodRecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeID,RecipeName,PrepTime,RecipeBy,File,Ingredients,RecipeDetail,CategoryID")] FoodRecipe foodRecipe)
        {
            using (var memoryStream = new MemoryStream())
            {
                await foodRecipe.File.FormFile.CopyToAsync(memoryStream);

                string photoname = foodRecipe.File.FormFile.FileName;
                foodRecipe.Extension = Path.GetExtension(photoname);
                if (!".jpg.jpeg.png.gif.bmp".Contains(foodRecipe.Extension.ToLower()))
                {
                    ModelState.AddModelError("File.FormFile", "Invalid Format of Image Given.");
                }
                else
                {
                    ModelState.Remove("Extension");
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(foodRecipe);
                await _context.SaveChangesAsync();
                var uploadsRootFolder = Path.Combine(_environment.WebRootPath, "recipephotos");
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }
                string filename = foodRecipe.RecipeID + foodRecipe.Extension;
                var filePath = Path.Combine(uploadsRootFolder, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await foodRecipe.File.FormFile.CopyToAsync(fileStream).ConfigureAwait(false);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.FoodCategories, "CategoryID", "CategoryName", foodRecipe.CategoryID);
            return View(foodRecipe);
        }
        [Authorize]
        // GET: FoodRecipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodRecipe = await _context.FoodRecipe.FindAsync(id);
            if (foodRecipe == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.FoodCategories, "CategoryID", "CategoryName", foodRecipe.CategoryID);
            return View(foodRecipe);
        }

        // POST: FoodRecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeID,RecipeName,PrepTime,RecipeBy,Extension,Ingredients,RecipeDetail,CategoryID")] FoodRecipe foodRecipe)
        {
            if (id != foodRecipe.RecipeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foodRecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodRecipeExists(foodRecipe.RecipeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.FoodCategories, "CategoryID", "CategoryName", foodRecipe.CategoryID);
            return View(foodRecipe);
        }

        // GET: FoodRecipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodRecipe = await _context.FoodRecipe
                .Include(f => f.FoodCategories)
                .FirstOrDefaultAsync(m => m.RecipeID == id);
            if (foodRecipe == null)
            {
                return NotFound();
            }

            return View(foodRecipe);
        }

        // POST: FoodRecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodRecipe = await _context.FoodRecipe.FindAsync(id);
            _context.FoodRecipe.Remove(foodRecipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodRecipeExists(int id)
        {
            return _context.FoodRecipe.Any(e => e.RecipeID == id);
        }
    }
}
