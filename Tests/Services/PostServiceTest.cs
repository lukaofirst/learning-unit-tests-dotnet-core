using Application.Services;
using Core.Entities;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace Tests.Services
{
    public class PostServiceTest
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly PostService _postService;

        public PostServiceTest()
        {
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _postService = new PostService(_httpClientFactory.Object);
        }

        [Trait("GetPosts", "Succeed")]
        [Fact]
        public async Task GetPosts_Should_Return_List_Of_Posts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { Id = 1, UserId = 1, Title = "Title 1", Body = "Body 1" },
                new Post { Id = 2, UserId = 2, Title = "Title 2", Body = "Body 2" },
            };
            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(posts))
            };

            var httpClient = SetupMockedHttpClient(expectedResponse);
            _httpClientFactory.Setup(x => x.CreateClient(nameof(PostService))).Returns(httpClient);

            // Act
            var result = await _postService.GetPosts();

            // Assert
            Assert.True(result.Any());
        }

        private static HttpClient SetupMockedHttpClient(HttpResponseMessage expectedResponse)
        {
            var handleHttpMessage = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handleHttpMessage
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var httpClient = new HttpClient(handleHttpMessage.Object)
            {
                BaseAddress = new Uri("https://mockedurl.com")
            };

            return httpClient;
        }
    }
}
