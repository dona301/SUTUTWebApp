using Microsoft.EntityFrameworkCore;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Repositories.Interfaces;

namespace SUTUTWebApp.Repositories;

public class StatusutrkeRepository : IStatusutrkeRepository
{
    private readonly MasterContext _context;

    public StatusutrkeRepository(MasterContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Statusutrke>> GetAllAsync(string? searchString)
    {
        var query = _context.Statusutrkes.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(s => s.Naziv.Contains(searchString));
        }

        return await query.ToListAsync();
    }

    public async Task<Statusutrke?> GetByIdAsync(int id)
    {
        return await _context.Statusutrkes.FirstOrDefaultAsync(s => s.StatusId == id);
    }

    public async Task AddAsync(Statusutrke statusutrke)
    {
        await _context.Statusutrkes.AddAsync(statusutrke);
    }

    public async Task UpdateAsync(Statusutrke statusutrke)
    {
        var tracked = await _context.Statusutrkes.FindAsync(statusutrke.StatusId);
        if (tracked != null)
            _context.Entry(tracked).CurrentValues.SetValues(statusutrke);
    }

    public Task DeleteAsync(Statusutrke statusutrke)
    {
        _context.Statusutrkes.Remove(statusutrke);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public bool Exists(int id)
    {
        return _context.Statusutrkes.Any(s => s.StatusId == id);
    }
}