using Moq;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Repositories.Interfaces;
using SUTUTWebApp.Services;

namespace SUTUTWebApp.Tests.Unit.Services
{
    public class StatusutrkeServiceTests
    {
       
        private readonly Mock<IStatusutrkeRepository> _repoMock = new();
        private StatusutrkeService MakeService() => new(_repoMock.Object);


        [Fact]
        public async Task GetAllAsync_DelegatesToRepository()
        {
            var ocekivano = new List<Statusutrke> { new() { StatusId = 1, Naziv = "Test" } };
            _repoMock.Setup(r => r.GetAllAsync("test"))
                     .ReturnsAsync(ocekivano);
            var service = MakeService();

            var result = await service.GetAllAsync("test");

            Assert.Single(result);
            _repoMock.Verify(r => r.GetAllAsync("test"), Times.Once);
        }


        [Fact]
        public async Task GetByIdAsync_ReturnsStatusWhenFound()
        {
            var status = new Statusutrke { StatusId = 3, Naziv = "Aktivan" };
            _repoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(status);
            var service = MakeService();

            var result = await service.GetByIdAsync(3);

            Assert.NotNull(result);
            Assert.Equal("Aktivan", result.Naziv);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNullWhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Statusutrke?)null);
            var service = MakeService();

            var result = await service.GetByIdAsync(99);

            Assert.Null(result);
        }


        [Fact]
        public async Task CreateAsync_CallsAddAndSaveChanges()
        {
            var service = MakeService();
            var noviStatus = new Statusutrke { Naziv = "Novi" };

            await service.CreateAsync(noviStatus);

            _repoMock.Verify(r => r.AddAsync(noviStatus), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenIdMismatch()
        {
            // id 1, ali objekt ima id 99 -> ne smije se azurirati
            var service = MakeService();
            var status = new Statusutrke { StatusId = 99, Naziv = "Test" };

            var result = await service.UpdateAsync(1, status);

            Assert.False(result);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Statusutrke>()), Times.Never);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrueAndCallsRepoWhenIdMatches()
        {
            var service = MakeService();
            var status = new Statusutrke { StatusId = 1, Naziv = "Izmijenjeno" };

            var result = await service.UpdateAsync(1, status);

            Assert.True(result);
            _repoMock.Verify(r => r.UpdateAsync(status), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task DeleteAsync_ReturnsFalseWhenStatusNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((Statusutrke?)null);
            var service = MakeService();

            var result = await service.DeleteAsync(5);

            Assert.False(result);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Statusutrke>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrueAndDeletesStatusWhenFound()
        {
            var status = new Statusutrke { StatusId = 5, Naziv = "Za brisanje" };
            _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(status);
            var service = MakeService();

            var result = await service.DeleteAsync(5);

            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(status), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public void Exists_DelegatesToRepository()
        {
            _repoMock.Setup(r => r.Exists(7)).Returns(true);
            var service = MakeService();

            var result = service.Exists(7);

            Assert.True(result);
            _repoMock.Verify(r => r.Exists(7), Times.Once);
        }
    }
}
