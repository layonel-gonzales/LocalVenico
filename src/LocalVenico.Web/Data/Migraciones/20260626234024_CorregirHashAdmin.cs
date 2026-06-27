using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalVenico.Web.Data.Migraciones
{
    /// <inheritdoc />
    public partial class CorregirHashAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "usuario_id",
                keyValue: 1,
                column: "password_hash",
                value: "$2b$12$w7U5s1ltCJlrCTawN.BLg.VDFKd3Ag8cXD03RrmdSVYlTTC5/bk5O");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "usuarios",
                keyColumn: "usuario_id",
                keyValue: 1,
                column: "password_hash",
                value: "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBPj7oFJDKHbPm");
        }
    }
}
