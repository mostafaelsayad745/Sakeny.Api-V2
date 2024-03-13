using sakeny.Entities;
using System.Collections.Concurrent;

namespace sakeny.Models.ChatDtos
{
    public class sharedDb
    {
        private readonly ConcurrentDictionary<string, UserChatTbl> _connection = new();
        public ConcurrentDictionary<string, UserChatTbl> connection => _connection;

    }
}
