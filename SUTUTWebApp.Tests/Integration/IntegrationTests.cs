using Microsoft.AspNetCore.Mvc;
using SUTUTWebApp.Controllers;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Models.ViewModels;
using SUTUTWebApp.Repositories;
using SUTUTWebApp.Services;

namespace SUTUTWebApp.Tests.Integration;

public class UtrkaIntegrationTests
{
    private static (UtrkasController controller, MasterContext ctx) BuildStack()
    {
        var ctx = TestDataFactory.MakeInMemoryContext(Guid.NewGuid().ToString());
        var repo = new UtrkaRepository(ctx);
        var service = new UtrkaService(repo);
        var controller = new UtrkasController(service);
        return (controller, ctx);
    }

    [Fact]
    public async Task Index_ReturnsUtrke_FromRealDatabase()
    {
        var (controller, ctx) = BuildStack();
        ctx.Utrkas.Add(TestDataFactory.MakeUtrka(id: 0));
        await ctx.SaveChangesAsync();

        var result = await controller.Index(null!);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Utrka>>(view.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Index_FiltersCorrectly_ThroughAllLayers()
    {
        var (controller, ctx) = BuildStack();
        var u1 = TestDataFactory.MakeUtrka(id: 0);
        u1.Naziv = "Zagreb Marathon";
        u1.Grad = "Zagreb";

        var u2 = TestDataFactory.MakeUtrka(id: 0);
        u2.Naziv = "Dubrovnik Trail";
        u2.Grad = "Dubrovnik";

        ctx.Utrkas.AddRange(u1, u2);
        await ctx.SaveChangesAsync();

        var result = await controller.Index("Marathon");

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Utrka>>(view.Model);
        Assert.Single(model);
        Assert.Equal("Zagreb Marathon", model.First().Naziv);
    }

    [Fact]
    public async Task Create_Post_PersistsUtrkaAndKategorija_ToDatabase()
    {
        var (controller, ctx) = BuildStack();
        var vm = TestDataFactory.MakeUtrkaFormVM();

        var result = await controller.Create(vm);

        Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal(1, ctx.Utrkas.Count());
        Assert.Equal(1, ctx.Kategorijas.Count());
        Assert.Equal("Nova Utrka", ctx.Utrkas.First().Naziv);
    }

    [Fact]
    public async Task Create_Post_DoesNotPersist_WhenBusinessValidationFails()
    {
        var (controller, ctx) = BuildStack();
        var vm = TestDataFactory.MakeUtrkaFormVM();

        vm.Datum = new DateOnly(2025, 9, 1);
        vm.Kategorije[0].Pocetak = new DateOnly(2025, 8, 1);

        var result = await controller.Create(vm);

        var view = Assert.IsType<ViewResult>(result);

        Assert.Equal("Form", view.ViewName);
        Assert.Equal(0, ctx.Utrkas.Count());
    }

    [Fact]
    public async Task Edit_Get_ReturnsPopulatedVm_WithExistingData()
    {
        var (controller, ctx) = BuildStack();
        var utrka = TestDataFactory.MakeUtrka(id: 0);
        utrka.Kategorijas.Add(TestDataFactory.MakeKategorija(id: 0, utrkaId: 0));
        ctx.Utrkas.Add(utrka);
        await ctx.SaveChangesAsync();

        var savedId = ctx.Utrkas.First().UtrkaId;
        var result = await controller.Edit(savedId);

        var view = Assert.IsType<ViewResult>(result);
        var vm = Assert.IsType<UtrkaFormVM>(view.Model);
        Assert.Equal(savedId, vm.UtrkaId);
        Assert.Single(vm.Kategorije);
    }

    [Fact]
    public async Task Edit_Post_UpdatesUtrka_InDatabase()
    {
        var (controller, ctx) = BuildStack();
        var utrka = TestDataFactory.MakeUtrka(id: 0);
        ctx.Utrkas.Add(utrka);
        await ctx.SaveChangesAsync();

        var savedId = ctx.Utrkas.First().UtrkaId;
        var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: savedId);
        vm.Naziv = "Ažurirana Utrka";

        var result = await controller.Edit(vm);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Ažurirana Utrka", ctx.Utrkas.First().Naziv);
    }

    [Fact]
    public async Task Edit_Post_AddsAndRemovesKategorijas_Correctly()
    {
        var (controller, ctx) = BuildStack();
        var utrka = TestDataFactory.MakeUtrka(id: 0);
        var kat = TestDataFactory.MakeKategorija(id: 0, utrkaId: 0);
        utrka.Kategorijas.Add(kat);
        ctx.Utrkas.Add(utrka);
        await ctx.SaveChangesAsync();

        var savedUtrkaId = ctx.Utrkas.First().UtrkaId;
        var savedKatId = ctx.Kategorijas.First().KategorijaId;

        var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: savedUtrkaId);

        vm.Kategorije[0].KategorijaId = savedKatId;
        vm.Kategorije[0].IsDeleted = true;

        vm.Kategorije.Add(new KategorijaRowVM
        {
            KategorijaId = 0,
            Naziv = "Half Marathon",
            Duljina = 21.1,
            MaxBrojTrkaca = 200,
            Startnina = 50m,
            Pocetak = vm.Datum,
            TipId = 1,
            IsDeleted = false
        });

        await controller.Edit(vm);

        Assert.Equal(1, ctx.Kategorijas.Count());
        Assert.Equal("Half Marathon", ctx.Kategorijas.First().Naziv);
    }

