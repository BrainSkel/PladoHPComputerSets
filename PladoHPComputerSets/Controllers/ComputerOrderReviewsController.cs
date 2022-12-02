using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PladoHPComputerSets.Data;
using PladoHPComputerSets.Models;

namespace PladoHPComputerSets.Controllers
{
    public class ComputerOrderReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComputerOrderReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComputerOrderReviews
        public async Task<IActionResult> Index()
        {
              return View(await _context.ComputerOrderReview.ToListAsync());
        }

        // GET: ComputerOrderReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ComputerOrderReview == null)
            {
                return NotFound();
            }

            var computerOrderReview = await _context.ComputerOrderReview
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computerOrderReview == null)
            {
                return NotFound();
            }

            return View(computerOrderReview);
        }

        // GET: ComputerOrderReviews/Create
        
        public IActionResult Create()
        {
            return View();
        }

        // POST: ComputerOrderReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderId,ReviewerName,Title,Comment,Rating")] ComputerOrderReview computerOrderReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computerOrderReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(computerOrderReview);
        }

        // GET: ComputerOrderReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ComputerOrderReview == null)
            {
                return NotFound();
            }

            var computerOrderReview = await _context.ComputerOrderReview.FindAsync(id);
            if (computerOrderReview == null)
            {
                return NotFound();
            }
            return View(computerOrderReview);
        }

        // POST: ComputerOrderReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,ReviewerName,Title,Comment,Rating")] ComputerOrderReview computerOrderReview)
        {
            if (id != computerOrderReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computerOrderReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputerOrderReviewExists(computerOrderReview.Id))
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
            return View(computerOrderReview);
        }

        // GET: ComputerOrderReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ComputerOrderReview == null)
            {
                return NotFound();
            }

            var computerOrderReview = await _context.ComputerOrderReview
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computerOrderReview == null)
            {
                return NotFound();
            }

            return View(computerOrderReview);
        }

        // POST: ComputerOrderReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ComputerOrderReview == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ComputerOrderReview'  is null.");
            }
            var computerOrderReview = await _context.ComputerOrderReview.FindAsync(id);
            if (computerOrderReview != null)
            {
                _context.ComputerOrderReview.Remove(computerOrderReview);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComputerOrderReviewExists(int id)
        {
          return _context.ComputerOrderReview.Any(e => e.Id == id);
        }
    }
}
