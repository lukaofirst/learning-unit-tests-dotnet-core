using Core.Entities;

namespace Core.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetPosts();
    }
}
