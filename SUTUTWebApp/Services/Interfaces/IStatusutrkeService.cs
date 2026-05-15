using SUTUTWebApp.Models.Entities;

namespace SUTUTWebApp.Services.Interfaces;

public interface IStatusutrkeService
{
    Task<IEnumerable<Statusutrke>> GetAllAsync(string? searchString);
    Task<Statusutrke?> GetByIdAsync(int id);
    Task CreateAsync(Statusutrke statusutrke);
    Task<bool> UpdateAsync(int id, Statusutrke statusutrke);
    Task<bool> DeleteAsync(int id);
    bool Exists(int id);
}
