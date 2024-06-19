using sakeny.Entities;

namespace sakeny.Services.PostRepo
{
    public interface IPostsRepository
    {
        Task<IEnumerable<PostModel>> GetPosts(decimal userId);
        Task<PostModel> GetPost(decimal id, decimal userId);
        Task CreatePost(PostModel postModel, decimal userId);
        Task UpdatePost(decimal id, PostModel postModel, decimal userId);
        Task DeletePost(decimal id, decimal userId);
    }

}
