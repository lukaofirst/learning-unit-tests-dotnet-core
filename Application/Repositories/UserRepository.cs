using Core.Entities;
using Core.Interfaces;

namespace Application.Repositories
{
	public class UserRepository : IUserRepository
	{
		public async Task<List<User>> GetAll()
		{
			var users = new List<User>()
			{
				new User(1, "Lorem", 25, "lorem@test.com"),
				new User(1, "Ipsum", 25, "ipsum@test.com"),
				new User(1, "Amet", 25, "amet@test.com"),
			};

			await Task.Delay(1000);

			return users;
		}

		public async Task<User> GetById(int id)
		{
			var users = new List<User>()
			{
				new User(1, "Lorem", 25, "lorem@test.com"),
				new User(2, "Ipsum", 25, "ipsum@test.com"),
				new User(3, "Amet", 25, "amet@test.com"),
			};

			var user = users.Single(x => x.Id == id);

			await Task.Delay(1000);

			return user!;
		}

		public async Task<User> Create(User user)
		{
			await Task.Delay(1000);

			return user;
		}

		public async Task<bool> Delete(int id)
		{
			await Task.Delay(1000);

			return true;
		}
	}
}