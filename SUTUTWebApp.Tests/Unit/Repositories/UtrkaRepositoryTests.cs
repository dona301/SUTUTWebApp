using Microsoft.EntityFrameworkCore;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Repositories;

namespace SUTUTWebApp.Tests.Unit.Repositories
{

    public class UtrkaRepositoryTests
    {
        private (UtrkaRepository repo, MasterContext ctx) Make(string dbName)
        {
            var ctx = TestDataFactory.MakeInMemoryContext(dbName);
            return (new UtrkaRepository(ctx), ctx);
        }

        [Fact]
        public async Task GetAllAsync_FiltersByNaziv()
        {
            var (repo, ctx) = Make(nameof(GetAllAsync_FiltersByNaziv));
            var u1 = TestDataFactory.MakeUtrka(id: 0); u1.Naziv = "Zagreb Maraton";
            var u2 = TestDataFactory.MakeUtrka(id: 0); u2.Naziv = "Dubrovnik Trail";
            ctx.Utrkas.AddRange(u1, u2);
            await ctx.SaveChangesAsync();

            var result = await repo.GetAllAsync("Maraton");

            Assert.Single(result);
            Assert.Equal("Zagreb Maraton", result.First().Naziv);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNullWhenNotExists()
        {
            var (repo, _) = Make(nameof(GetByIdAsync_ReturnsNullWhenNotExists));

            var result = await repo.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_IncludesAllRelatedData()
        {
            var (repo, ctx) = Make(nameof(GetByIdWithDetailsAsync_IncludesAllRelatedData));
            var utrka = TestDataFactory.MakeUtrka(id: 0);
            utrka.Kategorijas.Add(TestDataFactory.MakeKategorija(id: 0, utrkaId: 0));
            ctx.Utrkas.Add(utrka);
            await ctx.SaveChangesAsync();
            var savedId = ctx.Utrkas.First().UtrkaId;

            var result = await repo.GetByIdWithDetailsAsync(savedId);

            Assert.NotNull(result);
            Assert.NotNull(result.Organizator);
            Assert.NotNull(result.Status);
            Assert.Single(result.Kategorijas);
            Assert.NotNull(result.Kategorijas.First().Tip);
        }


        [Fact]
        public async Task AddAsync_SavesUtrkaToDatabase()
        {
            var (repo, ctx) = Make(nameof(AddAsync_SavesUtrkaToDatabase));
            var utrka = TestDataFactory.MakeUtrka(id: 0);

            await repo.AddAsync(utrka);
            await repo.SaveChangesAsync();

            Assert.Equal(1, ctx.Utrkas.Count());
            Assert.Equal("Test Utrka", ctx.Utrkas.First().Naziv);
        }

        [Fact]
        public async Task DeleteAsync_RemovesUtrkaAndKategorijas()
        {
            var (repo, ctx) = Make(nameof(DeleteAsync_RemovesUtrkaAndKategorijas));
            var utrka = TestDataFactory.MakeUtrka(id: 0);
            utrka.Kategorijas.Add(TestDataFactory.MakeKategorija(id: 0, utrkaId: 0));
            ctx.Utrkas.Add(utrka);
            await ctx.SaveChangesAsync();

            var saved = await ctx.Utrkas.Include(u => u.Kategorijas).FirstAsync();
            await repo.DeleteAsync(saved);
            await repo.SaveChangesAsync();

            Assert.Equal(0, ctx.Utrkas.Count());
            Assert.Equal(0, ctx.Kategorijas.Count());
        }


        [Fact]
        public async Task RemoveKategorija_RemovesOnlyThatKategorija()
        {
            var (repo, ctx) = Make(nameof(RemoveKategorija_RemovesOnlyThatKategorija));
            var utrka = TestDataFactory.MakeUtrka(id: 0);
            utrka.Kategorijas.Add(TestDataFactory.MakeKategorija(id: 0, utrkaId: 0));
            utrka.Kategorijas.Add(TestDataFactory.MakeKategorija(id: 0, utrkaId: 0));
            ctx.Utrkas.Add(utrka);
            await ctx.SaveChangesAsync();

            var prvaKategorija = ctx.Kategorijas.First();
            repo.RemoveKategorija(prvaKategorija);
            await repo.SaveChangesAsync();

            Assert.Equal(1, ctx.Kategorijas.Count());
        }

        [Fact]
        public async Task GetStatusiSelectAsync_ReturnsSelectListItems()
        {
            var (repo, _) = Make(nameof(GetStatusiSelectAsync_ReturnsSelectListItems));

            var result = await repo.GetStatusiSelectAsync();

            Assert.Single(result);
        }
    }
}