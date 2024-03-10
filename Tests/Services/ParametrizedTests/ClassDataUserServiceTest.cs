using Application.Services;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections;

namespace Tests.Services.ParametrizedTests
{
    public class ClassDataUserServiceTest
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly UserService _userService;

        public ClassDataUserServiceTest()
        {
            _logger = Substitute.For<ILogger<UserService>>();
            _userRepository = Substitute.For<IUserRepository>();
            _userService = new UserService(_userRepository, _logger);
        }

        [Trait("Create", "Succeed")]
        [Theory]
        [ClassData(typeof(UserTestData))]
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

        public class UserTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { 4, "Wagner", 25, "wagner@gmail.com" };
                yield return new object[] { 5, "Maggie", 22, "maggie@gmail.com" };
                yield return new object[] { 6, "Durant", 18, "durant@gmail.com" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
