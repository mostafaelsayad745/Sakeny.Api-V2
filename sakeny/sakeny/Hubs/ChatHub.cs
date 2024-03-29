using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public ChatHub(HOUSE_RENT_DBContext context , IUserInfoRepository userInfoRepository , IMapper mapper)
        {
            _context = context;
            _userInfoRepository = userInfoRepository;
            _mapper = mapper;
        }

        public async Task SendMessage(string senderName, string receiverName, string message)
        {

            

            


            if (string.IsNullOrEmpty(message) || message.Length > 500) // check message length
            {
                // handle invalid message
                return;
            }

            if (senderName == receiverName) // check if sender and receiver are the same
            {
                // handle same sender and receiver
                return;
            }

            var sender = await _userInfoRepository.GetUserAsync(senderName);
            if (sender == null)
            {
                return;
            }

            var receiver = await _userInfoRepository.GetUserAsync(receiverName);
            if (receiver == null)
            {
                return;
            }

            var userChat = new UserChatTbl
            {
                UserChatFrom = senderName,
                UserChatTo = receiverName,
                UserChatText = message,
                UserChatDate = DateTime.UtcNow
            };

            _context.UserChatTbls.Add(userChat);
            await _context.SaveChangesAsync();

            await Clients.User(receiverName).SendAsync("ReceiveMessage", senderName, message);

            
        }

        public async Task SendChatHistory(string user1, string user2)
        {
            var chatHistory = await GetChatHistory(user1, user2);
            Console.WriteLine($"SendChatHistory called with {user1} and {user2}. Sending {chatHistory.Count()} messages.");

            await Clients.All.SendAsync("ReceiveChatHistory", chatHistory);
        }


        public async Task<IEnumerable<UserChatTbl>> GetChatHistory(string user1, string user2)
        {
            return await _context.UserChatTbls
                .Where(chat =>
                    (chat.UserChatFrom == user1 && chat.UserChatTo == user2) ||
                    (chat.UserChatFrom == user2 && chat.UserChatTo == user1))
                .OrderBy(chat => chat.UserChatDate)
                .ToListAsync();
           // return _mapper.Map<IEnumerable<ChatDto>>(chat);

        }

        

    }

    public class ChatDto
    {
        public string UserChatFrom { get; set; }
        public string UserChatTo { get; set; }
        public string UserChatText { get; set; }
        public DateTime UserChatDate { get; set; }
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
