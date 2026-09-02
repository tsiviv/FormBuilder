using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormBuilder.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OptionsJson",
                table: "FormFields",
                type: "TEXT",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionsJson",
                table: "FormFields");
        }
    }
}
