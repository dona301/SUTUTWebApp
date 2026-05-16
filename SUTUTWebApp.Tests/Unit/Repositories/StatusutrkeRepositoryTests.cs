using Microsoft.EntityFrameworkCore;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Repositories;
using Xunit;

namespace SUTUTWebApp.Tests.Unit.Repositories
{
    public class StatusutrkeRepositoryTests
    {
        private MasterContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<MasterContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new MasterContext(options);
        }

        [Fact]
        public async Task GetAllAsync_WithoutSearchString_ReturnsAllStatuses()
        {
            var context = GetInMemoryContext(Guid.NewGuid().ToString());
            context.Statusutrkes.Add(new Statusutrke { StatusId = 1, Naziv = "U tijeku" });
            context.Statusutrkes.Add(new Statusutrke { StatusId = 2, Naziv = "Završena" });
            await context.SaveChangesAsync();

            var repository = new StatusutrkeRepository(context);
            
            var rezultat = await repository.GetAllAsync(null);

            Assert.NotNull(rezultat);
            Assert.Equal(2, rezultat.Count());
        }

        [Fact]
        public async Task GetAllAsync_WithSearchString_ReturnsFilteredStatuses()
        {
            var context = GetInMemoryContext(Guid.NewGuid().ToString());
            context.Statusutrkes.Add(new Statusutrke { StatusId = 1, Naziv = "U tijeku" });
            context.Statusutrkes.Add(new Statusutrke { StatusId = 2, Naziv = "Završena" });
            await context.SaveChangesAsync();

            var repository = new StatusutrkeRepository(context);

            var rezultat = await repository.GetAllAsync("tijeku");

            Assert.Single(rezultat);
            Assert.Equal("U tijeku", rezultat.First().Naziv);
        }

        [Fact]
        public async Task AddAsync_ISaveChangesAsync_SavesStatusToDb()
        {
            var context = GetInMemoryContext(Guid.NewGuid().ToString());
            var repository = new StatusutrkeRepository(context);

            var newStatus = new Statusutrke { StatusId = 3, Naziv = "Otkazano" };

            await repository.AddAsync(newStatus);
            await repository.SaveChangesAsync();

            var savedStatus = await context.Statusutrkes.FirstOrDefaultAsync(s => s.StatusId == 3);
            Assert.NotNull(savedStatus);
            Assert.Equal("Otkazano", savedStatus.Naziv);
        }

        [Fact]
        public async Task Exists_WhenStatusExists_ReturnsTrue()
        {
            var context = GetInMemoryContext(Guid.NewGuid().ToString());
            context.Statusutrkes.Add(new Statusutrke { StatusId = 10, Naziv = "Testni status" });
            await context.SaveChangesAsync();

            var repository = new StatusutrkeRepository(context);

            var exists = repository.Exists(10);
            var doesntExist = repository.Exists(99);

            Assert.True(exists);
            Assert.False(doesntExist);
        }

        [Fact]
        public async Task GetByIdAsync_WhenExists_ReturnsCorrectStatus()
        {
            var context = GetInMemoryContext(Guid.NewGuid().ToString());
            context.Statusutrkes.Add(new Statusutrke { StatusId = 5, Naziv = "Test" });
            await context.SaveChangesAsync();
            var repository = new StatusutrkeRepository(context);

            var result = await repository.GetByIdAsync(5);

            Assert.NotNull(result);
            Assert.Equal("Test", result.Naziv);
        }

        [Fact]
        public async Task GetByIdAsync_WhenNotExists_ReturnsNull()
        {
            var context = GetInMemoryContext(Guid.NewGuid().ToString());
            var repository = new StatusutrkeRepository(context);

            var result = await repository.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ChangesValues_InDatabase()
        {
            var context = GetInMemoryContext(Guid.NewGuid().ToString());
            context.Statusutrkes.Add(new Statusutrke { StatusId = 1, Naziv = "Stari status" });
            await context.SaveChangesAsync();
            var repository = new StatusutrkeRepository(context);

            await repository.UpdateAsync(new Statusutrke { StatusId = 1, Naziv = "Novi status" });
            await repository.SaveChangesAsync();

            var result = await context.Statusutrkes.FindAsync(1);
            Assert.Equal("Novi status", result!.Naziv);
        }

        [Fact]
        public async Task DeleteAsync_RemovesStatus_FromDatabase()
        {
            var context = GetInMemoryContext(Guid.NewGuid().ToString());
            var status = new Statusutrke { StatusId = 1, Naziv = "Za brisanje" };
            context.Statusutrkes.Add(status);
            await context.SaveChangesAsync();
            var repository = new StatusutrkeRepository(context);

            await repository.DeleteAsync(status);
            await repository.SaveChangesAsync();

            Assert.Equal(0, context.Statusutrkes.Count());
        }
    }
}
