using Microsoft.AspNetCore.Mvc;
using Moq;
using SUTUTWebApp.Controllers;
using SUTUTWebApp.Exceptions;
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
        public async Task Create_Post_ReturnsFormViewWhenKategorijaDatumPrijeDatumaUtrke()
        {
            _serviceMock.Setup(s => s.PopulateDropdownsAsync(It.IsAny<UtrkaFormVM>()))
                        .Returns(Task.CompletedTask);
            _serviceMock.Setup(s => s.ValidateBusiness(It.IsAny<UtrkaFormVM>()))
                        .Throws(new BusinessValidationException("Datum kategorije ne može biti prije datuma utrke."));

            var controller = MakeController();
            var vm = TestDataFactory.MakeUtrkaFormVM();

            var result = await controller.Create(vm);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal("Form", view.ViewName);
            Assert.False(controller.ModelState.IsValid);
            _serviceMock.Verify(s => s.CreateAsync(It.IsAny<UtrkaFormVM>()), Times.Never);
        }

        [Fact]
        public async Task Create_Post_ReturnsFormViewWhenPostojiDuplikatKategorija()
        {
            _serviceMock.Setup(s => s.PopulateDropdownsAsync(It.IsAny<UtrkaFormVM>()))
                        .Returns(Task.CompletedTask);
            _serviceMock.Setup(s => s.ValidateBusiness(It.IsAny<UtrkaFormVM>()))
                        .Throws(new BusinessValidationException("Postoje dvije kategorije iste duljine i istog tipa."));

            var controller = MakeController();
            var vm = TestDataFactory.MakeUtrkaFormVM();

            // druga kategorija s istom duljinom i tipom kao prva
            vm.Datum = new DateOnly(2025, 9, 1);
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
            _serviceMock.Setup(s => s.ValidateBusiness(It.IsAny<UtrkaFormVM>()))
                        // no .Throws() = passes silently
                        .Verifiable();

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
        public async Task Edit_Post_ReturnsFormViewWhenValidacijaNeProlazi()
        {
            _serviceMock.Setup(s => s.PopulateDropdownsAsync(It.IsAny<UtrkaFormVM>()))
                        .Returns(Task.CompletedTask);
            _serviceMock.Setup(s => s.ValidateBusiness(It.IsAny<UtrkaFormVM>()))
                        .Throws(new BusinessValidationException("Nevažeći datum."));

            var controller = MakeController();
            var vm = TestDataFactory.MakeUtrkaFormVM(utrkaId: 1);

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