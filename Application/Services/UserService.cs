using Application.Exceptions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly ILogger<UserService> _logger;

		public UserService(IUserRepository userRepository, ILogger<UserService> logger)
		{
			_logger = logger;
			_userRepository = userRepository;
		}

		public async Task<List<User>> GetAll()
		{
			var entities = await _userRepository.GetAll();

			return entities;
		}

		public async Task<User> GetById(int id)
		{
			var entity = await _userRepository.GetById(id)!;

			if (entity == null)
				throw new EntityNotFoundException("Entity not found in our database!");

			return entity;
		}

		public async Task<User> Create(User user)
		{
			var entityExist = await _userRepository.GetById(user.Id)!;

			if (entityExist != null)
				throw new EntityAlreadyExistException("Entity already exist in our database!");

			var createdEntity = await _userRepository.Create(user);

			return createdEntity;
		}

		public async Task<bool> Delete(int id)
		{
			var entityExist = await _userRepository.GetById(id)!;

			if (entityExist == null)
				throw new EntityNotFoundException("Entity not found in our database!");

			var result = await _userRepository.Delete(id);

			return result;
		}
	}
}