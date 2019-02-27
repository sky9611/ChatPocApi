using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatPocApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ChatPocApi.Data
{
    public class ChatPocRepository : IChatPocRepository
    {
        private readonly ChatPocContext _context;
        private readonly ILogger<ChatPocContext> _logger;

        public ChatPocRepository(ChatPocContext context, ILogger<ChatPocContext> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<User[]> GetAllUsersAsync(bool includeChannels = false)
        {
            _logger.LogInformation($"Getting all Users");

            IQueryable<User> query = _context.Users;

            if (includeChannels)
            {
                query = query
                    .Include(u => u.UserChannels)
                        .ThenInclude(uc => uc.Channel)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.Sender);
            }

            return await query.ToArrayAsync();
        }

        public async Task<User> GetUserAsync(string userName, bool includeChannels = false)
        {
            _logger.LogInformation($"Getting user with name: {userName}");

            IQueryable<User> query = _context.Users
                .Where(u => u.Name == userName); ;

            if (includeChannels)
            {
                query = query
                    .Include(u => u.UserChannels)
                        .ThenInclude(uc => uc.Channel)
                            .ThenInclude(c => c.Messages)
                                .ThenInclude(m => m.Sender);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Channel[]> GetAllChannelsAsync(bool includeMessages = false, bool includeUsers = false)
        {
            _logger.LogInformation($"Getting all Users");

            IQueryable<Channel> query = _context.Channels;

            if (includeMessages)
            {
                query = query
                    .Include(c => c.Messages);
            }

            if(includeUsers)
                query = query
                    .Include(c => c.UserChannels)
                        .ThenInclude(uc => uc.User);

            return await query.ToArrayAsync();
        }

        public async Task<Channel> GetChannelByNameAsync(string channelName, bool includeMessages = false, bool includeUsers = false)
        {
            _logger.LogInformation($"Getting channel named {channelName}");

            IQueryable<Channel> query = _context.Channels;

            if (includeMessages)
            {
                query = query
                    .Include(c => c.Messages)
                        .ThenInclude(m => m.Sender);
            }

            if (includeUsers)
                query = query
                    .Include(c => c.UserChannels)
                        .ThenInclude(uc => uc.User);

            // Add Query
            query = query
              .Where(c => c.Name == channelName);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Channel[]> GetChannelsByUserAsync(string userName, bool includeMessages = false, bool includeUsers = false)
        {
            _logger.LogInformation($"Getting all channels of {userName}");

            IQueryable<Channel> query = _context.Channels
                .Where(c => c.UserChannels.Any(uc => uc.User.Name == userName));

            if (includeMessages)
            {
                query = query
                  .Include(c => c.Messages);
            }

            if (includeUsers)
                query = query
                    .Include(c => c.UserChannels)
                        .ThenInclude(uc => uc.User);


            return await query.ToArrayAsync();
        }

        public async Task<bool> CreateChannelAsync(string channelName, ICollection<string> userNames)
        {
            _logger.LogInformation($"Create a new channel {channelName}");
            var channel = new Channel { Name = channelName, UserChannels = new List<UserChannel>()};
            foreach (var userName in userNames)
            {
                User user = GetUserAsync(userName).Result;
                channel.UserChannels.Add(
                    new UserChannel
                    {
                        User = user,
                        Channel = channel
                    }
                );
            }
            Add<Channel>(channel);
            return await SaveChangesAsync();
        }

        public async Task<Message> GetMessageAsync(string senderName, DateTime msgDate)
        {
            _logger.LogInformation($"Get the message from {senderName} sent in {msgDate.ToString()}");
            IQueryable<Message> query = _context.Messages
                .Where(m => m.Sender.Name==senderName && m.MsgDate == msgDate)
                .Include(m => m.Sender);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> PostMessageAsync(string senderName, string channelName, string content, DateTime msgDate)
        {
            _logger.LogInformation($"Post a new message from {senderName} to {channelName}");
            var message = new Message { Content = content, MsgDate = msgDate };
            User user = GetUserAsync(senderName).Result;
            var channel = _context.Channels
                .Include(c => c.Messages)
                .Single(c => c.Name == channelName);
            message.Sender = user;
            Add<Message>(message);
            channel.Messages.Add(message);

            return await SaveChangesAsync();
        }

        //public async Task<bool> DeleteChannelAsync(string channelName)
        //{
        //    _logger.LogInformation($"Create a new channel {channelName}");

        //    return await SaveChangesAsync();
        //}

        public async Task<Message> GetLastMessageByChannelAsync(string channelName)
        {
            _logger.LogInformation($"Getting last message of channel {channelName}");

            Task<ICollection<Message>> taskMessages = _context.Channels
                .Where(c => c.Name == channelName)
                .Select(c => c.Messages).FirstOrDefaultAsync();

            taskMessages.Result.OrderByDescending(m => m.MsgDate);

            Task<Message> res = Task<Message>.Factory.StartNew(() =>
            {
                return taskMessages.Result.FirstOrDefault();
            });

            return await res;
        }


        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempitng to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
