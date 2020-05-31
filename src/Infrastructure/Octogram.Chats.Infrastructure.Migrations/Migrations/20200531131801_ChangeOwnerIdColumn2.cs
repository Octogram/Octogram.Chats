using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Octogram.Chats.Infrastructure.Migrations.Migrations
{
    public partial class ChangeOwnerIdColumn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Accounts_OwnedId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_OwnedId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "OwnedId",
                table: "Chats");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Chats",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_OwnerId",
                table: "Chats",
                column: "OwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Accounts_OwnerId",
                table: "Chats",
                column: "OwnerId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Accounts_OwnerId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_OwnerId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Chats");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnedId",
                table: "Chats",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_OwnedId",
                table: "Chats",
                column: "OwnedId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Accounts_OwnedId",
                table: "Chats",
                column: "OwnedId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
