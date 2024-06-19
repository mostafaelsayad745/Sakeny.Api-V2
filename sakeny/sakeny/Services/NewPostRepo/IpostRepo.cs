using sakeny.Entities;

namespace sakeny.Services.NewPostRepo
{
    public interface IpostRepo
    {
        Task<IEnumerable<PostsTbl>> GetAllPostsAsync();
        Task<PostsTbl> GetPostByIdAsync(decimal id);
        Task<IEnumerable<PostsTbl>> GetAllPostsByUserIdAsync(decimal userId);
        Task AddPostAsync(PostDto postDto);
        Task<PostsTbl> UpdatePost(decimal id, PostDto postDto);
		Task DeletePost(decimal id);
        Task SaveChangesAsync();
        Task<IEnumerable<PostsTbl>> SearchPostsAsync(PostDto searchCriteria);
        Task<bool> PostExistsAsync(decimal id);
        Task<IEnumerable<PostDto>> GetAllPostsAsync(string userId);

	}
}
