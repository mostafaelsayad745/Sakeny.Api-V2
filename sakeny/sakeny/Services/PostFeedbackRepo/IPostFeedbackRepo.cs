using Microsoft.EntityFrameworkCore;
using sakeny.DbContexts;
using sakeny.Entities;

namespace sakeny.Services.PostFeedbackRepo
{
    public interface IPostFeedbackRepo
    {
        Task AddFeedback(PostFeedbackTbl feedback);
        void UpdateFeedback(PostFeedbackTbl feedback);
        void DeleteOneFeedback(decimal id);
        
        Task<IEnumerable<PostFeedbackTbl>> GetFeedbacksForPost(decimal postId);
        Task SaveChangesAsync();
        Task<PostFeedbackTbl> GetFeedbackByUserAndPost(int userId, decimal postId);
    }




    public class PostFeedbackRepo : IPostFeedbackRepo
    {
        private readonly HOUSE_RENT_DBContext _context;

        public PostFeedbackRepo(HOUSE_RENT_DBContext context)
        {
            _context = context;
        }

        public async Task AddFeedback(PostFeedbackTbl feedback)
        {
            _context.PostFeedbackTbls.Add(feedback);
            await _context.SaveChangesAsync();
        }

        public void UpdateFeedback(PostFeedbackTbl feedback)
        {
            _context.Entry(feedback).State = EntityState.Modified;
        }

        public void DeleteOneFeedback(decimal id)
        {
            var feedback = _context.PostFeedbackTbls.Find(id);
            if (feedback != null)
            {
                _context.PostFeedbackTbls.Remove(feedback);
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<PostFeedbackTbl>> GetFeedbacksForPost(decimal postId)
        {
            return await _context.PostFeedbackTbls
                .Where(f => f.PostId == postId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PostFeedbackTbl> GetFeedbackByUserAndPost(int userId, decimal postId)
        {
            return await _context.PostFeedbackTbls
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PostId == postId);
        }
    }

}
