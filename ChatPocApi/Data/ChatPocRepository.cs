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

        public async Task<User[]> GetAllUsersAsync(bool includeChats = false)
        {
            _logger.LogInformation($"Getting all Users");

            IQueryable<User> query = _context.Users;

            if (includeChats)
            {
                query = query
                  .Include(u => u.Chats)
                  .ThenInclude(c => c.Messages);
            }

            return await query.ToArrayAsync();
        }


        public async Task<User> GetUserAsync(string userName, bool includeChats = false)
        {
            _logger.LogInformation($"Getting user with name: {userName}");

            IQueryable<User> query = _context.Users;

            if (includeChats)
            {
                query = query
                  .Include(u => u.Chats)
                  .ThenInclude(c => c.Messages);
            }

            query = query.Where(u => u.Name == userName);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Chat> GetChatByUserAsync(string userName, string interlocutorName, bool includeMessages = false)
        {
            _logger.LogInformation($"Getting all Chats between {userName} and {interlocutorName}");

            IQueryable<Chat> query = _context.Chats;

            if (includeMessages)
            {
                query = query
                  .Include(c => c.Messages);
            }

            // Add Query
            query = query
              .Where(c => (c.User1.Name == userName && c.User2.Name == interlocutorName) || 
                          (c.User2.Name == userName && c.User1.Name == interlocutorName));

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Chat[]> GetChatsByUserAsync(string userName, bool includeMessages = false)
        {
            _logger.LogInformation($"Getting all Chats of {userName}");

            IQueryable<Chat> query = _context.Chats;

            if (includeMessages)
            {
                query = query
                  .Include(c => c.Messages);
            }

            // Add Query
            query = query
              .Where(c => c.User1.Name == userName || c.User2.Name == userName);

            return await query.ToArrayAsync();
        }

        public async Task<Message> GetLastMessageByUserAsync(string userName, string interlocutorName)
        {
            _logger.LogInformation($"Getting last message between {userName} and {interlocutorName}");

            IQueryable<Message> query = _context.Messages;

            // Add Query
            query = query
              .Where(c => (c.Sender.Name == userName && c.Recevier.Name == interlocutorName) ||
                          (c.Recevier.Name == userName && c.Sender.Name == interlocutorName))
              .OrderByDescending(c => c.MsgDate);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempitng to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
