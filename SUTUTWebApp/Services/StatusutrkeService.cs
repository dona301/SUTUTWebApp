using Microsoft.EntityFrameworkCore;
using SUTUTWebApp.Exceptions;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Repositories.Interfaces;
using SUTUTWebApp.Services.Interfaces;

namespace SUTUTWebApp.Services;

public class StatusutrkeService : IStatusutrkeService
{
    private readonly IStatusutrkeRepository _statusutrkeRepository;

    public StatusutrkeService(IStatusutrkeRepository statusutrkeRepository)
    {
        _statusutrkeRepository = statusutrkeRepository;
    }

    public Task<IEnumerable<Statusutrke>> GetAllAsync(string? searchString)
        => _statusutrkeRepository.GetAllAsync(searchString);

    public Task<Statusutrke?> GetByIdAsync(int id)
        => _statusutrkeRepository.GetByIdAsync(id);

    public async Task CreateAsync(Statusutrke statusutrke)
    {
        var exists = await _statusutrkeRepository.NazivExistsAsync(statusutrke.Naziv);
        if (exists)
            throw new BusinessValidationException("Status s tim nazivom već postoji.");
        await _statusutrkeRepository.AddAsync(statusutrke);
        await _statusutrkeRepository.SaveChangesAsync();
    }
    public async Task<bool> UpdateAsync(int id, Statusutrke statusutrke)
    {
        if (id != statusutrke.StatusId) return false;
        var exists = await _statusutrkeRepository.NazivExistsAsync(statusutrke.Naziv, excludeId: id);
        if (exists)
            throw new BusinessValidationException("Status s tim nazivom već postoji.");
        try
        {
            await _statusutrkeRepository.UpdateAsync(statusutrke);
            await _statusutrkeRepository.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_statusutrkeRepository.Exists(statusutrke.StatusId))
                return false;
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var statusutrke = await _statusutrkeRepository.GetByIdAsync(id);
        if (statusutrke == null) return false;

        var hasUtrke = await _statusutrkeRepository.HasUtrkeAsync(id);
        if (hasUtrke)
            throw new BusinessValidationException(
                "Nije moguće obrisati status jer postoje utrke koje ga koriste.");

        await _statusutrkeRepository.DeleteAsync(statusutrke);
        await _statusutrkeRepository.SaveChangesAsync();
        return true;
    }

    public bool Exists(int id)
        => _statusutrkeRepository.Exists(id);
    public async Task<bool> NazivExistsAsync(string naziv)
        => await _statusutrkeRepository.NazivExistsAsync(naziv);
}
