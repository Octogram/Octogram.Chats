using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Octogram.Chats.Infrastructure.Migrations.Migrations
{
    public partial class DeleteMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Members_OwnedId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Members_MemberId",
                table: "Chats");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 255, nullable: true),
                    UsernameId = table.Column<string>(maxLength: 64, nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: true),
                    Email = table.Column<string>(maxLength: 320, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Accounts_OwnedId",
                table: "Chats",
                column: "OwnedId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Accounts_MemberId",
                table: "Chats",
                column: "MemberId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Accounts_OwnedId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Accounts_MemberId",
                table: "Chats");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Members_OwnedId",
                table: "Chats",
                column: "OwnedId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Members_MemberId",
                table: "Chats",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
