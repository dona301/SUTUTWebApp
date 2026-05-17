using Microsoft.AspNetCore.Mvc;
using SUTUTWebApp.Exceptions;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Models.ViewModels;
using SUTUTWebApp.Services.Interfaces;

namespace SUTUTWebApp.Controllers;

public class StatusutrkesController : Controller
{
    private readonly IStatusutrkeService _statusutrkeService;

    public StatusutrkesController(IStatusutrkeService statusutrkeService)
    {
        _statusutrkeService = statusutrkeService;
    }

    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["CurrentFilter"] = searchString;
        var statusi = await _statusutrkeService.GetAllAsync(searchString);
        return View(statusi);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var statusutrke = await _statusutrkeService.GetByIdAsync(id.Value);
        if (statusutrke == null) return NotFound();
        return View(statusutrke);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("StatusId,Naziv")] Statusutrke statusutrke)
    {
        if (!ModelState.IsValid) return View(statusutrke);
        try
        {
            await _statusutrkeService.CreateAsync(statusutrke);
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessValidationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(statusutrke);
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var statusutrke = await _statusutrkeService.GetByIdAsync(id.Value);
        if (statusutrke == null) return NotFound();
        return View(statusutrke);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("StatusId,Naziv")] Statusutrke statusutrke)
    {
        if (!ModelState.IsValid) return View(statusutrke);
        try
        {
            var found = await _statusutrkeService.UpdateAsync(id, statusutrke);
            if (!found) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessValidationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(statusutrke);
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var statusutrke = await _statusutrkeService.GetByIdAsync(id.Value);
        if (statusutrke == null) return NotFound();
        return View(statusutrke);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _statusutrkeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (BusinessValidationException ex)
        {
            var status = await _statusutrkeService.GetByIdAsync(id);
            ModelState.AddModelError("", ex.Message);
            return View("Delete", status);
        }
    }
}