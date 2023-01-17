using Application.Exceptions;
using Application.Services;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Services
{
	public class UserServiceTest
	{
		private readonly Mock<ILogger<UserService>> _logger;
		private readonly Mock<IUserRepository> _userRepository;
		private readonly UserService _userService;

		public UserServiceTest()
		{
			_logger = new Mock<ILogger<UserService>>();
			_userRepository = new Mock<IUserRepository>();
			_userService = new UserService(_userRepository.Object, _logger.Object);
		}

		[Trait("GetAll", "Succeed")]
		[Fact]
		public async void GetAll_Should_Succeeded()
		{
			// Arrange
			var users = new List<User>()
			{
				new User(1, "Lorem", 25, "lorem@test.com"),
				new User(2, "Ipsum", 25, "ipsum@test.com"),
				new User(3, "Amet", 25, "amet@test.com"),
			};

			_userRepository.Setup(x => x.GetAll()).ReturnsAsync(users);

			// Act
			var result = await _userService.GetAll();

			// Assert
			Assert.NotNull(result);
		}

		[Trait("GetById", "Succeed")]
		[Fact]
		public async void GetById_Should_Succeeded_If_Entity_Exist()
		{
			// Arrange
			var user = new User(1, "Lorem", 25, "lorem@test.com");

			_userRepository.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);

			// Act
			var result = await _userService.GetById(user.Id);

			// Assert
			Assert.NotNull(result);
		}

		[Trait("GetById", "Throw EntityNotFoundException")]
		[Fact]
		public async void GetById_Should_Throw_EntityNotFoundException_If_Entity_Exist()
		{
			// Arrage
			var userId = 999;

			// Act 
			var result = _userService.GetById(userId);

			// Assert
			await Assert.ThrowsAsync<EntityNotFoundException>(() => result);
		}

		[Trait("Create", "Succeed")]
		[Theory]
		[InlineData(4, "Wagner", 25, "wagner@gmail.com")]
		[InlineData(5, "Maggie", 22, "maggie@gmail.com")]
		[InlineData(6, "Durant", 18, "durant@gmail.com")]
		public async void Create_Should_Succeeded_If_UserId_Not_Exist(int id, string name, int age, string email)
		{
			// Given
			var entity = new User(id, name, age, email);
			_userRepository.Setup(x => x.Create(entity)).ReturnsAsync(entity);

			// When
			var result = await _userService.Create(entity);

			// Then
			Assert.NotNull(result);
		}

		[Trait("Create", "Throw EntityAlreadyExistException")]
		[Fact]
		public async void Create_Should_Throw_EntityAlreadyExistException_If_UserId_Exist()
		{
			// Arrange
			var entity = new User(1, "Lorem", 25, "lorem@test.com");
			_userRepository.Setup(x => x.GetById(entity.Id)).ReturnsAsync(entity);

			// Act
			var result = _userService.Create(entity);

			// Assert
			await Assert.ThrowsAsync<EntityAlreadyExistException>(() => result);
		}

		[Trait("Delete", "Succeed")]
		[Fact]
		public async void Delete_Should_Succeeded_If_UserId_Exist()
		{
			// Arrange
			var entity = new User(1, "Lorem", 25, "lorem@test.com");
			_userRepository.Setup(x => x.GetById(entity.Id)).ReturnsAsync(entity);
			_userRepository.Setup(x => x.Delete(entity.Id)).ReturnsAsync(true);

			// Act
			var result = await _userService.Delete(entity.Id);

			// Assert
			Assert.True(result);
		}

		[Trait("Delete", "Throw EntityNotFoundException")]
		[Fact]
		public async void Delete_Should_Throw_EntityNotFoundException_If_UserId_Not_Exist()
		{
			// Arrange
			var userId = 1;
			User? entity = null;

			// Act
			_userRepository.Setup(x => x.GetById(userId))!.ReturnsAsync(entity);

			// Assert
			await Assert.ThrowsAsync<EntityNotFoundException>(() => _userService.Delete(userId));
		}
	}
}