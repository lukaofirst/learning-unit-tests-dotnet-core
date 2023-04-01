using Core.Entities;
using Core.Interfaces;
using System.Net.Http.Json;

namespace Application.Services
{
    public class PostService : IPostService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PostService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Post>> GetPosts()
        {
            var httpClient = _httpClientFactory.CreateClient(nameof(PostService));
            var response = await httpClient.GetAsync("/posts");

            var parsedResponse = await response.Content.ReadFromJsonAsync<List<Post>>();

            return parsedResponse!;
        }
    }
}
