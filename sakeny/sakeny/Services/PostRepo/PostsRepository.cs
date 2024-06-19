using Microsoft.EntityFrameworkCore;
using sakeny.DbContexts;
using sakeny.Entities;

namespace sakeny.Services.PostRepo
{
    public class PostsRepository : IPostsRepository
    {
        private readonly HOUSE_RENT_DBContext _context;

        public PostsRepository(HOUSE_RENT_DBContext context)
        {
            _context = context;
        }

        public async Task CreatePost(PostModel postModel, decimal userId)
        {

            var post = postModel.Post;
            post.PostUserId = userId;

            foreach (var picture in postModel.Pictures)
            {
                post.PostPicTbls.Add(picture);
            }

            foreach (var feature in postModel.Features)
            {
                _context.FeaturesTbls.Add(feature);
                var postFeature = new PostFeaturesTbl
                {
                    FeaturesId = feature.FeaturesId,
                    PostId = post.PostId,
                    Post = post,
                    Features = feature
                };
                post.PostFeaturesTbls.Add(postFeature);
            }

            foreach (var feedback in postModel.Feedbacks)
            {
                post.PostFeedbackTbls.Add(feedback);
            }

            _context.PostsTbls.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePost(decimal id, decimal userId)
        {
            var post = await _context.PostsTbls
                .Where(p => p.PostUserId == userId)
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post != null)
            {
                _context.PostsTbls.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PostModel> GetPost(decimal id, decimal userId)
        {
            var post = await _context.PostsTbls
                .Include(p => p.PostPicTbls)
                .Include(p => p.PostFeaturesTbls)
                .ThenInclude(pf => pf.Features)
                .Include(p => p.PostFeedbackTbls)
                .FirstOrDefaultAsync(p => p.PostId == id && p.PostUserId == userId);

            if (post == null)
            {
                return null;
            }

            var postModel = new PostModel
            {
                Post = post,
                Pictures = post.PostPicTbls.ToList(),
                Features = post.PostFeaturesTbls.Select(pf => pf.Features).ToList(),
                Feedbacks = post.PostFeedbackTbls.ToList()
            };

            return postModel;
        }


        public async Task<IEnumerable<PostModel>> GetPosts(decimal userId)
        {
            var posts = await _context.PostsTbls
                .Where(p => p.PostUserId == userId)
                .Include(p => p.PostPicTbls)
                .Include(p => p.PostFeaturesTbls)
                .ThenInclude(pf => pf.Features)
                .Include(p => p.PostFeedbackTbls)
                .ToListAsync();

            var postModels = posts.Select(post => new PostModel
            {
                Post = post,
                Pictures = post.PostPicTbls.ToList(),
                Features = post.PostFeaturesTbls.Select(pf => pf.Features).ToList(),
                Feedbacks = post.PostFeedbackTbls.ToList()
            });

            return postModels;
        }


        public async Task UpdatePost(decimal id, PostModel postModel, decimal userId)
        {
            var post = await _context.PostsTbls
                .Include(p => p.PostPicTbls)
                .Include(p => p.PostFeaturesTbls)
                .ThenInclude(pf => pf.Features)
                .Include(p => p.PostFeedbackTbls)
                .FirstOrDefaultAsync(p => p.PostId == id && p.PostUserId == userId);

            if (post == null)
            {
                throw new Exception("Post not found");
            }


            // Continue for all properties of Post

            // Update pictures
            post.PostPicTbls.Clear();
            foreach (var picture in postModel.Pictures)
            {
                post.PostPicTbls.Add(picture);
            }

            // Update features
            post.PostFeaturesTbls.Clear();
            foreach (var feature in postModel.Features)
            {
                var postFeature = new PostFeaturesTbl
                {
                    FeaturesId = feature.FeaturesId,
                    PostId = post.PostId,
                    Post = post,
                    Features = feature
                };
                post.PostFeaturesTbls.Add(postFeature);
            }

            // Update feedbacks
            post.PostFeedbackTbls.Clear();
            foreach (var feedback in postModel.Feedbacks)
            {
                post.PostFeedbackTbls.Add(feedback);
            }

            await _context.SaveChangesAsync();
        }

    }
}
