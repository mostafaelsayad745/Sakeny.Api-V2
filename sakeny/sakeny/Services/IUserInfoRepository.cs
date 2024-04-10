using sakeny.Entities;
using sakeny.Models.PostDtos;

namespace sakeny.Services
{
    public interface IUserInfoRepository
    {
        Task<bool> UserExistsAsync(int userId);

        Task<IEnumerable<UsersTbl>> GetUsersAsync();

        Task<(IEnumerable<UsersTbl>, PaginationMetadata)> GetUsersAsync
            (string? name, string? SearchQuery, int pageNumber, int pageSize);

        Task<UsersTbl?> GetUserAsync(string userName);

        Task<UsersTbl?> GetUserAsync(int userId, bool includePostFeedbacks);
        Task<UsersTbl?> GetUserAsync(int userId);

        Task AddUserAsync(UsersTbl user);
        Task UpdateUserAsync(UsersTbl user);
        void DeleteUserAsync(UsersTbl user);
        Task<bool> SaveChangesAsync();


        Task<bool> checkEmailNotRepated(UsersTbl user);

        Task<UsersTbl?> validateUser(string email, string password);


        // defination for the post feedbacks of the user

        Task<bool> PostFeedbackExistsAsync(int userId, int postId, DateTime? datetime);
        Task<IEnumerable<PostFeedbackTbl>> GetPostFeedbacksForUserAsync( int postId);
        Task<PostFeedbackTbl?> GetPostFeedbackForUserAsync(int userId,int postId, int postFeedbackId);

        Task AddPostFeedbackForUserAsync(int userId,int postId, PostFeedbackTbl postFeedbackTbl);
        void DeletePostFeedback(PostFeedbackTbl postFeedbackTbl);


        // defination for the posts of the user
        Task<bool> PostExistsAsync(int userId, int postId);
        Task<bool> PostExistsAsync(int postId);
        Task<(IEnumerable<PostsTbl>, PaginationMetadata)> GetPostsForUserAsync(int userId , string? name , 
            string? seacrchQuery , int pageNumber , int pageSize);
        Task<PostsTbl?> GetPostForUserAsync(int userId, int postId );
        Task<PostsTbl?> GetPostForUserAsync(int postId);
        Task<PostsTbl?> GetPostForUserAsyncForUpdate(int userId, int postId);
        Task AddPostForUserAsync(int userId, PostsTbl postTbl);
        void DeletePost(PostsTbl postTbl);
        Task<IEnumerable<PostsTbl>> GetAllPostsAsync();
        Task<IEnumerable<PostsTbl>> SearchForPostAsync(PostForSerchDto postForSerchDto);




        // defination for the features of the post
        Task<bool> FeaturesExistsAsync(int postId, int featuresId);
        Task<IEnumerable<FeaturesTbl>> GetFeaturesForPostAsync(int postId);
        Task<FeaturesTbl?> GetFeatureForPostAsync(int postId, int featuresId);
        Task AddFeatureForPostAsync(int postId, FeaturesTbl featureTbl);
        void DeleteFeature(FeaturesTbl featureTbl);


        // defination for the pictures of the post

        Task<bool> PictureExistsAsync(int postId, int pictureId);
        Task<IEnumerable<PostPicTbl>> GetPicturesForPostAsync(int postId);
        Task<PostPicTbl?> GetPictureForPostAsync(int postId, int pictureId);
        Task AddPictureForPostAsync(PostPicTbl pictureTbl);
        void DeletePicture(PostPicTbl pictureTbl);

        // defination for the notifications of the user
        Task<bool> NotificationExistsAsync(int userId, int notificationId);
        Task<IEnumerable<NotificationTbl>> GetNotificationsForUserAsync(int userId);
        Task<NotificationTbl> GetNotificationForUserAsync(int userId, int notificationId);
        Task AddNotificationForUserAsync(int userId, NotificationTbl notificationTbl);
        Task AddNotificationForUserAsync(NotificationTbl notificationTbl);
        void DeleteNotification(NotificationTbl notificationTbl);

        // defination for the chat of the user
        Task SendMessageAsync(int userId, decimal receiverId, string message);
        Task<IEnumerable<UserChatTbl>> GetChatHistoryAsync(string senderName, string receiverName);

        // defination for the user feedbacks
        
        Task AddFeedbackToUser(int userId, decimal receiverId, string message);
        Task<UserFeedbackTbl> GetFeedbackForUserAsync(int userId, decimal receiverId);
        Task<IEnumerable<UserFeedbackTbl>> GetFeedbacksForUserAsync(decimal receiverId);
        void DeleteFeedback (UserFeedbackTbl feedback);

        // defination for the post favouriate of the user
        Task<bool> PostFavouriateExistsAsync(int postId, int userId);
        Task<IEnumerable<PostFaviourateTbl>> GetPostFavouriatesForUserAsync(int userId);
        Task<PostFaviourateTbl?> GetPostFavouriateForUserAsync(int postId, int userId);
        Task AddPostToFaviourates(int userId , int postId);
        void DeletePostFavouriate(PostFaviourateTbl postFavouriateTbl);
        Task<IEnumerable<PostsTbl>> GetPostFavouriatesAsync(int userId);

        Task<IEnumerable<UsersTbl>> GetUsersWhoFavouriatePostAsync(int postId);

        // defination for the user ban

        Task<UserBanTbl?> BanUserAsync(string userNatId);
        Task<UserBanTbl?> UnBanUserAsync(string userNatId);
        Task<IEnumerable<UserBanTbl>> GetBannedUsersAsync();
        Task<UserBanTbl?> GetBannedUserAsync(string userNatId);
    }
}
