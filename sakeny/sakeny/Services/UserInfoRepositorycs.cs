using Microsoft.EntityFrameworkCore;
using sakeny.DbContexts;
using sakeny.Entities;
using sakeny.Models.PostDtos;
using System.Linq;

namespace sakeny.Services
{
    public class UserInfoRepositorycs : IUserInfoRepository
    {
        private readonly HOUSE_RENT_DBContext _context;

        public UserInfoRepositorycs(HOUSE_RENT_DBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        // defination for the post feedbacks of the user

        public async Task AddPostFeedbackForUserAsync(int userId,int postId , PostFeedbackTbl postFeedbackTbl)
        {
            var post = await _context.PostsTbls.Where(p => p.PostId == postId).FirstOrDefaultAsync();
            if (post != null)
            {
                postFeedbackTbl.Post = post;
                postFeedbackTbl.PostFeedDate = DateTime.Now;
                postFeedbackTbl.PostFeedTime = DateTime.Now.TimeOfDay;
                postFeedbackTbl.User = await GetUserAsync(userId);
                post.PostFeedbackTbls.Add(postFeedbackTbl);
            }

        }


        public void DeletePostFeedback(PostFeedbackTbl postFeedbackTbl)
        {
            _context.PostFeedbackTbls.Remove(postFeedbackTbl);
        }

        public async Task<bool> PostFeedbackExistsAsync(int userId, int postId , DateTime? datetime)
        {
            return await _context.PostFeedbackTbls
                .AnyAsync(u => u.UserId == userId && u.PostId == postId && u.PostFeedDate == datetime);
        }

        public async Task<PostFeedbackTbl?> GetPostFeedbackForUserAsync(int userId, int postId, int postFeedbackId)
        {
            return await _context.PostFeedbackTbls
                .Where(p => p.UserId == userId && p.PostFeedId == postFeedbackId && p.PostId == postId
                ).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PostFeedbackTbl>> GetPostFeedbacksForUserAsync( int postId)
        {
            //return await _context.PostFeedbackTbls
            //    .Where(p => p.UserId == userId && p.PostId == postId).ToListAsync();
            var postFeedbacks = await _context.PostsTbls
                   .Where(p => p.PostId == postId)
                   .SelectMany(p => p.PostFeedbackTbls) // Flatten the nested collections
                   .ToListAsync();

            return postFeedbacks;
        }




        // defination for the user


        public async Task<UsersTbl?> validateUser(string email, string password)
        {
            return await _context.UsersTbls.Where(u => u.UserEmail == email && u.UserPassword == password)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> checkEmailNotRepated(UsersTbl user)
        {
            if (await _context.UsersTbls.AnyAsync(u => u.UserEmail == user.UserEmail))
            {
                return true;
            }
            return false;
        }

        public async Task AddUserAsync(UsersTbl user)
        {
            if (user != null)
            {
                await _context.UsersTbls.AddAsync(user);
            }
        }



        public void DeleteUserAsync(UsersTbl user)
        {
            _context.UsersTbls.Remove(user);
        }

        public async Task<UsersTbl?> GetUserAsync(int userId)
        {
            return await _context.UsersTbls.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<string?> GetUserNameAsync(int userId)
        {
            var user = await _context.UsersTbls
                                    .Where(u => u.UserId == userId)
                                    .Select(u => u.UserFullName) // Selecting just the UserName
                                    .FirstOrDefaultAsync();

            return user;
        }

        public async Task<UsersTbl?> GetUserAsync(int userId, bool includePostFeedbacks = false)
        {
            if (includePostFeedbacks)
            {
                return await _context.UsersTbls.Include(u => u.PostFeedbackTbls)
                    .Where(u => u.UserId == userId).FirstOrDefaultAsync();
            }
            return await _context.UsersTbls.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        }



        public async Task<IEnumerable<UsersTbl>> GetUsersAsync()
        {
            return await _context.UsersTbls.OrderBy(u => u.UserName).ToListAsync();
        }

        public async Task<UsersTbl?> GetUserAsync(string userName)
        {
            userName = userName.Trim();
            return await _context.UsersTbls
                .Where(u => u.UserName.Contains(userName))
                .FirstOrDefaultAsync();
        }



        public async Task<(IEnumerable<UsersTbl>, PaginationMetadata)> GetUsersAsync(string? name, string? SearchQuery
            , int pageNumber, int pageSize)
        {

            var collection = _context.UsersTbls as IQueryable<UsersTbl>;
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(u => u.UserName == name);
            }
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                SearchQuery = SearchQuery.Trim();
                collection = collection.Where(u => u.UserName.Contains(SearchQuery) ||
                u.UserEmail != null && u.UserEmail.Contains(SearchQuery));
            }
            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(u => u.UserName)
                .Skip(pageSize * (pageNumber - 1)).Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task UpdateUserAsync(UsersTbl user)
        {

        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.UsersTbls.AnyAsync(u => u.UserId == userId);
        }


       // defination for the posts of the user

        public async Task<(IEnumerable<PostsTbl>,PaginationMetadata)> GetPostsForUserAsync(int userId 
            , string? name,
            string? searchQuery , int pageNumber , int PageSize)
        {
            var collection = _context.PostsTbls as IQueryable<PostsTbl>;

            if(! string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(p => p.PostUserId == userId && p.PostStatue == true
                && p.PostTitle == name);
            }
            if(! string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(p => p.PostUserId == userId && p.PostStatue == true);
                collection = collection.Where(p =>p.PostTitle != null 
                && p.PostTitle.Contains(searchQuery) || 
                p.PostAddress != null && p.PostAddress.Contains(searchQuery));
            }

            var totalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalItemCount, PageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(p => p.PostDate).
                Skip(PageSize * (pageNumber - 1)).Take(PageSize).ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }
        public async Task<bool> PostExistsAsync(int userId, int postId)
        {
            return await _context.PostsTbls.AnyAsync(u => u.PostUserId == userId && u.PostId == postId);
        }

        public async Task<bool> PostExistsAsync( int postId)
        {
            return await _context.PostsTbls.AnyAsync(u =>  u.PostId == postId);
        }

        public async Task<PostsTbl?> GetPostForUserAsync(int postId)
        {
            return await _context.PostsTbls.Where(p => p.PostId == postId)
                .FirstOrDefaultAsync();
        }

        public async Task<PostsTbl?> GetPostForUserAsync(int userId, int postId )
        {
            var post = await _context.PostsTbls.Where(p => p.PostUserId == userId && p.PostId == postId)
                .FirstOrDefaultAsync();
            

            if (post is not null && post.PostStatue == true )
            {
                return post;
            }
            return null;
        }
        public async Task<PostsTbl?> GetPostForUserAsyncForUpdate(int userId, int postId)
        {
            var post = await _context.PostsTbls.Where(p => p.PostUserId == userId && p.PostId == postId)
                .FirstOrDefaultAsync();

            if (post is not null && post.PostStatue == false)
            {
                post.PostStatue = true;
                return post;
            }
            else
            {
                post.PostStatue = false; 
                return post;
            }
        }

        public async Task<IEnumerable<PostsTbl>> SearchForPostAsync(PostForSerchDto postForSerchDto)
        {

            var query = _context.PostsTbls.AsQueryable();

            if (!string.IsNullOrWhiteSpace(postForSerchDto.PostTitle))
            {
                query = query.Where(p => p.PostTitle.Contains(postForSerchDto.PostTitle));
            }

            if (!string.IsNullOrWhiteSpace(postForSerchDto.PostAddress))
            {
                query = query.Where(p => p.PostAddress.Contains(postForSerchDto.PostAddress));
            }

            if (postForSerchDto.MinPrice.HasValue)
            {
                query = query.Where(p => p.PostPriceDisplay >= postForSerchDto.MinPrice.Value);
            }

            if (postForSerchDto.MaxPrice.HasValue)
            {
                query = query.Where(p => p.PostPriceDisplay <= postForSerchDto.MaxPrice.Value);
            }

            if (postForSerchDto.MinArea.HasValue)
            {
                query = query.Where(p => p.PostArea >= postForSerchDto.MinArea.Value);
            }

            if (postForSerchDto.MaxArea.HasValue)
            {
                query = query.Where(p => p.PostArea <= postForSerchDto.MaxArea.Value);
            }

            if (postForSerchDto.MinRooms.HasValue)
            {
                query = query.Where(p => p.PostBedrooms >= postForSerchDto.MinRooms.Value);
            }

            if (postForSerchDto.MaxRooms.HasValue)
            {
                query = query.Where(p => p.PostBedrooms <= postForSerchDto.MaxRooms.Value);
            }

            if (postForSerchDto.MinBathrooms.HasValue)
            {
                query = query.Where(p => p.PostBathrooms >= postForSerchDto.MinBathrooms.Value);
            }

            if (postForSerchDto.MaxBathrooms.HasValue)
            {
                query = query.Where(p => p.PostBathrooms <= postForSerchDto.MaxBathrooms.Value);
            }

            //if (postForSerchDto.MinGarages.HasValue)
            //{
            //    query = query.Where(p => p.PostGarages >= postForSerchDto.MinGarages.Value);
            //}

            //if (postForSerchDto.MaxGarages.HasValue)
            //{
            //    query = query.Where(p => p.PostGarages <= postForSerchDto.MaxGarages.Value);
            //}

            if (postForSerchDto.MinFloors.HasValue)
            {
                query = query.Where(p => p.PostFloor >= postForSerchDto.MinFloors.Value);
            }

            if (postForSerchDto.MaxFloors.HasValue)
            {
                query = query.Where(p => p.PostFloor <= postForSerchDto.MaxFloors.Value);
            }

            if (postForSerchDto.PostLookSea.HasValue)
            {
                query = query.Where(p => p.PostLookSea == postForSerchDto.PostLookSea.Value);
            }

            if (postForSerchDto.PostPetsAllow.HasValue)
            {
                query = query.Where(p => p.PostPetsAllow == postForSerchDto.PostPetsAllow.Value);
            }

            //if (postForSerchDto.MinYearBuilt.HasValue)
            //{
            //    query = query.Where(p => p.PostYearBuilt >= postForSerchDto.MinYearBuilt.Value);
            //}

            //if (postForSerchDto.MaxYearBuilt.HasValue)
            //{
            //    query = query.Where(p => p.PostYearBuilt <= postForSerchDto.MaxYearBuilt.Value);
            //}

            //if (postForSerchDto.MinRating.HasValue)
            //{
            //    query = query.Where(p => p.PostRating >= postForSerchDto.MinRating.Value);
            //}

            //if (postForSerchDto.MaxRating.HasValue)
            //{
            //    query = query.Where(p => p.PostRating <= postForSerchDto.MaxRating.Value);
            //}

            return await query.ToListAsync();
        }



        public async Task AddPostForUserAsync(int userId, PostsTbl postTbl)
        {
            var user = GetUserAsync(userId, false);
            if (user != null)
            {
                await _context.PostsTbls.AddAsync(postTbl);

            }
        }

        public void DeletePost(PostsTbl postTbl)
        {
            _context.PostsTbls.Remove(postTbl);
        }

        public async Task<IEnumerable<PostsTbl>> GetAllPostsAsync()
        {
            return await _context.PostsTbls.ToListAsync();
        }

        // defination for the features of the post

        public async Task<bool> FeaturesExistsAsync(int postId, int featuresId)
        {
            return await _context.PostFeaturesTbls.AnyAsync(u => u.PostId == postId && u.FeaturesId == featuresId);
        }

        public async Task<IEnumerable<FeaturesTbl>> GetFeaturesForPostAsync(int postId
            )
        {
            

            return await _context.PostFeaturesTbls.Where(p => p.PostId == postId)
                .Select(f => f.Features).ToListAsync();
        }

        public async Task<FeaturesTbl?> GetFeatureForPostAsync(int postId, int featuresId)
        {
            return await _context.PostFeaturesTbls.Where(p => p.PostId == postId && p.FeaturesId == featuresId)
                .Select(f => f.Features).FirstOrDefaultAsync();
        }

        public async Task AddFeatureForPostAsync(int postId, FeaturesTbl featureTbl)
        {
           var post = _context.PostsTbls.Where(p => p.PostId == postId).FirstOrDefault();
            if (post != null)
            {
                var postFeature = new PostFeaturesTbl
                {
                    FeaturesId = featureTbl.FeaturesId,
                    PostId = post.PostId,
                    Features = featureTbl,
                    Post = post
                };
                await _context.PostFeaturesTbls.AddAsync(postFeature);
            }
        }

        public void DeleteFeature(FeaturesTbl featureTbl)
        {
            _context.FeaturesTbls.Remove(featureTbl);
        }

        public async Task<bool> PictureExistsAsync(int postId, int pictureId)
        {
            return await _context.PostPicTbls.AnyAsync(u => u.PostId == postId && u.PostPicId == pictureId);
        }

        public async Task<IEnumerable<PostPicTbl>> GetPicturesForPostAsync(int postId)
        {
            return await _context.PostPicTbls.Where(p => p.PostId == postId).ToListAsync();
        }

        public async Task<PostPicTbl?> GetPictureForPostAsync(int postId, int pictureId)
        {
            var image = await _context.PostPicTbls.Where(p => p.PostId == postId && p.PostPicId == pictureId)
                .FirstOrDefaultAsync();
            image.Post = await GetPostForUserAsync(postId);
            return image;
        }

        public async Task AddPictureForPostAsync(PostPicTbl pictureTbl)
        {
            _context.PostPicTbls.Add(pictureTbl);
        }

        public void DeletePicture(PostPicTbl pictureTbl)
        {
            _context.PostPicTbls.Remove(pictureTbl);
        }

        // defination for the notifications of the user

        public async Task<IEnumerable<NotificationTbl>> GetNotificationsForUserAsync(int userId)
        {
            return await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();
        }

        public async Task<bool> NotificationExistsAsync(int userId, int notificationId)
        {
            return await _context.Notifications.AnyAsync(n => n.UserId == userId && n.NotificationId == notificationId);
        }

        public async Task<NotificationTbl> GetNotificationForUserAsync(int userId, int notificationId)
        {
            return await _context.Notifications.Where(n => n.UserId == userId && n.NotificationId == notificationId)
                .FirstOrDefaultAsync();
        }

        public async Task AddNotificationForUserAsync( NotificationTbl notificationTbl)
        {
            
            
                await _context.Notifications.AddAsync(notificationTbl);
            
        }

        public async Task AddNotificationForUserAsync(int userId, NotificationTbl notificationTbl)
        {
            var user = await GetUserAsync(userId);
            if (user != null)
            {
                notificationTbl.User = user;
                notificationTbl.UserId = userId;
                notificationTbl.Timestamp = DateTime.Now;
                await _context.Notifications.AddAsync(notificationTbl);
            }
        }

        public void DeleteNotification(NotificationTbl notificationTbl)
        {
            _context.Notifications.Remove(notificationTbl);
        }

        // defination for the chat of the user

        public async Task SendMessageAsync(int userId, decimal receiverId, string message)
        {
            var newMessage = new UserChatTbl
            {
                UserChatDate = DateTime.Now,
                UserChatTime = DateTime.Now.TimeOfDay,
                UserChatType = "text",
                UserChatText = message,
                UserChatFrom= _context.UsersTbls.Where(u => u.UserId == userId).Select(u => u.UserName).FirstOrDefault(),
                UserChatTo = _context.UsersTbls.Where(u => u.UserId == receiverId).Select(u => u.UserName).FirstOrDefault(),
                //SenderId = userId,
                //ReceiverId = (int)receiverId
            };
            await _context.UserChatTbls.AddAsync(newMessage);
        }

        public async Task<IEnumerable<UserChatTbl>> GetChatHistoryAsync(string senderName, string receiverName)
        {
            var messages = await _context.UserChatTbls
            .Where(m => (m.UserChatFrom == senderName && m.UserChatTo == receiverName) ||
                                   (m.UserChatFrom == receiverName && m.UserChatTo == senderName))
            .OrderBy(m => m.UserChatDate)
            .ToListAsync();

            return messages;
        }



        // defination for the user feedbacks
        public async Task AddFeedbackToUser(int userId, decimal receiverId, string message)
        {
            var newFeedback = new UserFeedbackTbl
            {
                FeedbackDate = DateTime.Now,
                FeedbackTime = DateTime.Now.TimeOfDay,
                FeedbackText = message,
                FeedbackFrom = _context.UsersTbls.Where(u => u.UserId == userId).Select(u => u.UserName).FirstOrDefault(),
                FeedbackTo = _context.UsersTbls.Where(u => u.UserId == receiverId).Select(u => u.UserName).FirstOrDefault()
            };
            await _context.UserFeedbackTbls.AddAsync(newFeedback);
        }

        
        public async Task<UserFeedbackTbl> GetFeedbackForUserAsync(int userId, decimal receiverId)
        {
            return await _context.UserFeedbackTbls
                .Where(f => f.FeedbackFrom == _context.UsersTbls.Where(u => u.UserId == userId).Select(u => u.UserName).FirstOrDefault()
                               && f.FeedbackTo == _context.UsersTbls.Where(u => u.UserId == receiverId).Select(u => u.UserName).FirstOrDefault())
                .FirstOrDefaultAsync();
        }

        public void DeleteFeedback(UserFeedbackTbl feedback)
        {
            _context.UserFeedbackTbls.Remove(feedback);
        }



        public async Task<IEnumerable<UserFeedbackTbl>> GetFeedbacksForUserAsync(decimal receiverId)
        {

            return await _context.UserFeedbackTbls
                .Where(f => f.FeedbackTo == _context.UsersTbls.Where(u => u.UserId == receiverId).Select(u => u.UserName).FirstOrDefault())
                .ToListAsync();
        }

        // defination for the post favouriate of the user

        public async Task AddPostToFaviourates(int userId , int postId)
        {
            var postFavouriate = new PostFaviourateTbl
            {
                PostId = postId,
                UserId = userId,
                Post = await GetPostForUserAsync(postId),
                User = await GetUserAsync(userId)
            };
            await _context.PostFaviourateTbls.AddAsync(postFavouriate);
        }

        public async Task<bool> PostFavouriateExistsAsync(int postId, int userId)
        {
            return await _context.PostFaviourateTbls.AnyAsync(p => p.PostId == postId && p.UserId == userId);
        }

        public async Task<IEnumerable<PostFaviourateTbl>> GetPostFavouriatesForUserAsync(int userId)
        {
            return await _context.PostFaviourateTbls.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<PostsTbl>> GetPostFavouriatesAsync(int userId)
        {
            return await _context.PostFaviourateTbls.Where(p => p.UserId == userId).Select(p => p.Post).ToListAsync();
        }

        public async Task<PostFaviourateTbl?> GetPostFavouriateForUserAsync(int postId, int userId)
        {
            return await _context.PostFaviourateTbls.Where(p => p.PostId == postId && p.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public void DeletePostFavouriate(PostFaviourateTbl postFavouriate)
        {
            _context.PostFaviourateTbls.Remove(postFavouriate);
        }


        public async Task<IEnumerable<UsersTbl>> GetUsersWhoFavouriatePostAsync(int postId)
        {
            return await _context.PostFaviourateTbls.Where(p => p.PostId == postId).Select(p => p.User).ToListAsync();
        }

        // defination for the user ban

        public async Task<UserBanTbl?> BanUserAsync(string? userNatId)
        {
            var user = await _context.UsersTbls.Where(u => u.UserNatId == userNatId).FirstOrDefaultAsync();
            if (user != null)
            {
                var userBan = new UserBanTbl
                {
                    UserBanNatId = user.UserNatId,
                };
                await _context.UserBanTbls.AddAsync(userBan);
                return userBan;
            }
            return null;
        }

        public async Task<UserBanTbl?> UnBanUserAsync(string? userNatId)
        {
            var user = await _context.UsersTbls.Where(u => u.UserNatId == userNatId).FirstOrDefaultAsync();
            if (user != null)
            {
                var userBan = await _context.UserBanTbls.Where(u => u.UserBanNatId == user.UserNatId).FirstOrDefaultAsync();
                if (userBan != null)
                {
                    _context.UserBanTbls.Remove(userBan);
                    return userBan;
                }
            }
            return null;
        }

        public async Task<IEnumerable<UserBanTbl>> GetBannedUsersAsync()
        {

            return await _context.UserBanTbls.ToListAsync();
        }

        public async Task<UserBanTbl?> GetBannedUserAsync(string? userNatId)
        {
            return await _context.UserBanTbls.Where(u => u.UserBanNatId == userNatId).FirstOrDefaultAsync();
        }

       
    }
}
