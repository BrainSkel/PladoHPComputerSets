﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Unobtrusive.Ajax;
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
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Orders(List<ComputerOrder> model)
        {
              return View(await _context.ComputerOrder.ToListAsync());
        }

        private static Random random = new Random();


        public async Task<IActionResult> TrackOrder(string searchTerm = " ")
        {
             var model = _context.ComputerOrder
                            .OrderByDescending(r => r.OrdererName)
                            .Where(r => r.Packed.Equals(false))
                            .Where(r=> r.TrackingNR == searchTerm)
                            .Select(r => new ComputerOrder
                            {
                                Id = r.Id,
                                TrackingNR = r.TrackingNR,
                                OrdererName = r.OrdererName,
                                Description = r.Description,
                                Type = r.Type,
                                Price = r.Price,
                                Case = r.Case,
                                Monitor = r.Monitor,
                                Packed = r.Packed

                            }
                            );
                return View(model);

        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> StastisticAsync()
        {

            var notCompletedOrders = await _context.ComputerOrder
                .Where(r => r.Packed.Equals(false))
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





        public ActionResult Search(string searchString = null)
        {
            var records = from r in _context.ComputerOrder
                          select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                records = records.Where(s => s.OrdererName.Contains(searchString));
            }

            return View(records);
        }




        // GET: ComputerOrders/Details/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,OrdererName,TrackingNR,Description,Price,Type,Case,Monitor,Packed")] ComputerOrder computerOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(computerOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Orders));
            }
            return View(computerOrder);
        }

        // GET: ComputerOrders/Edit/5
        public async Task<IActionResult> Orders_edit(int? id)
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Orders_Edit(int id, [Bind("Id,OrdererName,TrackingNR,Description,Price,Type,Case,Monitor,Packed")] ComputerOrder computerOrder)
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
                return RedirectToAction(nameof(Orders));
            }
            return View(computerOrder);
        }

        // GET: ComputerOrders/Delete/5
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Orders_delete(int? id)
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
            return RedirectToAction(nameof(Orders));
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
        public async Task<IActionResult> CreateNew([Bind("Id,TrackingNR,OrdererName,Description,Price,Type,Case,Monitor,Packed")] ComputerOrder computerOrder)
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
