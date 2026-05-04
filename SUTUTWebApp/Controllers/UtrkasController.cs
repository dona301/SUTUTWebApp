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
    public class UtrkasController : Controller
    {
        private readonly MasterContext _context;

        public UtrkasController(MasterContext context)
        {
            _context = context;
        }

        // GET: Utrkas
        public async Task<IActionResult> Index()
        {
            var masterContext = _context.Utrkas.Include(u => u.Organizator).Include(u => u.Status);
            return View(await masterContext.ToListAsync());
        }

        // GET: Utrkas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utrka = await _context.Utrkas
                .Include(u => u.Organizator)
                .Include(u => u.Status)
                .FirstOrDefaultAsync(m => m.UtrkaId == id);
            if (utrka == null)
            {
                return NotFound();
            }

            return View(utrka);
        }

        // GET: Utrkas/Create
        public IActionResult Create()
        {
            ViewData["OrganizatorId"] = new SelectList(_context.Organizators, "OrganizatorId", "OrganizatorId");
            ViewData["StatusId"] = new SelectList(_context.Statusutrkes, "StatusId", "StatusId");
            return View();
        }

        // POST: Utrkas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UtrkaId,Naziv,Datum,Grad,Drzava,OrganizatorId,StatusId")] Utrka utrka)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utrka);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizatorId"] = new SelectList(_context.Organizators, "OrganizatorId", "OrganizatorId", utrka.OrganizatorId);
            ViewData["StatusId"] = new SelectList(_context.Statusutrkes, "StatusId", "StatusId", utrka.StatusId);
            return View(utrka);
        }

        // GET: Utrkas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utrka = await _context.Utrkas.FindAsync(id);
            if (utrka == null)
            {
                return NotFound();
            }
            ViewData["OrganizatorId"] = new SelectList(_context.Organizators, "OrganizatorId", "OrganizatorId", utrka.OrganizatorId);
            ViewData["StatusId"] = new SelectList(_context.Statusutrkes, "StatusId", "StatusId", utrka.StatusId);
            return View(utrka);
        }

        // POST: Utrkas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UtrkaId,Naziv,Datum,Grad,Drzava,OrganizatorId,StatusId")] Utrka utrka)
        {
            if (id != utrka.UtrkaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utrka);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtrkaExists(utrka.UtrkaId))
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
            ViewData["OrganizatorId"] = new SelectList(_context.Organizators, "OrganizatorId", "OrganizatorId", utrka.OrganizatorId);
            ViewData["StatusId"] = new SelectList(_context.Statusutrkes, "StatusId", "StatusId", utrka.StatusId);
            return View(utrka);
        }

        // GET: Utrkas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utrka = await _context.Utrkas
                .Include(u => u.Organizator)
                .Include(u => u.Status)
                .FirstOrDefaultAsync(m => m.UtrkaId == id);
            if (utrka == null)
            {
                return NotFound();
            }

            return View(utrka);
        }

        // POST: Utrkas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utrka = await _context.Utrkas.FindAsync(id);
            if (utrka != null)
            {
                _context.Utrkas.Remove(utrka);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtrkaExists(int id)
        {
            return _context.Utrkas.Any(e => e.UtrkaId == id);
        }
    }
}
