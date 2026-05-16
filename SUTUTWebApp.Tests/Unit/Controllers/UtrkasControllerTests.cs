using Microsoft.AspNetCore.Mvc;
using Moq;
using SUTUTWebApp.Controllers;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Models.ViewModels;
using SUTUTWebApp.Services.Interfaces;

namespace SUTUTWebApp.Tests.Unit.Controllers
{
    public class UtrkasControllerTests
    {
        private readonly Mock<IUtrkaService> _serviceMock = new();
        private UtrkasController MakeController() => new(_serviceMock.Object);

        [Fact]
        public async Task Index_ReturnsViewResultWithListOfUtrke()
        {
            var utrke = new List<Utrka> { TestDataFactory.MakeUtrka() };
            _serviceMock.Setup(s => s.GetAllAsync(null)).ReturnsAsync(utrke);
            var controller = MakeController();

            var result = await controller.Index(null!);

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Utrka>>(view.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Index_SetsCurrentFilterInViewData()
        {
            _serviceMock.Setup(s => s.GetAllAsync("Zagreb")).ReturnsAsync(new List<Utrka>());
            var controller = MakeController();

            await controller.Index("Zagreb");

            Assert.Equal("Zagreb", controller.ViewData["CurrentFilter"]);
        }

        [Fact]
        public async Task Details_ReturnsNotFoundWhenServiceReturnsNull()
        {
            _serviceMock.Setup(s => s.GetByIdWithDetailsAsync(99)).ReturnsAsync((Utrka?)null);
            var controller = MakeController();

            var result = await controller.Details(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResultWithUtrka()
        {
            var utrka = TestDataFactory.MakeUtrka(id: 1);
            _serviceMock.Setup(s => s.GetByIdWithDetailsAsync(1)).ReturnsAsync(utrka);
            var controller = MakeController();

            var result = await controller.Details(1);

            var view = Assert.IsType<ViewResult>(result);
            Assert.IsType<Utrka>(view.Model);
        }


        [Fact]
        public async Task Create_Get_ReturnsFormViewWithEmptyVm()
        {
            _serviceMock.Setup(s => s.PopulateDropdownsAsync(It.IsAny<UtrkaFormVM>()))
                        .Returns(Task.CompletedTask);
            var controller = MakeController();

            var result = await controller.Create();

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Form", view.ViewName);
            Assert.IsType<UtrkaFormVM>(view.Model);
        }


        [Fact]
        public async Task Create_Post_ReturnsFormViewWhenKategorijaDatumPrijeDatumaUtrke()
        {
            _serviceMock.Setup(s => s.PopulateDropdownsAsync(It.IsAny<UtrkaFormVM>()))
                        .Returns(Task.CompletedTask);
            var controller = MakeController();

            var vm = TestDataFactory.MakeUtrkaFormVM();
            vm.Datum = new DateOnly(2025, 9, 1);
            vm.Kategorije[0].Pocetak = new DateOnly(2025, 8, 1); // prije datuma utrke

            var result = await controller.Create(vm);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Form", view.ViewName);
            Assert.False(controller.ModelState.IsValid);
            // ne smije kreirati utrku
            _serviceMock.Verify(s => s.CreateAsync(It.IsAny<UtrkaFormVM>()), Times.Never);
        }

        [Fact]
        public async Task Create_Post_ReturnsFormViewWhenPostojiDuplikatKategorija()
        {
            _serviceMock.Setup(s => s.PopulateDropdownsAsync(It.IsAny<UtrkaFormVM>()))
                        .Returns(Task.CompletedTask);
            var controller = MakeController();

            var vm = TestDataFactory.MakeUtrkaFormVM();
            vm.Datum = new DateOnly(2025, 9, 1);

            // druga kategorija s istom duljinom i tipom kao prva
            vm.Kategorije.Add(new KategorijaRowVM
            {
                KategorijaId = 0,
                Naziv = "Duplikat",
                Duljina = vm.Kategorije[0].Duljina,  
                TipId = vm.Kategorije[0].TipId,       
                MaxBrojTrkaca = 50,
                Startnina = 20m,
                Pocetak = vm.Datum,
                IsDeleted = false
            });

            var result = await controller.Create(vm);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Form", view.ViewName);
            Assert.False(controller.ModelState.IsValid);
            _serviceMock.Verify(s => s.CreateAsync(It.IsAny<UtrkaFormVM>()), Times.Never);
        }

        [Fact]
        public async Task Create_Post_IgnoriraDeletedKategorijePriValidacijiDuplikata()
        {
            var controller = MakeController();
            var vm = TestDataFactory.MakeUtrkaFormVM();
            vm.Datum = new DateOnly(2025, 9, 1);
            vm.Kategorije[0].Pocetak = vm.Datum;
            vm.Kategorije[0].IsDeleted = true; // ova se ignorira

            vm.Kategorije.Add(new KategorijaRowVM
            {
                Naziv = "Aktivna",
                Duljina = vm.Kategorije[0].Duljina,
                TipId = vm.Kategorije[0].TipId,
                MaxBrojTrkaca = 50,
                Startnina = 20m,
                Pocetak = vm.Datum,
                IsDeleted = false
            });

            var result = await controller.Create(vm);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Create_Post_RedirectsToIndexWhenValid()
        {
            var controller = MakeController();
            var vm = TestDataFactory.MakeUtrkaFormVM();
            vm.Datum = new DateOnly(2025, 9, 1);
            vm.Kategorije[0].Pocetak = vm.Datum; // datum kategorije = datum utrke

            var result = await controller.Create(vm);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            _serviceMock.Verify(s => s.CreateAsync(vm), Times.Once);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFoundWhenServiceReturnsNull()
        {
            _serviceMock.Setup(s => s.GetFormVmForEditAsync(5)).ReturnsAsync((UtrkaFormVM?)null);
            var controller = MakeController();

            var result = await controller.Edit(5);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_ReturnsFormViewWithPopulatedVm()
        {
            var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: 5);
            _serviceMock.Setup(s => s.GetFormVmForEditAsync(5)).ReturnsAsync(vm);
            var controller = MakeController();

            var result = await controller.Edit(5);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Form", view.ViewName);
            var model = Assert.IsType<UtrkaFormVM>(view.Model);
            Assert.Equal(5, model.UtrkaId);
        }


        [Fact]
        public async Task Edit_Post_ReturnsFormViewWhenValidacijaNeProlazi()
        {
            _serviceMock.Setup(s => s.PopulateDropdownsAsync(It.IsAny<UtrkaFormVM>()))
                        .Returns(Task.CompletedTask);
            var controller = MakeController();

            var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: 1);
            vm.Datum = new DateOnly(2025, 9, 1);
            vm.Kategorije[0].Pocetak = new DateOnly(2025, 7, 1); // nevazeci datum

            var result = await controller.Edit(vm);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Form", view.ViewName);
            _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<UtrkaFormVM>()), Times.Never);
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFoundWhenUpdateReturnsFalse()
        {
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<UtrkaFormVM>())).ReturnsAsync(false);
            var controller = MakeController();

            var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: 99);
            vm.Datum = new DateOnly(2025, 9, 1);
            vm.Kategorije[0].Pocetak = vm.Datum;

            var result = await controller.Edit(vm);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_RedirectsToIndexWhenValid()
        {
            _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<UtrkaFormVM>())).ReturnsAsync(true);
            var controller = MakeController();

            var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: 1);
            vm.Datum = new DateOnly(2025, 9, 1);
            vm.Kategorije[0].Pocetak = vm.Datum;

            var result = await controller.Edit(vm);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }


        [Fact]
        public async Task Delete_Get_ReturnsNotFoundWhenServiceReturnsNull()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(3)).ReturnsAsync((Utrka?)null);
            var controller = MakeController();

            var result = await controller.Delete(3);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsViewResultWithUtrka()
        {
            var utrka = TestDataFactory.MakeUtrka(id: 3);
            _serviceMock.Setup(s => s.GetByIdAsync(3)).ReturnsAsync(utrka);
            var controller = MakeController();

            var result = await controller.Delete(3);

            var view = Assert.IsType<ViewResult>(result);
            Assert.IsType<Utrka>(view.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_RedirectsToIndexAndCallsDelete()
        {
            var controller = MakeController();

            var result = await controller.DeleteConfirmed(1);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}