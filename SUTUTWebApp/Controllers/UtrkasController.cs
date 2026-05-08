using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Models.ViewModels;

namespace SUTUTWebApp.Controllers;

public class UtrkasController : Controller
{
    private readonly MasterContext _context;

    public UtrkasController(MasterContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;

        var utrke = _context.Utrkas
            .Include(u => u.Organizator)
            .Include(u => u.Status)
            .Include(u => u.Kategorijas)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            utrke = utrke.Where(u =>
                u.Naziv.Contains(searchString) ||
                u.Grad.Contains(searchString) ||
                u.Drzava.Contains(searchString) ||
                u.Organizator.Ime.Contains(searchString));
        }

        return View(await utrke.ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        var vm = new UtrkaFormVM();
        await PopulateDropdowns(vm);
        return View("Form", vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UtrkaFormVM vm)
    {
        ValidateBusiness(vm);

        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(vm);
            return View("Form", vm);
        }

        var utrka = new Utrka
        {
            Naziv = vm.Naziv,
            Datum = vm.Datum,
            Grad = vm.Grad,
            Drzava = vm.Drzava,
            OrganizatorId = vm.OrganizatorId,
            StatusId = vm.StatusId
        };
        _context.Utrkas.Add(utrka);
        await _context.SaveChangesAsync();

        foreach (var row in vm.Kategorije.Where(k => !k.IsDeleted))
        {
            _context.Kategorijas.Add(new Kategorija
            {
                Naziv = row.Naziv,
                Duljina = row.Duljina,
                MaxBrojTrkaca = row.MaxBrojTrkaca,
                Startnina = row.Startnina,
                Početak = row.Pocetak,
                UtrkaId = utrka.UtrkaId,
                TipId = row.TipId
            });
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var utrka = await _context.Utrkas
            .Include(u => u.Kategorijas)
            .FirstOrDefaultAsync(u => u.UtrkaId == id);

        if (utrka == null) return NotFound();

        var vm = new UtrkaFormVM
        {
            UtrkaId = utrka.UtrkaId,
            Naziv = utrka.Naziv,
            Datum = utrka.Datum,
            Grad = utrka.Grad,
            Drzava = utrka.Drzava,
            OrganizatorId = utrka.OrganizatorId,
            StatusId = utrka.StatusId,
            Kategorije = utrka.Kategorijas.Select(k => new KategorijaRowVM
            {
                KategorijaId = k.KategorijaId,
                Naziv = k.Naziv,
                Duljina = k.Duljina,
                MaxBrojTrkaca = k.MaxBrojTrkaca,
                Startnina = k.Startnina,
                Pocetak = k.Početak,
                TipId = k.TipId
            }).ToList()
        };

        await PopulateDropdowns(vm);
        return View("Form", vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UtrkaFormVM vm)
    {
        ValidateBusiness(vm);

        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(vm);
            return View("Form", vm);
        }

        var utrka = await _context.Utrkas
            .Include(u => u.Kategorijas)
            .FirstOrDefaultAsync(u => u.UtrkaId == vm.UtrkaId);

        if (utrka == null) return NotFound();

        utrka.Naziv = vm.Naziv;
        utrka.Datum = vm.Datum;
        utrka.Grad = vm.Grad;
        utrka.Drzava = vm.Drzava;
        utrka.OrganizatorId = vm.OrganizatorId;
        utrka.StatusId = vm.StatusId;

        foreach (var row in vm.Kategorije)
        {
            if (row.KategorijaId == 0)
            {
                if (!row.IsDeleted)
                {
                    _context.Kategorijas.Add(new Kategorija
                    {
                        Naziv = row.Naziv,
                        Duljina = row.Duljina,
                        MaxBrojTrkaca = row.MaxBrojTrkaca,
                        Startnina = row.Startnina,
                        Početak = row.Pocetak,
                        UtrkaId = utrka.UtrkaId,
                        TipId = row.TipId
                    });
                }
            }
            else
            {
                var existing = utrka.Kategorijas.First(k => k.KategorijaId == row.KategorijaId);
                if (row.IsDeleted)
                {
                    _context.Kategorijas.Remove(existing);
                }
                else
                {
                    existing.Naziv = row.Naziv;
                    existing.Duljina = row.Duljina;
                    existing.MaxBrojTrkaca = row.MaxBrojTrkaca;
                    existing.Startnina = row.Startnina;
                    existing.Početak = row.Pocetak;
                    existing.TipId = row.TipId;
                }
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var utrka = await _context.Utrkas
            .Include(u => u.Organizator)
            .Include(u => u.Status)
            .FirstOrDefaultAsync(u => u.UtrkaId == id);
        if (utrka == null) return NotFound();
        return View(utrka);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var utrka = await _context.Utrkas
            .Include(u => u.Kategorijas)
            .FirstOrDefaultAsync(u => u.UtrkaId == id);
        if (utrka != null)
        {
            _context.Kategorijas.RemoveRange(utrka.Kategorijas);
            _context.Utrkas.Remove(utrka);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns(UtrkaFormVM vm)
    {
        vm.Organizatori = await _context.Organizators
            .Select(o => new SelectListItem { Value = o.OrganizatorId.ToString(), Text = o.Ime })
            .ToListAsync();

        vm.Statusi = await _context.Statusutrkes
            .Select(s => new SelectListItem { Value = s.StatusId.ToString(), Text = s.Naziv })
            .ToListAsync();

        vm.TipoviKategorije = await _context.Tipkategorijes
            .Select(t => new SelectListItem { Value = t.TipId.ToString(), Text = t.Naziv })
            .ToListAsync();
    }

    public async Task<IActionResult> Details(int id)
    {
        var utrka = await _context.Utrkas
            .Include(u => u.Organizator)
            .Include(u => u.Status)
            .Include(u => u.Kategorijas)
                .ThenInclude(k => k.Tip)
            .FirstOrDefaultAsync(u => u.UtrkaId == id);

        if (utrka == null) return NotFound();

        return View(utrka);
    }

    private void ValidateBusiness(UtrkaFormVM vm)
    {
        var activeRows = vm.Kategorije.Where(k => !k.IsDeleted).ToList();

        foreach (var k in activeRows)
        {
            if (k.Pocetak < vm.Datum)
                ModelState.AddModelError("",
                    $"Kategorija '{k.Naziv}': datum početka ({k.Pocetak}) ne može biti prije datuma utrke ({vm.Datum}).");
        }

        var dupes = activeRows
            .GroupBy(k => new { k.Duljina, k.TipId })
            .Where(g => g.Count() > 1);
        foreach (var d in dupes)
            ModelState.AddModelError("",
                $"Postoje dvije kategorije iste duljine ({d.Key.Duljina} km) i istog tipa.");
    }
}