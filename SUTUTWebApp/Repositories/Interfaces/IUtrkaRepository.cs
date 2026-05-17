using Microsoft.AspNetCore.Mvc.Rendering;
using SUTUTWebApp.Models.Entities;

namespace SUTUTWebApp.Repositories.Interfaces;

public interface IUtrkaRepository
{
    // Queries
    Task<IEnumerable<Utrka>> GetAllAsync(string? searchString);
    Task<Utrka?> GetByIdAsync(int id);
    Task<Utrka?> GetByIdWithKategorijasAsync(int id);
    Task<Utrka?> GetByIdWithDetailsAsync(int id);

    // Commands
    Task AddAsync(Utrka utrka);
    Task UpdateAsync(Utrka utrka);
    Task DeleteAsync(Utrka utrka);
    Task SaveChangesAsync();
    void RemoveKategorija(Kategorija kategorija);
    Task<bool> HasRezultatiAsync(int utrkaId);
    Task<bool> HasRezultatiForKategorijaAsync(int kategorijaId);

    // Dropdown
    Task<List<SelectListItem>> GetOrganizatoriSelectAsync();
    Task<List<SelectListItem>> GetStatusiSelectAsync();
    Task<List<SelectListItem>> GetTipoviKategorijeSelectAsync();
}