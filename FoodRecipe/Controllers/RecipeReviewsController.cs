using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodRecipes.Data;
using FoodRecipes.Models;
using Microsoft.AspNetCore.Authorization;

namespace FoodRecipes.Controllers
{
    [Authorize]
    public class RecipeReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipeReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RecipeReviews
        public async Task<IActionResult> Index()
        {
            return View(await _context.RecipeReview.ToListAsync());
        }

        // GET: RecipeReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeReview = await _context.RecipeReview
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (recipeReview == null)
            {
                return NotFound();
            }

            return View(recipeReview);
        }



        // GET: RecipeReviews/Create
       
        public IActionResult Create()
        {
            ViewData["RecipeID"] = new SelectList(_context.FoodRecipe, "RecipeID", "RecipeName");
            return View();
        }

        // POST: RecipeReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewID,Name,Rating,ReviewText,RecipeID,ReviewDate")] RecipeReview recipeReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipeReview);
        }

       
        // GET: RecipeReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeReview = await _context.RecipeReview.FindAsync(id);
            if (recipeReview == null)
            {
                return NotFound();
            }
            return View(recipeReview);
        }

        // POST: RecipeReviews/Edit/5
              [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewID,Name,Rating,ReviewText,RecipeID,ReviewDate")] RecipeReview recipeReview)
        {
            if (id != recipeReview.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeReviewExists(recipeReview.ReviewID))
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
            return View(recipeReview);
        }

       
        // GET: RecipeReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeReview = await _context.RecipeReview
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (recipeReview == null)
            {
                return NotFound();
            }

            return View(recipeReview);
        }

        // POST: RecipeReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeReview = await _context.RecipeReview.FindAsync(id);
            _context.RecipeReview.Remove(recipeReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeReviewExists(int id)
        {
            return _context.RecipeReview.Any(e => e.ReviewID == id);
        }
    }
}
