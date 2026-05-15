using Microsoft.AspNetCore.Mvc.Rendering;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Models.ViewModels;

namespace SUTUTWebApp.Services.Interfaces;

public interface IUtrkaService
{
    Task<IEnumerable<Utrka>> GetAllAsync(string? searchString);
    Task<Utrka?> GetByIdAsync(int id);
    Task<Utrka?> GetByIdWithDetailsAsync(int id);
    Task PopulateDropdownsAsync(UtrkaFormVM vm);
    Task<UtrkaFormVM?> GetFormVmForEditAsync(int id);
    Task CreateAsync(UtrkaFormVM vm);
    Task<bool> UpdateAsync(UtrkaFormVM vm);
    Task<bool> DeleteAsync(int id);
}
