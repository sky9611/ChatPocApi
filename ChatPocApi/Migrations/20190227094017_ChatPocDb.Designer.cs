﻿// <auto-generated />
using System;
using ChatPocApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatPocApi.Migrations
{
    [DbContext(typeof(ChatPocContext))]
    [Migration("20190227094017_ChatPocDb")]
    partial class ChatPocDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChatPocApi.Data.Entities.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Channels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Office"
                        },
                        new
                        {
                            Id = 2,
                            Name = "shiki_mikaya"
                        });
                });

            modelBuilder.Entity("ChatPocApi.Data.Entities.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ChannelId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("MsgDate");

                    b.Property<int?>("SenderUserId");

                    b.HasKey("MessageId");

                    b.HasIndex("ChannelId");

                    b.HasIndex("SenderUserId");

                    b.ToTable("Messages");

                    b.HasData(
                        new
                        {
                            MessageId = 1,
                            ChannelId = 1,
                            Content = "Hi, I'm Shiki",
                            MsgDate = new DateTime(2019, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SenderUserId = 1
                        },
                        new
                        {
                            MessageId = 2,
                            ChannelId = 1,
                            Content = "Hi, I'm Mikaya",
                            MsgDate = new DateTime(2019, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SenderUserId = 2
                        },
                        new
                        {
                            MessageId = 3,
                            ChannelId = 1,
                            Content = "Hi, I'm the boss",
                            MsgDate = new DateTime(2019, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SenderUserId = 3
                        },
                        new
                        {
                            MessageId = 4,
                            ChannelId = 2,
                            Content = "Hello Shiki",
                            MsgDate = new DateTime(2019, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SenderUserId = 2
                        },
                        new
                        {
                            MessageId = 5,
                            ChannelId = 2,
                            Content = "Hello Mikaya",
                            MsgDate = new DateTime(2019, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SenderUserId = 1
                        });
                });

            modelBuilder.Entity("ChatPocApi.Data.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("ProfilePicture");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
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
                });

            modelBuilder.Entity("ChatPocApi.Data.Entities.UserChannel", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ChannelId");

                    b.HasKey("UserId", "ChannelId");

                    b.HasIndex("ChannelId");

                    b.ToTable("UserChannels");

                    b.HasData(
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
                        },
                        new
                        {
                            UserId = 2,
                            ChannelId = 2
                        },
                        new
                        {
                            UserId = 1,
                            ChannelId = 2
                        });
                });

            modelBuilder.Entity("ChatPocApi.Data.Entities.Message", b =>
                {
                    b.HasOne("ChatPocApi.Data.Entities.Channel")
                        .WithMany("Messages")
                        .HasForeignKey("ChannelId");

                    b.HasOne("ChatPocApi.Data.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderUserId");
                });

            modelBuilder.Entity("ChatPocApi.Data.Entities.UserChannel", b =>
                {
                    b.HasOne("ChatPocApi.Data.Entities.Channel", "Channel")
                        .WithMany("UserChannels")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ChatPocApi.Data.Entities.User", "User")
                        .WithMany("UserChannels")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
