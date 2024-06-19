using sakeny.Entities;

namespace sakeny.Services.FaviourateRepo
{
    public interface IFaviourateRepo
    {
        Task<IEnumerable<int>> GetFavoritePosts(int userId);
        Task AddPostToFaviourate(PostFaviourateTbl postFaviourateTbl);
        Task RemovePostFromFaviourate(int postid);
        
    }
}
