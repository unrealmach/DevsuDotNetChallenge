using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account.Infrastructure.Adapters.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "account");

            migrationBuilder.CreateTable(
                name: "person",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Gender = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Age = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Identification = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "client",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Password = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Status = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_client_person_Id",
                        column: x => x.Id,
                        principalSchema: "account",
                        principalTable: "person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_account_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "account",
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "balance",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_balance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_balance_account_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "account",
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "history",
                schema: "account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_history_account_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "account",
                        principalTable: "account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_history_client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "account",
                        principalTable: "client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_ClientId",
                schema: "account",
                table: "account",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_balance_AccountId",
                schema: "account",
                table: "balance",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_client_ClientId",
                schema: "account",
                table: "client",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_history_AccountId",
                schema: "account",
                table: "history",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_history_ClientId",
                schema: "account",
                table: "history",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "balance",
                schema: "account");

            migrationBuilder.DropTable(
                name: "history",
                schema: "account");

            migrationBuilder.DropTable(
                name: "account",
                schema: "account");

            migrationBuilder.DropTable(
                name: "client",
                schema: "account");

            migrationBuilder.DropTable(
                name: "person",
                schema: "account");
        }
    }
}
