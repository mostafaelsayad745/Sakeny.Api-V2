using Microsoft.EntityFrameworkCore;
using sakeny.DbContexts;
using sakeny.Entities;

namespace sakeny.Services.NotificationRepo
{
    public interface INotificationRepo
    {
        Task AddNotification(NotificationTbl notification);
        Task<IEnumerable<NotificationTbl>> GetNotificationsForUser(decimal userId);
    }

    public class NotificationRepo : INotificationRepo
    {
        private readonly HOUSE_RENT_DBContext _context;

        public NotificationRepo(HOUSE_RENT_DBContext context)
        {
            _context = context;
        }

        public async Task AddNotification(NotificationTbl notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationTbl>> GetNotificationsForUser(decimal userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Timestamp)
                .ToListAsync();
        }
    }


}
