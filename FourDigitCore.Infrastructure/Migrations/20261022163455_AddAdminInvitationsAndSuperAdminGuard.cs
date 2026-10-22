using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourDigitCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminInvitationsAndSuperAdminGuard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSuperAdmin",
                table: "admin_users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "admin_invitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    AssignedRole = table.Column<string>(type: "text", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsAccepted = table.Column<bool>(type: "boolean", nullable: false),
                    AcceptedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    InvitedByAdminId = table.Column<Guid>(type: "uuid", nullable: false),
                    InvitedByAdminName = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admin_invitations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_admin_invitations_Email",
                table: "admin_invitations",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_admin_invitations_TokenHash",
                table: "admin_invitations",
                column: "TokenHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_invitations");

            migrationBuilder.DropColumn(
                name: "IsSuperAdmin",
                table: "admin_users");
        }
    }
}
