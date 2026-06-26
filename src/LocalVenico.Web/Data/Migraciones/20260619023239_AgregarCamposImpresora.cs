using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalVenico.Web.Data.Migraciones
{
    /// <inheritdoc />
    public partial class AgregarCamposImpresora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnchoPapelMm",
                table: "config_impresora",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CaracteresPorLinea",
                table: "config_impresora",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CopiasImpresion",
                table: "config_impresora",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "CorteAutomatico",
                table: "config_impresora",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LogoTexto",
                table: "config_impresora",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MensajePieBoleta",
                table: "config_impresora",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnchoPapelMm",
                table: "config_impresora");

            migrationBuilder.DropColumn(
                name: "CaracteresPorLinea",
                table: "config_impresora");

            migrationBuilder.DropColumn(
                name: "CopiasImpresion",
                table: "config_impresora");

            migrationBuilder.DropColumn(
                name: "CorteAutomatico",
                table: "config_impresora");

            migrationBuilder.DropColumn(
                name: "LogoTexto",
                table: "config_impresora");

            migrationBuilder.DropColumn(
                name: "MensajePieBoleta",
                table: "config_impresora");
        }
    }
}
