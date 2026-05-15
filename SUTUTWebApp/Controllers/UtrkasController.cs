using Microsoft.AspNetCore.Mvc;
using SUTUTWebApp.Models.ViewModels;
using SUTUTWebApp.Services.Interfaces;

namespace SUTUTWebApp.Controllers;

public class UtrkasController : Controller
{
    private readonly IUtrkaService _utrkaService;

    public UtrkasController(IUtrkaService utrkaService)
    {
        _utrkaService = utrkaService;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        var utrke = await _utrkaService.GetAllAsync(searchString);
        return View(utrke);
    }

    public async Task<IActionResult> Details(int id)
    {
        var utrka = await _utrkaService.GetByIdWithDetailsAsync(id);
        if (utrka == null) return NotFound();
        return View(utrka);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new UtrkaFormVM();
        await _utrkaService.PopulateDropdownsAsync(vm);
        return View("Form", vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UtrkaFormVM vm)
    {
        ValidateBusiness(vm);

        if (!ModelState.IsValid)
        {
            await _utrkaService.PopulateDropdownsAsync(vm);
            return View("Form", vm);
        }

        await _utrkaService.CreateAsync(vm);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _utrkaService.GetFormVmForEditAsync(id);
        if (vm == null) return NotFound();
        return View("Form", vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UtrkaFormVM vm)
    {
        ValidateBusiness(vm);

        if (!ModelState.IsValid)
        {
            await _utrkaService.PopulateDropdownsAsync(vm);
            return View("Form", vm);
        }

        var found = await _utrkaService.UpdateAsync(vm);
        if (!found) return NotFound();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var utrka = await _utrkaService.GetByIdAsync(id);
        if (utrka == null) return NotFound();
        return View(utrka);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _utrkaService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // Business validation stays in the controller — it works with ModelState
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