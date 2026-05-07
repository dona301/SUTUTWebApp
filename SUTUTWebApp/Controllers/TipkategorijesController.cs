using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SUTUTWebApp.Models.Entities;

namespace SUTUTWebApp.Controllers
{
    public class TipkategorijesController : Controller
    {
        private readonly MasterContext _context;

        public TipkategorijesController(MasterContext context)
        {
            _context = context;
        }

        // GET: Tipkategorijes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tipkategorijes.ToListAsync());
        }

        // GET: Tipkategorijes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipkategorije = await _context.Tipkategorijes
                .FirstOrDefaultAsync(m => m.TipId == id);
            if (tipkategorije == null)
            {
                return NotFound();
            }

            return View(tipkategorije);
        }

        // GET: Tipkategorijes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tipkategorijes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipId,Naziv")] Tipkategorije tipkategorije)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipkategorije);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipkategorije);
        }

        // GET: Tipkategorijes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipkategorije = await _context.Tipkategorijes.FindAsync(id);
            if (tipkategorije == null)
            {
                return NotFound();
            }
            return View(tipkategorije);
        }

        // POST: Tipkategorijes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipId,Naziv")] Tipkategorije tipkategorije)
        {
            if (id != tipkategorije.TipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipkategorije);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipkategorijeExists(tipkategorije.TipId))
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
            return View(tipkategorije);
        }

        // GET: Tipkategorijes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipkategorije = await _context.Tipkategorijes
                .FirstOrDefaultAsync(m => m.TipId == id);
            if (tipkategorije == null)
            {
                return NotFound();
            }

            return View(tipkategorije);
        }

        // POST: Tipkategorijes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipkategorije = await _context.Tipkategorijes.FindAsync(id);
            if (tipkategorije != null)
            {
                _context.Tipkategorijes.Remove(tipkategorije);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipkategorijeExists(int id)
        {
            return _context.Tipkategorijes.Any(e => e.TipId == id);
        }
    }
}
