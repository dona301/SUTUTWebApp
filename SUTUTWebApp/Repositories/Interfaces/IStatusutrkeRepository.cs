using SUTUTWebApp.Models.Entities;

namespace SUTUTWebApp.Repositories.Interfaces;

public interface IStatusutrkeRepository
{
    Task<IEnumerable<Statusutrke>> GetAllAsync(string? searchString);
    Task<Statusutrke?> GetByIdAsync(int id);
    Task AddAsync(Statusutrke statusutrke);
    Task UpdateAsync(Statusutrke statusutrke);
    Task DeleteAsync(Statusutrke statusutrke);
    Task SaveChangesAsync();
    bool Exists(int id);
    Task<bool> NazivExistsAsync(string naziv, int? excludeId = null);

}
