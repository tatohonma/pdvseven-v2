using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace a7D.PDV.Ativacao.API.Migrations
{
    /// <inheritdoc />
    public partial class alter_tax_id_to_CpfCnpj_to_Client_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxId",
                table: "clients",
                newName: "CpfCnpj");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CpfCnpj",
                table: "clients",
                newName: "TaxId");
        }
    }
}
