using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Models.ViewModels;
using SUTUTWebApp.Repositories;
using SUTUTWebApp.Repositories.Interfaces;
using SUTUTWebApp.Services.Interfaces;

namespace SUTUTWebApp.Services;

public class UtrkaService : IUtrkaService
{
    private readonly IUtrkaRepository _utrkaRepository;

    public UtrkaService(IUtrkaRepository utrkaRepository)
    {
        _utrkaRepository = utrkaRepository;
    }

    public Task<IEnumerable<Utrka>> GetAllAsync(string? searchString)
        => _utrkaRepository.GetAllAsync(searchString);

    public Task<Utrka?> GetByIdAsync(int id)
        => _utrkaRepository.GetByIdAsync(id);

    public Task<Utrka?> GetByIdWithDetailsAsync(int id)
        => _utrkaRepository.GetByIdWithDetailsAsync(id);
    public async Task PopulateDropdownsAsync(UtrkaFormVM vm)
    {
        vm.Organizatori = await _utrkaRepository.GetOrganizatoriSelectAsync();
        vm.Statusi = await _utrkaRepository.GetStatusiSelectAsync();
        vm.TipoviKategorije = await _utrkaRepository.GetTipoviKategorijeSelectAsync();
    }

    public async Task<UtrkaFormVM?> GetFormVmForEditAsync(int id)
    {
        var utrka = await _utrkaRepository.GetByIdWithKategorijasAsync(id);
        if (utrka == null) return null;

        var vm = new UtrkaFormVM
        {
            UtrkaId = utrka.UtrkaId,
            Naziv = utrka.Naziv,
            Datum = utrka.Datum,
            Grad = utrka.Grad,
            Drzava = utrka.Drzava,
            OrganizatorId = utrka.OrganizatorId,
            StatusId = utrka.StatusId,
            Kategorije = utrka.Kategorijas.Select(k => new KategorijaRowVM
            {
                KategorijaId = k.KategorijaId,
                Naziv = k.Naziv,
                Duljina = k.Duljina,
                MaxBrojTrkaca = k.MaxBrojTrkaca,
                Startnina = k.Startnina,
                Pocetak = k.Početak,
                TipId = k.TipId
            }).ToList()
        };

        await PopulateDropdownsAsync(vm);
        return vm;
    }

    public async Task CreateAsync(UtrkaFormVM vm)
    {
        var utrka = new Utrka
        {
            Naziv = vm.Naziv,
            Datum = vm.Datum,
            Grad = vm.Grad,
            Drzava = vm.Drzava,
            OrganizatorId = vm.OrganizatorId,
            StatusId = vm.StatusId
        };

        await _utrkaRepository.AddAsync(utrka);
        await _utrkaRepository.SaveChangesAsync();

        foreach (var row in vm.Kategorije.Where(k => !k.IsDeleted))
        {
            utrka.Kategorijas.Add(new Kategorija
            {
                Naziv = row.Naziv,
                Duljina = row.Duljina,
                MaxBrojTrkaca = row.MaxBrojTrkaca,
                Startnina = row.Startnina,
                Početak = row.Pocetak,
                UtrkaId = utrka.UtrkaId,
                TipId = row.TipId
            });
        }

        await _utrkaRepository.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(UtrkaFormVM vm)
    {
        var utrka = await _utrkaRepository.GetByIdWithKategorijasAsync(vm.UtrkaId);
        if (utrka == null) return false;

        utrka.Naziv = vm.Naziv;
        utrka.Datum = vm.Datum;
        utrka.Grad = vm.Grad;
        utrka.Drzava = vm.Drzava;
        utrka.OrganizatorId = vm.OrganizatorId;
        utrka.StatusId = vm.StatusId;

        foreach (var row in vm.Kategorije)
        {
            if (row.KategorijaId == 0)
            {
                if (!row.IsDeleted)
                {
                    utrka.Kategorijas.Add(new Kategorija
                    {
                        Naziv = row.Naziv,
                        Duljina = row.Duljina,
                        MaxBrojTrkaca = row.MaxBrojTrkaca,
                        Startnina = row.Startnina,
                        Početak = row.Pocetak,
                        UtrkaId = utrka.UtrkaId,
                        TipId = row.TipId
                    });
                }
            }
            else
            {
                var existing = utrka.Kategorijas.First(k => k.KategorijaId == row.KategorijaId);
                if (row.IsDeleted)
                {
                    _utrkaRepository.RemoveKategorija(existing);
                }
                else
                {
                    existing.Naziv = row.Naziv;
                    existing.Duljina = row.Duljina;
                    existing.MaxBrojTrkaca = row.MaxBrojTrkaca;
                    existing.Startnina = row.Startnina;
                    existing.Početak = row.Pocetak;
                    existing.TipId = row.TipId;
                }
            }
        }

        await _utrkaRepository.UpdateAsync(utrka);
        await _utrkaRepository.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var utrka = await _utrkaRepository.GetByIdWithKategorijasAsync(id);
        if (utrka == null) return false;

        await _utrkaRepository.DeleteAsync(utrka);
        await _utrkaRepository.SaveChangesAsync();
        return true;
    }
}