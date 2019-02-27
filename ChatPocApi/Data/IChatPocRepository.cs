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
        Task<User[]> GetAllUsersAsync(bool includeChannels = false);
        Task<User> GetUserAsync(string userName, bool includeChannels = false);

        // Channels
        Task<Channel[]> GetAllChannelsAsync(bool includeMessages = false, bool includeUsers = false);
        Task<Channel> GetChannelByNameAsync(string channelName, bool includeMessages = false, bool includeUsers = false);
        Task<Channel[]> GetChannelsByUserAsync(string userName, bool includeMessages = false, bool includeUsers = false);
        Task<bool> CreateChannelAsync(string channelName, ICollection<string> users);

        // Messages
        Task<Message> GetMessageAsync(string senderName, DateTime msgDate);
        Task<Message> GetLastMessageByChannelAsync(string channelName);
        Task<bool> PostMessageAsync(string senderName, string channelName, string content, DateTime msgDate);
    }
}
