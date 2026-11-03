using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareClinicManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakePrescriptionConsultationOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Consultations_ConsultationId",
                table: "Prescriptions");

            migrationBuilder.AlterColumn<int>(
                name: "ConsultationId",
                table: "Prescriptions",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Consultations_ConsultationId",
                table: "Prescriptions",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Consultations_ConsultationId",
                table: "Prescriptions");

            migrationBuilder.AlterColumn<int>(
                name: "ConsultationId",
                table: "Prescriptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Consultations_ConsultationId",
                table: "Prescriptions",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
