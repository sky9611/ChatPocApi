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
    public class ChatPocContext : DbContext
    {
        private readonly IConfiguration _config;

        public DbSet<User> Users { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserChannel> UserChannels { get; set; }

        public ChatPocContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("ChatPoc"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserChannel>()
                .HasKey(uc => new { uc.UserId, uc.ChannelId});

            modelBuilder.Entity<UserChannel>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserChannels)
                .HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<UserChannel>()
                .HasOne(uc => uc.Channel)
                .WithMany(c => c.UserChannels)
                .HasForeignKey(uc => uc.ChannelId);

            modelBuilder.Entity<User>()
                .HasData(
                    new
                    {
                        UserId = 1,
                        Name = "Shiki",
                        ProfilePicture = "https://i.pinimg.com/236x/c6/6b/fb/c66bfb865e8eb898c7b1e6df076d7442--garden-fate.jpg"
                    },
                    new
                    {
                        UserId = 2,
                        Name = "Mikiya",
                        ProfilePicture = "https://vignette.wikia.nocookie.net/typemoon/images/1/13/Mikiya_Kokutou_old_sketch.png/revision/latest/scale-to-width-down/96?cb=20131005113140"
                    },
                    new
                    {
                        UserId = 3,
                        Name = "Toko",
                        ProfilePicture = "https://img.moegirl.org/common/5/51/Cangqichengzi_03.JPG"
                });

            modelBuilder.Entity<Channel>()
                .HasData(
                    new
                    {
                        Id = 1,
                        Name = "伽蓝の堂"
                    }
                );

            modelBuilder.Entity<UserChannel>()
                .HasData(
                    new
                    {
                        UserId = 1,
                        ChannelId = 1
                    },
                    new
                    {
                        UserId = 2,
                        ChannelId = 1
                    },
                    new
                    {
                        UserId = 3,
                        ChannelId = 1
                    }
                );

            modelBuilder.Entity<Message>()
                .HasData(
                    new
                    {
                        MessageId = 1,
                        SenderUserId = 1,
                        ChannelId = 1,
                        Content = "Hi, I'm Shiki",
                        MsgDate = new DateTime(2019, 2, 26)
                    },
                    new
                    {
                        MessageId = 2,
                        SenderUserId = 2,
                        ChannelId = 1,
                        Content = "Hi, I'm Mikaya",
                        MsgDate = new DateTime(2019, 2, 26)
                    },
                    new
                    {
                        MessageId = 3,
                        SenderUserId = 3,
                        ChannelId = 1,
                        Content = "Hi, I'm the boss",
                        MsgDate = new DateTime(2019, 2, 26)
                    }
                );

        }
    }
}
