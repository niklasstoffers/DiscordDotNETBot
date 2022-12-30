using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hainz.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    SendDMUponBan = table.Column<bool>(type: "boolean", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GuildUserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    HasElevatedPrivileges = table.Column<bool>(type: "boolean", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChannelId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    GuildId = table.Column<int>(type: "integer", nullable: false),
                    ChannelType = table.Column<int>(type: "integer", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Added = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildChannels_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildGuildUser",
                columns: table => new
                {
                    GuildsId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildGuildUser", x => new { x.GuildsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GuildGuildUser_GuildUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "GuildUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildGuildUser_Guilds_GuildsId",
                        column: x => x.GuildsId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildChannels_GuildId",
                table: "GuildChannels",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GuildGuildUser_UsersId",
                table: "GuildGuildUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildChannels");

            migrationBuilder.DropTable(
                name: "GuildGuildUser");

            migrationBuilder.DropTable(
                name: "GuildUsers");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