    [Fact]
    public async Task Delete_Get_ReturnsView_WithUtrka()
    {
        var (controller, ctx) = BuildStack();
        ctx.Utrkas.Add(TestDataFactory.MakeUtrka(id: 0));
        await ctx.SaveChangesAsync();

        var savedId = ctx.Utrkas.First().UtrkaId;
        var result = await controller.Delete(savedId);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Utrka>(view.Model);
        Assert.Equal(savedId, model.UtrkaId);
    }

    [Fact]
    public async Task DeleteConfirmed_RemovesUtrkaAndKategorijas_FromDatabase()
    {
        var (controller, ctx) = BuildStack();
        var utrka = TestDataFactory.MakeUtrka(id: 0);
        utrka.Kategorijas.Add(TestDataFactory.MakeKategorija(id: 0, utrkaId: 0));
        ctx.Utrkas.Add(utrka);
        await ctx.SaveChangesAsync();

        var savedId = ctx.Utrkas.First().UtrkaId;

        var result = await controller.DeleteConfirmed(savedId);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(0, ctx.Utrkas.Count());
        Assert.Equal(0, ctx.Kategorijas.Count());
    }
}

public class StatusutrkeIntegrationTests
{
    private static (StatusutrkesController controller, MasterContext ctx) BuildStack(string dbName)
    {
        var ctx = TestDataFactory.MakeInMemoryContext(dbName);
        var repo = new StatusutrkeRepository(ctx);
        var service = new StatusutrkeService(repo);
        var controller = new StatusutrkesController(service);
        return (controller, ctx);
    }

    [Fact]
    public async Task Index_ReturnsSeededStatusi()
    {
        var (controller, _) = BuildStack(nameof(Index_ReturnsSeededStatusi));

        var result = await controller.Index(null!);

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Statusutrke>>(view.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Create_Post_PersistsNewStatus()
    {
        var (controller, ctx) = BuildStack(nameof(Create_Post_PersistsNewStatus));
        var newStatus = new Statusutrke { Naziv = "Otkazan" };

        var result = await controller.Create(newStatus);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(2, ctx.Statusutrkes.Count());
        Assert.Contains(ctx.Statusutrkes, s => s.Naziv == "Otkazan");
    }

    [Fact]
    public async Task Edit_Post_UpdatesStatus_InDatabase()
    {
        var (controller, ctx) = BuildStack(nameof(Edit_Post_UpdatesStatus_InDatabase));
        var savedId = ctx.Statusutrkes.First().StatusId;

        var updated = new Statusutrke { StatusId = savedId, Naziv = "Izmijenjen" };
        var result = await controller.Edit(savedId, updated);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Izmijenjen", ctx.Statusutrkes.First().Naziv);
    }

    [Fact]
    public async Task DeleteConfirmed_RemovesStatus_FromDatabase()
    {
        var (controller, ctx) = BuildStack(nameof(DeleteConfirmed_RemovesStatus_FromDatabase));
        var savedId = ctx.Statusutrkes.First().StatusId;

        await controller.DeleteConfirmed(savedId);

        Assert.Equal(0, ctx.Statusutrkes.Count());
    }
}