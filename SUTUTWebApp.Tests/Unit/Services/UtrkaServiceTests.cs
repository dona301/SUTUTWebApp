using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Models.ViewModels;
using SUTUTWebApp.Repositories.Interfaces;
using SUTUTWebApp.Services;

namespace SUTUTWebApp.Tests.Unit.Services
{
    public class UtrkaServiceTests
    {
        private readonly Mock<IUtrkaRepository> _repoMock = new();
        private UtrkaService MakeService() => new(_repoMock.Object);

        [Fact]
        public async Task GetAllAsync_DelegatesToRepository()
        {
            var utrke = new List<Utrka> { TestDataFactory.MakeUtrka() };
            _repoMock.Setup(r => r.GetAllAsync("Zagreb")).ReturnsAsync(utrke);
            var service = MakeService();

            var result = await service.GetAllAsync("Zagreb");

            Assert.Single(result);
            _repoMock.Verify(r => r.GetAllAsync("Zagreb"), Times.Once);
        }

        [Fact]
        public async Task GetByIdWithDetailsAsync_ReturnsNullWhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdWithDetailsAsync(99))
                     .ReturnsAsync((Utrka?)null);
            var service = MakeService();

            var result = await service.GetByIdWithDetailsAsync(99);

            Assert.Null(result);
        }


        [Fact]
        public async Task PopulateDropdownsAsync_FillsAll3ListsOnViewModel()
        {
            _repoMock.Setup(r => r.GetOrganizatoriSelectAsync())
                     .ReturnsAsync(new List<SelectListItem> { new() { Value = "1", Text = "Org" } });
            _repoMock.Setup(r => r.GetStatusiSelectAsync())
                     .ReturnsAsync(new List<SelectListItem> { new() { Value = "1", Text = "Status" } });
            _repoMock.Setup(r => r.GetTipoviKategorijeSelectAsync())
                     .ReturnsAsync(new List<SelectListItem> { new() { Value = "1", Text = "Tip" } });

            var service = MakeService();
            var vm = new UtrkaFormVM();

            await service.PopulateDropdownsAsync(vm);

            Assert.Single(vm.Organizatori);
            Assert.Single(vm.Statusi);
            Assert.Single(vm.TipoviKategorije);
        }


        [Fact]
        public async Task GetFormVmForEditAsync_ReturnsNullWhenUtrkaNotFound()
        {
            _repoMock.Setup(r => r.GetByIdWithKategorijasAsync(5))
                     .ReturnsAsync((Utrka?)null);
            var service = MakeService();

            var result = await service.GetFormVmForEditAsync(5);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetFormVmForEditAsync_MapsUtrkaToViewModelCorrectly()
        {
            var kategorija = TestDataFactory.MakeKategorija(id: 7, utrkaId: 1);
            var utrka = TestDataFactory.MakeUtrka(id: 1);
            utrka.Kategorijas.Add(kategorija);

            _repoMock.Setup(r => r.GetByIdWithKategorijasAsync(1)).ReturnsAsync(utrka);
            _repoMock.Setup(r => r.GetOrganizatoriSelectAsync()).ReturnsAsync(new List<SelectListItem>());
            _repoMock.Setup(r => r.GetStatusiSelectAsync()).ReturnsAsync(new List<SelectListItem>());
            _repoMock.Setup(r => r.GetTipoviKategorijeSelectAsync()).ReturnsAsync(new List<SelectListItem>());

            var service = MakeService();

            var vm = await service.GetFormVmForEditAsync(1);

            Assert.NotNull(vm);
            Assert.Equal(1, vm.UtrkaId);
            Assert.Equal(utrka.Naziv, vm.Naziv);
            Assert.Equal(utrka.Datum, vm.Datum);
            Assert.Equal(utrka.Grad, vm.Grad);
            Assert.Equal(utrka.Drzava, vm.Drzava);
            Assert.Equal(utrka.OrganizatorId, vm.OrganizatorId);
            Assert.Equal(utrka.StatusId, vm.StatusId);

           
            Assert.Single(vm.Kategorije);
            Assert.Equal(7, vm.Kategorije[0].KategorijaId);
            Assert.Equal(kategorija.Naziv, vm.Kategorije[0].Naziv);
            Assert.Equal(kategorija.Duljina, vm.Kategorije[0].Duljina);
        }


        [Fact]
        public async Task CreateAsync_CallsAddAndSaveWithMappedUtrka()
        {
            var service = MakeService();
            var vm = TestDataFactory.MakeUtrkaFormVM();

            await service.CreateAsync(vm);

            // provjer ima li utrka ispravne atribute
            _repoMock.Verify(r => r.AddAsync(It.Is<Utrka>(u =>
                u.Naziv == vm.Naziv &&
                u.Grad == vm.Grad &&
                u.Drzava == vm.Drzava &&
                u.OrganizatorId == vm.OrganizatorId
            )), Times.Once);

            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateAsync_IgnoresDeletedKategorije()
        {
            var service = MakeService();
            var vm = TestDataFactory.MakeUtrkaFormVM();

            vm.Kategorije[0].IsDeleted = true;

            Utrka? snimljenaUtrka = null;
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Utrka>()))
                     .Callback<Utrka>(u => snimljenaUtrka = u)
                     .Returns(Task.CompletedTask);

            await service.CreateAsync(vm);

            Assert.NotNull(snimljenaUtrka);
            Assert.Empty(snimljenaUtrka.Kategorijas);
        }


        [Fact]
        public async Task UpdateAsync_ReturnsFalseWhenUtrkaNotFound()
        {
            _repoMock.Setup(r => r.GetByIdWithKategorijasAsync(It.IsAny<int>()))
                     .ReturnsAsync((Utrka?)null);
            var service = MakeService();

            var result = await service.UpdateAsync(TestDataFactory.MakeUtrkaFormVM(utrkaId: 99));

            Assert.False(result);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsTrueAndUpdatesFields()
        {
            var utrka = TestDataFactory.MakeUtrka(id: 1);
            _repoMock.Setup(r => r.GetByIdWithKategorijasAsync(1)).ReturnsAsync(utrka);
            var service = MakeService();

            var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: 1);
            vm.Naziv = "Novo ime";
            vm.Kategorije.Clear();

            var result = await service.UpdateAsync(vm);

            Assert.True(result);
            Assert.Equal("Novo ime", utrka.Naziv);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_AddsNewKategorijaWhenKategorijaIdIsZero()
        {
            var utrka = TestDataFactory.MakeUtrka(id: 1);
            _repoMock.Setup(r => r.GetByIdWithKategorijasAsync(1)).ReturnsAsync(utrka);
            var service = MakeService();

            var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: 1);
            vm.Kategorije[0].KategorijaId = 0;
            vm.Kategorije[0].IsDeleted = false;

            await service.UpdateAsync(vm);

            Assert.Single(utrka.Kategorijas);
        }

        [Fact]
        public async Task UpdateAsync_RemovesKategorijaWhenIsDeletedIsTrue()
        {
            var utrka = TestDataFactory.MakeUtrka(id: 1);
            var kat = TestDataFactory.MakeKategorija(id: 10, utrkaId: 1);
            utrka.Kategorijas.Add(kat);
            _repoMock.Setup(r => r.GetByIdWithKategorijasAsync(1)).ReturnsAsync(utrka);
            var service = MakeService();

            var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: 1);
            vm.Kategorije[0].KategorijaId = 10; // postojeca
            vm.Kategorije[0].IsDeleted = true;  // za brisanje

            await service.UpdateAsync(vm);

            _repoMock.Verify(r => r.RemoveKategorija(kat), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingKategorijaWhenNotDeleted()
        {
            var utrka = TestDataFactory.MakeUtrka(id: 1);
            var kat = TestDataFactory.MakeKategorija(id: 10, utrkaId: 1);
            utrka.Kategorijas.Add(kat);
            _repoMock.Setup(r => r.GetByIdWithKategorijasAsync(1)).ReturnsAsync(utrka);
            var service = MakeService();

            var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: 1);
            vm.Kategorije[0].KategorijaId = 10;
            vm.Kategorije[0].IsDeleted = false;
            vm.Kategorije[0].Naziv = "Novo ime kategorije";

            await service.UpdateAsync(vm);

            Assert.Equal("Novo ime kategorije", kat.Naziv);
            _repoMock.Verify(r => r.RemoveKategorija(It.IsAny<Kategorija>()), Times.Never);
        }


        [Fact]
        public async Task DeleteAsync_ReturnsFalseWhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdWithKategorijasAsync(5))
                     .ReturnsAsync((Utrka?)null);
            var service = MakeService();

            var result = await service.DeleteAsync(5);

            Assert.False(result);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Utrka>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrueAndCallsDeleteWhenFound()
        {
            var utrka = TestDataFactory.MakeUtrka(id: 5);
            _repoMock.Setup(r => r.GetByIdWithKategorijasAsync(5)).ReturnsAsync(utrka);
            var service = MakeService();

            var result = await service.DeleteAsync(5);

            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(utrka), Times.Once);
            _repoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
