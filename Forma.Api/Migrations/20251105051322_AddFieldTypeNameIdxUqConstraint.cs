using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forma.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldTypeNameIdxUqConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FieldTypes_Name",
                table: "FieldTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FieldTypes_Name",
                table: "FieldTypes");
        }
    }
}
