using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PladoHPComputerSets.Data;
using PladoHPComputerSets.Models;
using PladoHPComputerSets.Models.ViewModels;

namespace PladoHPComputerSets.Controllers
{
    public class ComputerOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public List<ComputerOrder> ShowCompletedOrders { get; private set; }
        public List<ComputerOrder> ShowNotCompletedOrders { get; private set; }

        public ComputerOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ComputerOrders
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return View(await _context.ComputerOrder.ToListAsync());
        }

        public async Task<IActionResult> TrackOrder()
        {

            var model = _context.ComputerOrder
                .Where(r => r.Packed.Equals(false))
                .Select(r => new ComputerOrder
                {
                    Id = r.Id,
                    OrdererName = r.OrdererName,
                    Description = r.Description,
                    Type = r.Type,
                    Case = r.Case,
                    Monitor = r.Monitor,
                    Packed = r.Packed

                }
                );
            return View(model);

        }


        public async Task<IActionResult> StastisticAsync()
        {

            var notCompletedOrders = await _context.ComputerOrder
                .Where(r => r.Packed != null)
                .ToListAsync();

            var completedOrders = await _context.ComputerOrder
                .Where(r => r.Packed.Equals(true))
                .ToListAsync();

            var completedAmount = await _context.ComputerOrder
                .Where(r => r.Packed.Equals(true))
                .CountAsync();


            int totalOrders = _context.ComputerOrder.Count();

            int totalCompletedOrders = _context.ComputerOrder
                .Where(r => r.Packed.Equals(true))
                .Count();








            var result = new ComputerOrderStastisticsViewModel()
                {
                    CompletedOrders = completedOrders,
                    NotCompletedOrders = notCompletedOrders,
                    TotalOrders = totalOrders,
                    TotalCompletedOrders = totalCompletedOrders
        };

            return View(result);
        }

        // GET: ComputerOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ComputerOrder == null)
            {
                return NotFound();
            }

            var computerOrder = await _context.ComputerOrder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computerOrder == null)
            {
                return NotFound();
            }

            return View(computerOrder);
        }

        // GET: ComputerOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ComputerOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrdererName,Description,Price,Type,Case,Monitor,Packed")] ComputerOrder computerOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computerOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(computerOrder);
        }

        // GET: ComputerOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ComputerOrder == null)
            {
                return NotFound();
            }

            var computerOrder = await _context.ComputerOrder.FindAsync(id);
            if (computerOrder == null)
            {
                return NotFound();
            }
            return View(computerOrder);
        }

        // POST: ComputerOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrdererName,Description,Price,Type,Case,Monitor,Packed")] ComputerOrder computerOrder)
        {
            if (id != computerOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(computerOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComputerOrderExists(computerOrder.Id))
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
            return View(computerOrder);
        }

        // GET: ComputerOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ComputerOrder == null)
            {
                return NotFound();
            }

            var computerOrder = await _context.ComputerOrder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computerOrder == null)
            {
                return NotFound();
            }

            return View(computerOrder);
        }

        // POST: ComputerOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ComputerOrder == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ComputerOrder'  is null.");
            }
            var computerOrder = await _context.ComputerOrder.FindAsync(id);
            if (computerOrder != null)
            {
                _context.ComputerOrder.Remove(computerOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComputerOrderExists(int id)
        {
          return _context.ComputerOrder.Any(e => e.Id == id);
        }

        // GET: ComputerOrders/CreateNew
        public IActionResult CreateNew()
        {
            return View();
        }

        // POST: ComputerOrders/CreateNew
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNew([Bind("Id,OrdererName,Description,Price,Type,Case,Monitor,Packed")] ComputerOrder computerOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computerOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TrackOrder));
            }
            return View(computerOrder);
        }
    }
}
