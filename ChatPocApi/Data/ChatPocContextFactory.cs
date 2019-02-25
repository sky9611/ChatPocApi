using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ChatPocApi.Data
{
    public class CampContextFactory : IDesignTimeDbContextFactory<ChatPocContext>
    {
        public ChatPocContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            return new ChatPocContext(new DbContextOptionsBuilder<ChatPocContext>().Options, config);
        }
    }
}
