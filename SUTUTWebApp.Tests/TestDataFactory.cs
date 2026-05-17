using Microsoft.EntityFrameworkCore;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Models.ViewModels;

namespace SUTUTWebApp.Tests;

public static class TestDataFactory
{
    public static Statusutrke MakeStatus(int id = 1, string naziv = "Aktivan") =>
        new() { StatusId = id, Naziv = naziv };

    public static Organizator MakeOrganizator(int id = 1, string ime = "Test Org") =>
        new()
        {
            OrganizatorId = id,
            Ime = ime,
            VrstaOrganizatoraId = 1,
            BrojMobitela = "0912345678",
            Email = "d@d.com",
            Oib = "12345678901",
            Opis = "Opis organizatora",
        };

    public static Tipkategorije MakeTip(int id = 1, string naziv = "Trail") =>
        new() { TipId = id, Naziv = naziv };

    public static Utrka MakeUtrka(int id = 1, int statusId = 1, int organizatorId = 1) =>
        new()
        {
            UtrkaId = id,
            Naziv = "Test Utrka",
            Datum = new DateOnly(2025, 6, 1),
            Grad = "Zagreb",
            Drzava = "Hrvatska",
            OrganizatorId = organizatorId,
            StatusId = statusId
        };

    public static Kategorija MakeKategorija(int id = 1, int utrkaId = 1, int tipId = 1) =>
        new()
        {
            KategorijaId = id,
            Naziv = "5km",
            Duljina = 5.0,
            MaxBrojTrkaca = 100,
            Startnina = 25.00m,
            Početak = new DateOnly(2025, 6, 1),
            UtrkaId = utrkaId,
            TipId = tipId
        };

    public static UtrkaFormVM MakeUtrkaFormVM(int utrkaId = 0)
    {
        var futureDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30));

        return new UtrkaFormVM
        {
            UtrkaId = utrkaId,
            Naziv = "Nova Utrka",
            Datum = futureDate,
            Grad = "Zagreb",
            Drzava = "Hrvatska",
            OrganizatorId = 1,
            StatusId = 1,
            Kategorije = new List<KategorijaRowVM>
        {
            new KategorijaRowVM
            {
                KategorijaId = 0,
                Naziv = "5km",
                Duljina = 5.0,
                MaxBrojTrkaca = 100,
                Startnina = 25m,
                Pocetak = futureDate,
                TipId = 1,
                IsDeleted = false
            }
        }
        };
    }

    public static MasterContext MakeInMemoryContext(string dbName)
    {
        var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<MasterContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var ctx = new MasterContext(options);

        ctx.Vrstaorganizators.Add(new Vrstaorganizator { VrstaOrganizatoraId = 1, Naziv = "Udruga" });
        ctx.Organizators.Add(MakeOrganizator());
        ctx.Statusutrkes.Add(MakeStatus());
        ctx.Tipkategorijes.Add(MakeTip());
        ctx.SaveChanges();

        return ctx;
    }
}