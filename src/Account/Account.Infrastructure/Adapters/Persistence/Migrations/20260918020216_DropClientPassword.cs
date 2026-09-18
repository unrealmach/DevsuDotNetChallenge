using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account.Infrastructure.Adapters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropClientPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                schema: "account",
                table: "client");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "account",
                table: "client",
                type: "character varying(160)",
                maxLength: 160,
                nullable: false,
                defaultValue: "");
        }
    }
}
