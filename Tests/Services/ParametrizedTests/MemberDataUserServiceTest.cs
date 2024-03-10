﻿using Application.Services;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Tests.Services.ParametrizedTests
{
    public class MemberDataUserServiceTest
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly UserService _userService;

        public MemberDataUserServiceTest()
        {
            _logger = Substitute.For<ILogger<UserService>>();
            _userRepository = Substitute.For<IUserRepository>();
            _userService = new UserService(_userRepository, _logger);
        }

        [Trait("Create", "Succeed")]
        [Theory]
        [MemberData(nameof(UserTestData))]
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

        public static IEnumerable<object[]> UserTestData => new List<object[]>
        {
            new object[] { 4, "Wagner", 25, "wagner@gmail.com" },
            new object[] { 5, "Maggie", 22, "maggie@gmail.com" },
            new object[] { 6, "Durant", 18, "durant@gmail.com" },
        };
    }
}
