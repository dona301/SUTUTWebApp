using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Repositories.Interfaces;

namespace SUTUTWebApp.Repositories;

public class UtrkaRepository : IUtrkaRepository
{
    private readonly MasterContext _context;

    public UtrkaRepository(MasterContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Utrka>> GetAllAsync(string? searchString)
    {
        var query = _context.Utrkas
            .Include(u => u.Organizator)
            .Include(u => u.Status)
            .Include(u => u.Kategorijas)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(u =>
                u.Naziv.Contains(searchString) ||
                u.Grad.Contains(searchString) ||
                u.Drzava.Contains(searchString) ||
                u.Organizator.Ime.Contains(searchString));
        }

        return await query.ToListAsync();
    }

    public async Task<Utrka?> GetByIdAsync(int id)
    {
        return await _context.Utrkas
            .Include(u => u.Organizator)
            .Include(u => u.Status)
            .FirstOrDefaultAsync(u => u.UtrkaId == id);
    }

    public async Task<Utrka?> GetByIdWithKategorijasAsync(int id)
    {
        return await _context.Utrkas
            .Include(u => u.Kategorijas)
            .FirstOrDefaultAsync(u => u.UtrkaId == id);
    }

    public async Task<Utrka?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Utrkas
            .Include(u => u.Organizator)
            .Include(u => u.Status)
            .Include(u => u.Kategorijas)
                .ThenInclude(k => k.Tip)
            .FirstOrDefaultAsync(u => u.UtrkaId == id);
    }

    public async Task AddAsync(Utrka utrka)
    {
        await _context.Utrkas.AddAsync(utrka);
    }

    public Task UpdateAsync(Utrka utrka)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Utrka utrka)
    {
        _context.Kategorijas.RemoveRange(utrka.Kategorijas);
        _context.Utrkas.Remove(utrka);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<List<SelectListItem>> GetOrganizatoriSelectAsync()
    {
        return await _context.Organizators
            .Select(o => new SelectListItem
            {
                Value = o.OrganizatorId.ToString(),
                Text = o.Ime
            })
            .ToListAsync();
    }

    public async Task<List<SelectListItem>> GetStatusiSelectAsync()
    {
        return await _context.Statusutrkes
            .Select(s => new SelectListItem
            {
                Value = s.StatusId.ToString(),
                Text = s.Naziv
            })
            .ToListAsync();
    }

    public async Task<List<SelectListItem>> GetTipoviKategorijeSelectAsync()
    {
        return await _context.Tipkategorijes
            .Select(t => new SelectListItem
            {
                Value = t.TipId.ToString(),
                Text = t.Naziv
            })
            .ToListAsync();
    }
    public void RemoveKategorija(Kategorija kategorija)
    {
        _context.Kategorijas.Remove(kategorija);
    }
}