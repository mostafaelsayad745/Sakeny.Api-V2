using Microsoft.EntityFrameworkCore;
using sakeny.DbContexts;
using sakeny.Entities;

namespace sakeny.Services.NewPostRepo
{
    public class PostRepo : IpostRepo
    {
        private readonly HOUSE_RENT_DBContext _context;

        public PostRepo(HOUSE_RENT_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostsTbl>> GetAllPostsAsync()
        {
            return await _context.PostsTbls
                .Include(p => p.PostFeedbackTbls)
                .Include(p => p.PostPicTbls)
                .Include(p => p.Features)
                .ToListAsync();
            
        }

        public async Task<IEnumerable<PostsTbl>> GetAllPostsByUserIdAsync(decimal userId)
        {
            return await _context.PostsTbls
                .Include(p => p.PostFeedbackTbls)
                .Include(p => p.PostPicTbls)
                .Include(p => p.Features)
                .Where(p => p.PostUserId == userId)
                .ToListAsync();
        }

        public async Task<PostsTbl> GetPostByIdAsync(decimal id)
        {
            return await _context.PostsTbls
                .Include(p => p.PostFeedbackTbls)
                .Include(p => p.PostPicTbls)
                .Include(p => p.Features)
                .FirstOrDefaultAsync(p => p.PostId == id);
        }
        





        public async Task AddPostAsync(PostDto postDto)
        {
            var post = new PostsTbl
            {
                PostDate = postDto.PostDate,
                PostTime = postDto.PostTime,
                PostCategory = postDto.PostCategory,
                PostTitle = postDto.PostTitle,
                PostBody = postDto.PostBody,
                PostArea = postDto.PostArea,
                PostKitchens = postDto.PostKitchens,
                PostBedrooms = postDto.PostBedrooms,
                PostBathrooms = postDto.PostBathrooms,
                PostLookSea = postDto.PostLookSea,
                PostPetsAllow = postDto.PostPetsAllow,
                PostCurrency = postDto.PostCurrency,
                PostPriceAi = postDto.PostPriceAi,
                PostPriceDisplay = postDto.PostPriceDisplay,
                PostPriceType = postDto.PostPriceType,
                PostAddress = postDto.PostAddress,
                PostCity = postDto.PostCity,
                PostState = postDto.PostState,
                PostFloor = postDto.PostFloor,
                PostLatitude = postDto.PostLatitude,
                PostLongitude = postDto.PostLongitude,
                PostStatue = postDto.PostStatue,
                PostUserId = postDto.PostUserId
            };

            

            post.PostPicTbls = postDto.PostPicTbls.Select(picDto => new PostPicTbl
            {
                //Picture = Convert.FromBase64String(picDto.PictureString),
                PictureString = picDto.PictureString
            }).ToList();

            post.Features = postDto.Features.Select(featureDto => new FeaturesTbl
            {
                FeaturesName = featureDto.FeaturesName
            }).ToList();

            await _context.PostsTbls.AddAsync(post);
        }


        public async Task<PostsTbl> UpdatePost(decimal id, PostDto postDto)
        {
            var post = _context.PostsTbls
                .Include(p => p.PostFeedbackTbls)
                .Include(p => p.PostPicTbls)
                .Include(p => p.Features)
                .FirstOrDefault(p => p.PostId == id);

            if (post != null)
            {
                // Map properties from postDto to post
                post.PostDate = postDto.PostDate;
                post.PostTime = postDto.PostTime;
                post.PostCategory = postDto.PostCategory;
                post.PostTitle = postDto.PostTitle;
                post.PostBody = postDto.PostBody;
                post.PostArea = postDto.PostArea;
                post.PostKitchens = postDto.PostKitchens;
                post.PostBedrooms = postDto.PostBedrooms;
                post.PostBathrooms = postDto.PostBathrooms;
                post.PostLookSea = postDto.PostLookSea;
                post.PostPetsAllow = postDto.PostPetsAllow;
                post.PostCurrency = postDto.PostCurrency;
                post.PostPriceAi = postDto.PostPriceAi;
                post.PostPriceDisplay = postDto.PostPriceDisplay;
                post.PostPriceType = postDto.PostPriceType;
                post.PostAddress = postDto.PostAddress;
                post.PostCity = postDto.PostCity;
                post.PostState = postDto.PostState;
                post.PostFloor = postDto.PostFloor;
                post.PostLatitude = postDto.PostLatitude;
                post.PostLongitude = postDto.PostLongitude;
                post.PostStatue = postDto.PostStatue;
                post.PostUserId = postDto.PostUserId;

                post.PostPicTbls = postDto.PostPicTbls.Select(picDto => new PostPicTbl
                {
                    PictureString = picDto.PictureString
                }).ToList();

                post.Features = postDto.Features.Select(featureDto => new FeaturesTbl
                {
                    FeaturesName = featureDto.FeaturesName
                }).ToList();

                _context.Entry(post).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return post;
			}
			return null;
		}


        public async Task DeletePost(decimal id)
        {
            var favorites = _context.PostFaviourateTbls.Where(f => f.PostId == id);
            _context.PostFaviourateTbls.RemoveRange(favorites);
            await _context.SaveChangesAsync();

            var post = await _context.PostsTbls.FindAsync(id);
            if (post != null)
            {
                _context.PostsTbls.Remove(post);
                await _context.SaveChangesAsync();
            }
        }



        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PostsTbl>> SearchPostsAsync(PostDto searchCriteria)
        {
            var query = _context.PostsTbls.AsQueryable();

            if (searchCriteria.PostBedrooms.HasValue)
            {
                query = query.Where(p => p.PostBedrooms >= searchCriteria.PostBedrooms);
            }

            if (searchCriteria.PostArea.HasValue)
            {
                query = query.Where(p => p.PostArea >= searchCriteria.PostArea);
            }

            if (!string.IsNullOrEmpty(searchCriteria.PostCategory))
            {
                query = query.Where(p => p.PostCategory == searchCriteria.PostCategory);
            }

            if (!string.IsNullOrEmpty(searchCriteria.PostCity))
            {
                query = query.Where(p => p.PostCity == searchCriteria.PostCity);
            }

            if (!string.IsNullOrEmpty(searchCriteria.PostState))
            {
                query = query.Where(p => p.PostState == searchCriteria.PostState);
            }

            if (searchCriteria.PostPriceAi.HasValue)
            {
                query = query.Where(p => p.PostPriceAi >= searchCriteria.PostPriceAi);
            }

            if (searchCriteria.PostStatue.HasValue)
            {
                query = query.Where(p => p.PostStatue == searchCriteria.PostStatue);
            }

            return await query
                .Include(p => p.PostPicTbls)
                .Include(p => p.Features)
                .ToListAsync();
        }
        public async Task<bool> PostExistsAsync(decimal id)
        {
            return await _context.PostsTbls.AnyAsync(p => p.PostId == id);
        }

		public async Task<IEnumerable<PostDto>> GetAllPostsAsync(string userId)
		{
			// Assuming userId is a string, parse it to the appropriate type needed for querying.
			// This parsing should be safe and handled appropriately.
			var parsedUserId = int.Parse(userId); // Adjust parsing as per your user ID type.

			// Fetch favorite post IDs for the current user.
			var favouritePostIds = await _context.PostFaviourateTbls
				.Where(f => f.UserId == parsedUserId)
				.Select(f => f.PostId)
				.ToListAsync();

			var postsQuery = _context.PostsTbls
				.OrderByDescending(p => p.PostDate)
				.Select(p => new PostDto
				{
					PostId = p.PostId,
					PostDate = p.PostDate,
					PostTime = p.PostTime,
					PostCategory = p.PostCategory,
					PostTitle = p.PostTitle,
					PostAddress = p.PostAddress,
                    PostBody = p.PostBody,
                    PostBedrooms = p.PostBedrooms,
                    PostBathrooms = p.PostBathrooms,
                    PostKitchens = p.PostKitchens,
                    PostLookSea = p.PostLookSea,
                    PostPetsAllow = p.PostPetsAllow,
                    PostCurrency = p.PostCurrency,
                    PostPriceDisplay = p.PostPriceDisplay,
                    PostPriceType = p.PostPriceType,
                    PostFloor = p.PostFloor,
                    PostLatitude = p.PostLatitude,
                    PostLongitude = p.PostLongitude,
                    PostStatue = p.PostStatue,
					PostArea = p.PostArea,
					PostPriceAi = p.PostPriceAi,
					PostCity = p.PostCity,
					PostState = p.PostState,
					PostUserId = p.PostUserId,
					PostOwnerName = _context.UsersTbls.Where(u => u.UserId == p.PostUserId).Select(u => u.UserFullName).FirstOrDefault(),
					PostPicTbls = p.PostPicTbls.Select(pic => new PostPicTblDto
					{
						PictureString = pic.PictureString
					}).Take(1).ToList(),
					Features = p.Features.Select(f => new FeaturesTblDto
					{
						FeaturesName = f.FeaturesName
					}).ToList(),
					IsFavourited = favouritePostIds.Contains((int)p.PostId), // Set IsFavourited based on the fetched favorite post IDs
				});

			return await postsQuery.ToListAsync();
		}



	}

}
