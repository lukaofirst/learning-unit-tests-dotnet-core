using Application.Exceptions;
using Application.Services;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Tests.Services
{
	public class UserServiceTest
	{
		private readonly ILogger<UserService> _logger;
		private readonly IUserRepository _userRepository;
		private readonly UserService _userService;

		public UserServiceTest()
		{
			_logger = Substitute.For<ILogger<UserService>>();
			_userRepository = Substitute.For<IUserRepository>();
			_userService = new UserService(_userRepository, _logger);
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
			_userRepository.GetAll().Returns(users);

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
			_userRepository.GetById(user.Id).Returns(user);

			// Act
			var result = await _userService.GetById(user.Id);

			// Assert
			Assert.NotNull(result);
		}
		
		[Trait("GetById", "Succeed")]
		[Fact]
		public async void GetById_Should_Call_UserService_GetById_Once()
		{
			// Arrange
			var user = new User(1, "Lorem", 25, "lorem@test.com");

			_userRepository.GetById(user.Id).Returns(user);

			// Act
			var result = await _userService.GetById(user.Id);

			// Assert
			await _userRepository.Received(1).GetById(user.Id)!;
		}

		[Trait("GetById", "Throw EntityNotFoundException")]
		[Fact]
        public async Task GetById_Should_Throw_EntityNotFoundException_If_Entity_Exist()
		{
			// Arrange
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
			_userRepository.Create(entity).Returns(entity);

			// When
			var result = await _userService.Create(entity);

			// Then
			Assert.NotNull(result);
		}

		[Trait("Create", "Throws EntityAlreadyExistException")]
		[Fact]
		public async void Create_Should_Throw_EntityAlreadyExistException_If_UserId_Exist()
		{
			// Arrange
			var entity = new User(1, "Lorem", 25, "lorem@test.com");
			_userRepository.GetById(entity.Id).Returns(entity);

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
			_userRepository.GetById(entity.Id).Returns(entity);
			_userRepository.Delete(entity.Id).Returns(true);

			// Act
			var result = await _userService.Delete(entity.Id);

			// Assert
			Assert.True(result);
		}

		[Trait("Delete", "Throws EntityNotFoundException")]
		[Fact]
		public async void Delete_Should_Throw_EntityNotFoundException_If_UserId_Not_Exist()
		{
			// Arrange
			var userId = 1;
			User? entity = null;

			// Act
			_userRepository.GetById(userId)!.Returns(entity);

			// Assert
			await Assert.ThrowsAsync<EntityNotFoundException>(() => _userService.Delete(userId));
		}
	}
}