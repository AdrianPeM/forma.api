using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forma.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateFieldTypesEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FieldTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FieldTypes",
                columns: new[] { "Id", "FieldTypeId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Text" },
                    { 2, 2, "Paragraph" },
                    { 3, 3, "Number" },
                    { 4, 4, "Checkbox" },
                    { 5, 5, "Toggle" },
                    { 6, 6, "Dropdown" },
                    { 7, 7, "Date" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldTypes");
        }
    }
}
