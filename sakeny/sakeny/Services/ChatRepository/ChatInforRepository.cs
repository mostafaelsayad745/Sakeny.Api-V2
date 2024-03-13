using Microsoft.EntityFrameworkCore;
using sakeny.DbContexts;
using sakeny.Entities;

namespace sakeny.Services
{
    public class chatInfoRepository : IChatInfoRepository
    {
        //private readonly HOUSE_RENT_DBContext _context;

        //public chatInfoRepository(HOUSE_RENT_DBContext context)
        //{
        //    _context = context;
        //}

        //public async Task<IEnumerable<UserChatTbl>> GetChatHistoryAsync(int userId1, int userId2)
        //{
        //    var messages = await _context.UserChatTbls
        //    .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
        //                (m.SenderId == userId2 && m.ReceiverId == userId1))
        //    .OrderBy(m => m.UserChatDate)
        //    .ToListAsync();

        //    return messages;
        //}

        //public async Task SendMessageAsync(int senderId, int receiverId, string message)
        //{
        //    var newMessage = new UserChatTbl
        //    {
        //        UserChatDate = DateTime.Now,
        //        UserChatTime = DateTime.Now.TimeOfDay,
        //        UserChatType = "text",
        //        UserChatText = message,
        //        SenderId = senderId,
        //        ReceiverId = receiverId
        //    };
        //    await _context.UserChatTbls.AddAsync(newMessage);
        //    await _context.SaveChangesAsync();

        //}
    }
}
