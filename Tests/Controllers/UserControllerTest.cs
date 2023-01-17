using System.Net;
using Application.Exceptions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebAPI.Controllers;

namespace Tests.Controllers
{
	public class UserControllerTest
	{
		private readonly Mock<ILogger<UserController>> _logger;
		private readonly Mock<IUserService> _userService;
		private readonly UserController _userController;

		public UserControllerTest()
		{
			_logger = new Mock<ILogger<UserController>>();
			_userService = new Mock<IUserService>();
			_userController = new UserController(_userService.Object, _logger.Object);
		}

		[Trait("GetAll", "200")]
		[Fact]
		public async void GetAll_Should_Return_200_If_Succeeded()
		{
			var users = new List<User>()
			{
				new User(1, "Lorem", 25, "lorem@test.com"),
				new User(1, "Ipsum", 25, "ipsum@test.com"),
				new User(1, "Amet", 25, "amet@test.com"),
			};

			_userService.Setup(x => x.GetAll()).ReturnsAsync(users);

			var result = (await _userController.GetAll()) as OkObjectResult;

			Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
		}

		[Trait("GetAll", "500")]
		[Fact]
		public void GetAll_Should_Return_500_If_Succeeded()
		{
			_userService.Setup(x => x.GetAll()).ThrowsAsync(new Exception());

			Assert.ThrowsAsync<Exception>(() => _userController.GetAll());
		}

		[Trait("GetById", "200")]
		[Fact]
		public async void GetById_Should_Return_200_If_Succeeded()
		{
			var user = new User(1, "Lorem", 25, "lorem@test.com");

			_userService.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);

			var result = (await _userController.GetById(user.Id)) as OkObjectResult;

			Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
		}

		[Trait("GetById", "400")]
		[Fact]
		public async void GetById_Should_Return_400_If_Throw_EntityNotFoundException()
		{
			_userService.Setup(x => x.GetById(It.IsAny<int>()))
				.ThrowsAsync(new EntityNotFoundException("Entity not found in our database!"));

			var result = (await _userController.GetById(It.IsAny<int>())) as NotFoundObjectResult;

			Assert.Equal((int)HttpStatusCode.NotFound, result!.StatusCode);
		}

		[Trait("GetById", "500")]
		[Fact]
		public async void GetById_Should_Return_500_If_Throw_Exception()
		{
			_userService.Setup(x => x.GetById(It.IsAny<int>()))
				.ThrowsAsync(new Exception());

			var result = (await _userController.GetById(It.IsAny<int>())) as ObjectResult;

			Assert.Equal((int)HttpStatusCode.InternalServerError, result!.StatusCode);
		}

		[Trait("Create", "200")]
		[Fact]
		public async void Create_Should_Return_200_If_Succeeded()
		{
			var user = new User(1, "Lorem", 25, "lorem@test.com");
			_userService.Setup(x => x.Create(user)).ReturnsAsync(user);

			var result = (await _userController.Create(user)) as OkObjectResult;

			Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
		}

		[Trait("Create", "409")]
		[Fact]
		public async void Create_Should_Return_409_If_Throw_EntityAlreadyExistException()
		{
			_userService.Setup(x => x.Create(It.IsAny<User>()))
				.ThrowsAsync(new EntityAlreadyExistException("Entity already exist in our database!"));

			var result = (await _userController.Create(It.IsAny<User>())) as ConflictObjectResult;

			Assert.Equal((int)HttpStatusCode.Conflict, result!.StatusCode);

		}

		[Trait("Create", "500")]
		[Fact]
		public async void Create_Should_Return_500_If_Throw_Exception()
		{
			_userService.Setup(x => x.Create(It.IsAny<User>()))
				.ThrowsAsync(new Exception());

			var result = (await _userController.Create(It.IsAny<User>())) as ObjectResult;

			Assert.Equal((int)HttpStatusCode.InternalServerError, result!.StatusCode);
		}

		[Trait("Delete", "200")]
		[Fact]
		public async void Delete_Should_Return_200_If_Succeeded()
		{
			var userId = 1;
			_userService.Setup(x => x.Delete(userId)).ReturnsAsync(true);

			var result = (await _userController.Delete(userId)) as NoContentResult;

			Assert.Equal((int)HttpStatusCode.NoContent, result!.StatusCode);
		}

		[Trait("Delete", "400")]
		[Fact]
		public async void Delete_Should_Return_400_If_Throw_EntityNotFoundException()
		{
			_userService.Setup(x => x.Delete(It.IsAny<int>()))
				.ThrowsAsync(new EntityNotFoundException("Entity not found in our database!"));

			var result = (await _userController.Delete(It.IsAny<int>())) as NotFoundObjectResult;

			Assert.Equal((int)HttpStatusCode.NotFound, result!.StatusCode);
		}

		[Trait("Delete", "500")]
		[Fact]
		public async void Delete_Should_Return_500_If_Throw_Exception()
		{
			_userService.Setup(x => x.Delete(It.IsAny<int>()))
				.ThrowsAsync(new Exception());

			var result = (await _userController.Delete(It.IsAny<int>())) as ObjectResult;

			Assert.Equal((int)HttpStatusCode.InternalServerError, result!.StatusCode);
		}
	}
}