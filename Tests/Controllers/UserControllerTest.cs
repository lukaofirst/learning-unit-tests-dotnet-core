using System.Net;
using Application.Exceptions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WebAPI.Controllers;

namespace Tests.Controllers
{
	public class UserControllerTest
	{
		private readonly ILogger<UserController> _logger;
		private readonly IUserService _userService;
		private readonly UserController _userController;

		public UserControllerTest()
		{
			_logger = Substitute.For<ILogger<UserController>>();
			_userService = Substitute.For<IUserService>();
			_userController = new UserController(_userService, _logger);
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

			_userService.GetAll().Returns(users);

			var result = (await _userController.GetAll()) as OkObjectResult;

			Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
		}

		[Trait("GetAll", "500")]
		[Fact]
		public void GetAll_Should_Return_500_If_Succeeded()
		{
			_userService.GetAll().Returns(Task.FromException<List<User>>(new Exception()));

			Assert.ThrowsAsync<Exception>(() => _userController.GetAll());
		}

		[Trait("GetById", "200")]
		[Fact]
		public async void GetById_Should_Return_200_If_Succeeded()
		{
			var user = new User(1, "Lorem", 25, "lorem@test.com");

			_userService.GetById(user.Id).Returns(user);

			var result = (await _userController.GetById(user.Id)) as OkObjectResult;

			Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
		}

		[Trait("GetById", "400")]
		[Fact]
		public async void GetById_Should_Return_400_If_Throw_EntityNotFoundException()
		{
            int userId = 999;
            _userService.GetById(userId)
				.Returns(Task.FromException<User>(new EntityNotFoundException("Entity not found in our database!")));

			var result = (await _userController.GetById(userId)) as NotFoundObjectResult;

			Assert.Equal((int)HttpStatusCode.NotFound, result!.StatusCode);
		}

		[Trait("GetById", "500")]
		[Fact]
		public async void GetById_Should_Return_500_If_Throw_Exception()
		{
            int userId = Arg.Any<int>();
            _userService.GetById(userId)
				.Returns(Task.FromException<User>(new Exception()));

			var result = (await _userController.GetById(userId)) as ObjectResult;

			Assert.Equal((int)HttpStatusCode.InternalServerError, result!.StatusCode);
		}

		[Trait("Create", "200")]
		[Fact]
		public async void Create_Should_Return_200_If_Succeeded()
		{
			var user = new User(1, "Lorem", 25, "lorem@test.com");
			_userService.Create(user).Returns(user);

			var result = (await _userController.Create(user)) as OkObjectResult;

			Assert.Equal((int)HttpStatusCode.OK, result!.StatusCode);
		}

		[Trait("Create", "409")]
		[Fact]
		public async void Create_Should_Return_409_If_Throw_EntityAlreadyExistException()
		{
            var user = new User(1, "Lorem", 25, "lorem@test.com");
            _userService.Create(user)
				.Returns(Task.FromException<User>(new EntityAlreadyExistException("Entity already exist in our database!")));

			var result = (await _userController.Create(user)) as ConflictObjectResult;

			Assert.Equal((int)HttpStatusCode.Conflict, result!.StatusCode);
		}

		[Trait("Create", "500")]
		[Fact]
		public async void Create_Should_Return_500_If_Throw_Exception()
		{
			User user = Arg.Any<User>();
            _userService.Create(user)
				.Returns(Task.FromException<User>(new Exception()));

			var result = (await _userController.Create(user)) as ObjectResult;

			Assert.Equal((int)HttpStatusCode.InternalServerError, result!.StatusCode);
		}

		[Trait("Delete", "200")]
		[Fact]
		public async void Delete_Should_Return_200_If_Succeeded()
		{
			var userId = 1;
			_userService.Delete(userId).Returns(true);

			var result = (await _userController.Delete(userId)) as NoContentResult;

			Assert.Equal((int)HttpStatusCode.NoContent, result!.StatusCode);
		}

		[Trait("Delete", "400")]
		[Fact]
		public async void Delete_Should_Return_400_If_Throw_EntityNotFoundException()
		{
			int userId = 999;
            _userService.Delete(userId)
                .Returns(Task.FromException<bool>(new EntityNotFoundException("Entity not found in our database!")));

            var result = (await _userController.Delete(userId)) as NotFoundObjectResult;

			Assert.Equal((int)HttpStatusCode.NotFound, result!.StatusCode);
		}

		[Trait("Delete", "500")]
		[Fact]
		public async void Delete_Should_Return_500_If_Throw_Exception()
		{
			int userId = Arg.Any<int>();
			_userService.Delete(userId)
				.Returns(Task.FromException<bool>(new Exception()));

            var result = (await _userController.Delete(userId)) as ObjectResult;

			Assert.Equal((int)HttpStatusCode.InternalServerError, result!.StatusCode);
		}
	}
}