using sakeny.DbContexts;
using sakeny.Entities;

namespace sakeny.Services
{
    public class NotificationInfoReopsitory
    {
        private readonly HOUSE_RENT_DBContext _context;

        public NotificationInfoReopsitory(HOUSE_RENT_DBContext context)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
        }
        public async Task SendNotificationAsync(decimal userId, string message)
        {
            var notification = new NotificationTbl
            {
                UserId = userId,
                Message = message,
                Timestamp = DateTime.Now
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            // Add logic here to send the FCM notification to the user with the provided message.
            // This will involve interacting with the FCM service.
        }
    }
}
