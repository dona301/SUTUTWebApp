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
        public async Task CreateAsync_CallsAddAndSaveChanges()
        {
            var service = MakeService();
            var noviStatus = new Statusutrke { Naziv = "Novi" };

            await service.CreateAsync(noviStatus);

            _repoMock.Verify(r => r.AddAsync(noviStatus), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_ReturnsFalseWhenIdMismatch()
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
    }
}
