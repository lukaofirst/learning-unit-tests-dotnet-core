using Core.Entities;

namespace Core.Interfaces
{
	public interface IUserService
	{
		Task<List<User>> GetAll();
		Task<User> GetById(int id);
		Task<User> Create(User user);
		Task<bool> Delete(int id);
	}
}