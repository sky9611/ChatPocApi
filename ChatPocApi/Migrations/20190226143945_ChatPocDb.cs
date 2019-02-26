using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatPocApi.Migrations
{
    public partial class ChatPocDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ProfilePicture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderUserId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    MsgDate = table.Column<DateTime>(nullable: false),
                    ChannelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserChannels",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ChannelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChannels", x => new { x.UserId, x.ChannelId });
                    table.ForeignKey(
                        name: "FK_UserChannels_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChannels_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "伽蓝の堂" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Name", "ProfilePicture" },
                values: new object[,]
                {
                    { 1, "Shiki", "https://i.pinimg.com/236x/c6/6b/fb/c66bfb865e8eb898c7b1e6df076d7442--garden-fate.jpg" },
                    { 2, "Mikiya", "https://vignette.wikia.nocookie.net/typemoon/images/1/13/Mikiya_Kokutou_old_sketch.png/revision/latest/scale-to-width-down/96?cb=20131005113140" },
                    { 3, "Toko", "https://img.moegirl.org/common/5/51/Cangqichengzi_03.JPG" }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "MessageId", "ChannelId", "Content", "MsgDate", "SenderUserId" },
                values: new object[,]
                {
                    { 1, 1, "Hi, I'm Shiki", new DateTime(2019, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 1, "Hi, I'm Mikaya", new DateTime(2019, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 1, "Hi, I'm the boss", new DateTime(2019, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });

            migrationBuilder.InsertData(
                table: "UserChannels",
                columns: new[] { "UserId", "ChannelId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChannelId",
                table: "Messages",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderUserId",
                table: "Messages",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChannels_ChannelId",
                table: "UserChannels",
                column: "ChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "UserChannels");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
