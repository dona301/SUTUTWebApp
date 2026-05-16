using Microsoft.AspNetCore.Mvc;
using Moq;
using SUTUTWebApp.Controllers;
using SUTUTWebApp.Models.Entities;
using SUTUTWebApp.Services.Interfaces;

namespace SUTUTWebApp.Tests.Unit.Controllers
{
    public class StatusutrkesControllerTests
    {
        private readonly Mock<IStatusutrkeService> _serviceMock = new();
        private StatusutrkesController MakeController() => new(_serviceMock.Object);

        [Fact]
        public async Task Index_ReturnsViewResultWithListOfStatusi()
        {
            var statusi = new List<Statusutrke>
            {
                new() { StatusId = 1, Naziv = "Aktivan" },
                new() { StatusId = 2, Naziv = "Završen" }
            };
            _serviceMock.Setup(s => s.GetAllAsync(null)).ReturnsAsync(statusi);
            var controller = MakeController();

            var result = await controller.Index(null!);

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Statusutrke>>(view.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Index_PassesSearchStringToService()
        {
            _serviceMock.Setup(s => s.GetAllAsync("test")).ReturnsAsync(new List<Statusutrke>());
            var controller = MakeController();

            await controller.Index("test");

            _serviceMock.Verify(s => s.GetAllAsync("test"), Times.Once);
        }

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
        public async Task Details_ReturnsViewResultWithCorrectModel()
        {
            var status = new Statusutrke { StatusId = 5, Naziv = "Test" };
            _serviceMock.Setup(s => s.GetByIdAsync(5)).ReturnsAsync(status);
            var controller = MakeController();

            var result = await controller.Details(5);

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Statusutrke>(view.Model);
            Assert.Equal(5, model.StatusId);
        }

        [Fact]
        public void Create_Get_ReturnsViewResult()
        {
            var controller = MakeController();

            var result = controller.Create();

            Assert.IsType<ViewResult>(result);
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
        public async Task Edit_Get_ReturnsNotFoundWhenIdIsNull()
        {
            var controller = MakeController();

            var result = await controller.Edit(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFoundWhenServiceReturnsNull()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(3)).ReturnsAsync((Statusutrke?)null);
            var controller = MakeController();

            var result = await controller.Edit(3);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_ReturnsViewResultWithCorrectModel()
        {
            var status = new Statusutrke { StatusId = 3, Naziv = "Test" };
            _serviceMock.Setup(s => s.GetByIdAsync(3)).ReturnsAsync(status);
            var controller = MakeController();

            var result = await controller.Edit(3);

            var view = Assert.IsType<ViewResult>(result);
            Assert.IsType<Statusutrke>(view.Model);
        }

        [Fact]
        public async Task Edit_Post_ReturnsViewWhenModelStateInvalid()
        {
            var controller = MakeController();
            controller.ModelState.AddModelError("Naziv", "Obavezno");
            var status = new Statusutrke { StatusId = 1 };

            var result = await controller.Edit(1, status);

            Assert.IsType<ViewResult>(result);
            _serviceMock.Verify(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<Statusutrke>()), Times.Never);
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
        public async Task Delete_Get_ReturnsNotFoundWhenIdIsNull()
        {
            var controller = MakeController();

            var result = await controller.Delete(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFoundWhenServiceReturnsNull()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(7)).ReturnsAsync((Statusutrke?)null);
            var controller = MakeController();

            var result = await controller.Delete(7);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsViewResultWithCorrectModel()
        {
            var status = new Statusutrke { StatusId = 7, Naziv = "Za brisanje" };
            _serviceMock.Setup(s => s.GetByIdAsync(7)).ReturnsAsync(status);
            var controller = MakeController();

            var result = await controller.Delete(7);

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Statusutrke>(view.Model);
            Assert.Equal(7, model.StatusId);
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
    }
}
