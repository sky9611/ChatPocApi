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
                  .ThenInclude(uc => uc.Channel);
            }

            return await query.ToArrayAsync();
        }


        public async Task<User> GetUserAsync(string userName, bool includeChannels = false)
        {
            _logger.LogInformation($"Getting user with name: {userName}");

            IQueryable<User> query = _context.Users
                .Where(u => u.Name == userName);

            if (includeChannels)
            {
                query = query
                  .Include(u => u.UserChannels
                                    .Where(uc => uc.User.Name == userName)
                                    .Select(uc => uc.Channel));
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Channel> GetChannelByNameAsync(string channelName, bool includeMessages = false)
        {
            _logger.LogInformation($"Getting channel named {channelName}");

            IQueryable<Channel> query = _context.Channels;

            if (includeMessages)
            {
                query = query
                  .Include(c => c.Messages);
            }

            // Add Query
            query = query
              .Where(c => c.Name == channelName);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Channel[]> GetChannelsByUserAsync(string userName, bool includeMessages = false)
        {
            _logger.LogInformation($"Getting all channels of {userName}");

            IQueryable<Channel> query = _context.UserChannels
                .Where(uc => uc.User.Name == userName)
                .Select(uc => uc.Channel)
                .Distinct();

            if (includeMessages)
            {
                query = query
                  .Include(c => c.Messages);
            }

            return await query.ToArrayAsync();
        }

        public async Task<Message> GetLastMessageByChannelAsync(string channelName)
        {
            _logger.LogInformation($"Getting last message of channel {channelName}");

            Task<ICollection<Message>> taskMessages = _context.Channels
                .Where(c => c.Name == channelName)
                .Select(c => c.Messages).FirstOrDefaultAsync() ;

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
