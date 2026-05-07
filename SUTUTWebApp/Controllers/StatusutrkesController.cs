using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SUTUTWebApp.Models.Entities;

namespace SUTUTWebApp.Controllers
{
    public class StatusutrkesController : Controller
    {
        private readonly MasterContext _context;

        public StatusutrkesController(MasterContext context)
        {
            _context = context;
        }

        // GET: Statusutrkes
        public async Task<IActionResult> Index(string searchString)
        {
            var statusi = _context.Statusutrkes.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                statusi = statusi.Where(s => s.Naziv.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await statusi.ToListAsync());
        }

        // GET: Statusutrkes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusutrke = await _context.Statusutrkes
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (statusutrke == null)
            {
                return NotFound();
            }

            return View(statusutrke);
        }

        // GET: Statusutrkes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Statusutrkes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,Naziv")] Statusutrke statusutrke)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statusutrke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statusutrke);
        }

        // GET: Statusutrkes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusutrke = await _context.Statusutrkes.FindAsync(id);
            if (statusutrke == null)
            {
                return NotFound();
            }
            return View(statusutrke);
        }

        // POST: Statusutrkes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,Naziv")] Statusutrke statusutrke)
        {
            if (id != statusutrke.StatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusutrke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusutrkeExists(statusutrke.StatusId))
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
            return View(statusutrke);
        }

        // GET: Statusutrkes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusutrke = await _context.Statusutrkes
                .FirstOrDefaultAsync(m => m.StatusId == id);
            if (statusutrke == null)
            {
                return NotFound();
            }

            return View(statusutrke);
        }

        // POST: Statusutrkes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statusutrke = await _context.Statusutrkes.FindAsync(id);
            if (statusutrke != null)
            {
                _context.Statusutrkes.Remove(statusutrke);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusutrkeExists(int id)
        {
            return _context.Statusutrkes.Any(e => e.StatusId == id);
        }
    }
}
