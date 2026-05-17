using Microsoft.AspNetCore.Mvc;
using SUTUTWebApp.Exceptions;
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
        try
        {
            _utrkaService.ValidateBusiness(vm);
        }
        catch (BusinessValidationException ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

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
        try
        {
            _utrkaService.ValidateBusiness(vm);
        }
        catch (BusinessValidationException ex)
        {
            ModelState.AddModelError("", ex.Message);
        }

        if (!ModelState.IsValid)
        {
            await _utrkaService.PopulateDropdownsAsync(vm);
            return View("Form", vm);
        }

        try
        {
            var found = await _utrkaService.UpdateAsync(vm);
            if (!found) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessValidationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            await _utrkaService.PopulateDropdownsAsync(vm);
            return View("Form", vm);
        }
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
        try
        {
            await _utrkaService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessValidationException ex)
        {
            var utrka = await _utrkaService.GetByIdAsync(id);
            ModelState.AddModelError("", ex.Message);
            return View(utrka);
        }
    }
}