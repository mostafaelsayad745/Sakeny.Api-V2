using Microsoft.EntityFrameworkCore;
using sakeny.DbContexts;
using sakeny.Entities;

namespace sakeny.Services.FaviourateRepo
{
    public class FaviourateRepo : IFaviourateRepo
    {
        private readonly HOUSE_RENT_DBContext _context;

        public FaviourateRepo(HOUSE_RENT_DBContext context)
        {
            _context = context;
        }

        public async Task AddPostToFaviourate(PostFaviourateTbl postFaviourateTbl)
        {
            _context.PostFaviourateTbls.Add(postFaviourateTbl);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<int>> GetFavoritePosts(int userId)
        {
            var postIds = await _context.PostFaviourateTbls
                .Where(p => p.UserId == userId)
                .Select(p => p.PostId)
                .ToListAsync();

            return postIds.Select(id => (int)id);
        }

        public async Task RemovePostFromFaviourate(int postid)
        {
            var postFaviourateTbl = await _context.PostFaviourateTbls
                .FirstOrDefaultAsync(p => p.PostId == postid);
            _context.PostFaviourateTbls.Remove(postFaviourateTbl);
            await _context.SaveChangesAsync();
        }
    }
}
