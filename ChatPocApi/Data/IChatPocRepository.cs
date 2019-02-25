using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatPocApi.Data.Entities;

namespace ChatPocApi.Data
{
    public interface IChatPocRepository
    {
        // General
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // Users
        Task<User[]> GetAllUsersAsync(bool includeChats = false);
        Task<User> GetUserAsync(string userName, bool includeChats = false);

        // Chats
        Task<Chat[]> GetChatsByUserAsync(string userName, bool includeMessages = false);
        Task<Chat> GetChatByUserAsync(string userName, string interlocutorName, bool includeMessages = false);

        // Messages
        Task<Message> GetLastMessageByUserAsync(string userName, string interlocutorName);
    }
}
