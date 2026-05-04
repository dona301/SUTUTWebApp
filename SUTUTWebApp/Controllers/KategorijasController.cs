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
    public class KategorijasController : Controller
    {
        private readonly MasterContext _context;

        public KategorijasController(MasterContext context)
        {
            _context = context;
        }

        // GET: Kategorijas
        public async Task<IActionResult> Index()
        {
            var masterContext = _context.Kategorijas.Include(k => k.Tip).Include(k => k.Utrka);
            return View(await masterContext.ToListAsync());
        }

        // GET: Kategorijas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorijas
                .Include(k => k.Tip)
                .Include(k => k.Utrka)
                .FirstOrDefaultAsync(m => m.KategorijaId == id);
            if (kategorija == null)
            {
                return NotFound();
            }

            return View(kategorija);
        }

        // GET: Kategorijas/Create
        public IActionResult Create()
        {
            ViewData["TipId"] = new SelectList(_context.Tipkategorijes, "TipId", "TipId");
            ViewData["UtrkaId"] = new SelectList(_context.Utrkas, "UtrkaId", "UtrkaId");
            return View();
        }

        // POST: Kategorijas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KategorijaId,Naziv,Duljina,MaxBrojTrkaca,Startnina,Početak,UtrkaId,TipId")] Kategorija kategorija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kategorija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipId"] = new SelectList(_context.Tipkategorijes, "TipId", "TipId", kategorija.TipId);
            ViewData["UtrkaId"] = new SelectList(_context.Utrkas, "UtrkaId", "UtrkaId", kategorija.UtrkaId);
            return View(kategorija);
        }

        // GET: Kategorijas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorijas.FindAsync(id);
            if (kategorija == null)
            {
                return NotFound();
            }
            ViewData["TipId"] = new SelectList(_context.Tipkategorijes, "TipId", "TipId", kategorija.TipId);
            ViewData["UtrkaId"] = new SelectList(_context.Utrkas, "UtrkaId", "UtrkaId", kategorija.UtrkaId);
            return View(kategorija);
        }

        // POST: Kategorijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KategorijaId,Naziv,Duljina,MaxBrojTrkaca,Startnina,Početak,UtrkaId,TipId")] Kategorija kategorija)
        {
            if (id != kategorija.KategorijaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategorija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategorijaExists(kategorija.KategorijaId))
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
            ViewData["TipId"] = new SelectList(_context.Tipkategorijes, "TipId", "TipId", kategorija.TipId);
            ViewData["UtrkaId"] = new SelectList(_context.Utrkas, "UtrkaId", "UtrkaId", kategorija.UtrkaId);
            return View(kategorija);
        }

        // GET: Kategorijas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorijas
                .Include(k => k.Tip)
                .Include(k => k.Utrka)
                .FirstOrDefaultAsync(m => m.KategorijaId == id);
            if (kategorija == null)
            {
                return NotFound();
            }

            return View(kategorija);
        }

        // POST: Kategorijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategorija = await _context.Kategorijas.FindAsync(id);
            if (kategorija != null)
            {
                _context.Kategorijas.Remove(kategorija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KategorijaExists(int id)
        {
            return _context.Kategorijas.Any(e => e.KategorijaId == id);
        }
    }
}
