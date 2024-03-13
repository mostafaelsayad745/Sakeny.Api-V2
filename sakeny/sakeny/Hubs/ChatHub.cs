using Microsoft.AspNetCore.SignalR;
using sakeny.DbContexts;
using sakeny.Entities;
using sakeny.Models.ChatDtos;
using sakeny.Services;

namespace sakeny.Hubs
{

    public class ChatHub : Hub
    {
        private readonly HOUSE_RENT_DBContext _context;
        private readonly IUserInfoRepository _userInfoRepository;

        public ChatHub(HOUSE_RENT_DBContext context , IUserInfoRepository userInfoRepository)
        {
            _context = context;
            _userInfoRepository = userInfoRepository;
        }

        public async Task SendMessage(string senderName, string receiverName, string message)
        {
            var (sender,pagniationSender) = await _userInfoRepository.GetUsersAsync(string.Empty,senderName,1,10);
            if (sender == null)
            {
                return;
            }
            var (receiver, pagniationReceiver) = await _userInfoRepository.GetUsersAsync(string.Empty, receiverName, 1, 10);
            if (receiver == null)
            {
                return;
            }


            await Clients.User(receiverName).SendAsync("ReceiveMessage", senderName, message);

            var userChat = new UserChatTbl
            {
                UserChatFrom = senderName,
                UserChatTo = receiverName,
                UserChatText = message,
                UserChatDate = DateTime.UtcNow
            };

            _context.UserChatTbls.Add(userChat);
            await _context.SaveChangesAsync();
        }
    }


    //public class ConnUsers
    //{
    //    public string userFrom { get; set; }
    //    public string userTo { get; set; }
    //}
    //public class ChatHub :Hub
    //{
    //    private readonly sharedDb _sharedDb;

    //    public ChatHub(sharedDb sharedDb)
    //    {
    //        _sharedDb = sharedDb;
    //    }

    //    public async Task JoinSpecificChatRoom (ConnUsers conn)
    //    {
    //        await Groups.AddToGroupAsync(Context.ConnectionId, conn.userTo);




    //        await Clients.Group(conn.userTo).SendAsync("JoinSpecificChatRoom", "adimn",
    //            $"{conn.userTo} has joined to the chat room");
    //    }




    //    //public async Task SendMessage (string message)
    //    //{
    //    //    if(_sharedDb.connection.TryGetValue(Context.ConnectionId, out var user))
    //    //    {
    //    //        await Clients.Group(user.userTo).SendAsync("ReceiveMessage", user.userFrom, message);
    //    //    }
    //    //}


    //    //public async Task SendMessage(string userFromId, string userToId, string message)
    //    //{
    //    //    var chatMessage = new UserChatTbl
    //    //    {
    //    //        // Assign values from parameters and any other necessary properties
    //    //        UserChatFrom = userFromId,
    //    //        UserChatTo = userToId,
    //    //        UserChatText = message,
    //    //        UserChatDate = DateTime.UtcNow, // or your specific timezone
    //    //                                        // Other properties like UserChatTime if needed
    //    //    };

    //    //    // Save chatMessage to database
    //    //    // dbContext.UserChatTbls.Add(chatMessage);
    //    //    // await dbContext.SaveChangesAsync();

    //    //    await Clients.User(userToId).SendAsync("ReceiveMessage", userFromId, message);
    //    //}

    //}
}
