using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatPocApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChatPocApi.Data
{
    public class ChatPocContext: DbContext
    {
        private readonly IConfiguration _config;

        public DbSet<User> Users { get; set; }

        public ChatPocContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }
    }
}
