using Microsoft.AspNetCore.Mvc;
using Moq;
using SUTUTWebApp.Controllers;
using SUTUTWebApp.Exceptions;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Services.Interfaces;

namespace SUTUTWebApp.Tests.Unit.Controllers
{
    public class StatusutrkesControllerTests
    {
        private readonly Mock<IStatusutrkeService> _serviceMock = new();
        private StatusutrkesController MakeController() => new(_serviceMock.Object);

        [Fact]
        public async Task Details_ReturnsNotFoundWhenIdIsNull()
        {
            var controller = MakeController();

            var result = await controller.Details(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFoundWhenServiceReturnsNull()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(5)).ReturnsAsync((Statusutrke?)null);
            var controller = MakeController();

            var result = await controller.Details(5);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_ReturnsViewWhenModelStateInvalid()
        {
            var controller = MakeController();
            controller.ModelState.AddModelError("Naziv", "Naziv je obavezan");
            var status = new Statusutrke();

            var result = await controller.Create(status);

            Assert.IsType<ViewResult>(result);
            _serviceMock.Verify(s => s.CreateAsync(It.IsAny<Statusutrke>()), Times.Never);
        }

        [Fact]
        public async Task Create_Post_RedirectsToIndexWhenModelStateValid()
        {
            var controller = MakeController();
            var status = new Statusutrke { Naziv = "Novi status" };

            var result = await controller.Create(status);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            _serviceMock.Verify(s => s.CreateAsync(status), Times.Once);
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFoundWhenServiceReturnsFalse()
        {
            _serviceMock.Setup(s => s.UpdateAsync(1, It.IsAny<Statusutrke>())).ReturnsAsync(false);
            var controller = MakeController();
            var status = new Statusutrke { StatusId = 1, Naziv = "Test" };

            var result = await controller.Edit(1, status);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_RedirectsToIndexWhenSuccessful()
        {
            _serviceMock.Setup(s => s.UpdateAsync(1, It.IsAny<Statusutrke>())).ReturnsAsync(true);
            var controller = MakeController();
            var status = new Statusutrke { StatusId = 1, Naziv = "Azurirano" };

            var result = await controller.Edit(1, status);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_RedirectsToIndex()
        {
            var controller = MakeController();

            var result = await controller.DeleteConfirmed(1);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task Create_Post_ReturnsViewWhenNazivAlreadyExists()
        {
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<Statusutrke>()))
                        .ThrowsAsync(new BusinessValidationException("Status s tim nazivom već postoji."));
            var controller = MakeController();
            var status = new Statusutrke { Naziv = "Postojeci" };

            var result = await controller.Create(status);

            var view = Assert.IsType<ViewResult>(result);
            Assert.False(controller.ModelState.IsValid);
            _serviceMock.Verify(s => s.CreateAsync(It.IsAny<Statusutrke>()), Times.Once);
        }

        [Fact]
        public async Task Edit_Post_ReturnsViewWhenNazivAlreadyExists()
        {
            _serviceMock.Setup(s => s.UpdateAsync(1, It.IsAny<Statusutrke>()))
                        .ThrowsAsync(new BusinessValidationException("Status s tim nazivom već postoji."));
            var controller = MakeController();
            var status = new Statusutrke { StatusId = 1, Naziv = "Postojeci" };

            var result = await controller.Edit(1, status);

            var view = Assert.IsType<ViewResult>(result);
            Assert.False(controller.ModelState.IsValid);
            _serviceMock.Verify(s => s.UpdateAsync(1, It.IsAny<Statusutrke>()), Times.Once);
        }
    }
}
