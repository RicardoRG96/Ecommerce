using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Database.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_FK_Municipality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipality_Region_MunicipalityId",
                table: "Municipality");

            migrationBuilder.AlterColumn<long>(
                name: "MunicipalityId",
                table: "Municipality",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Municipality_RegionId",
                table: "Municipality",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipality_Region_RegionId",
                table: "Municipality",
                column: "RegionId",
                principalTable: "Region",
                principalColumn: "RegionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Municipality_Region_RegionId",
                table: "Municipality");

            migrationBuilder.DropIndex(
                name: "IX_Municipality_RegionId",
                table: "Municipality");

            migrationBuilder.AlterColumn<long>(
                name: "MunicipalityId",
                table: "Municipality",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Municipality_Region_MunicipalityId",
                table: "Municipality",
                column: "MunicipalityId",
                principalTable: "Region",
                principalColumn: "RegionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
