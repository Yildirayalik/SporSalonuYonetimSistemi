using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandevuYonetimSistemi.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainerContactFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Trainers",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Trainers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Trainers",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Trainers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainerId1",
                table: "TrainerAvailabilities",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainerAvailabilities_TrainerId1",
                table: "TrainerAvailabilities",
                column: "TrainerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerAvailabilities_Trainers_TrainerId1",
                table: "TrainerAvailabilities",
                column: "TrainerId1",
                principalTable: "Trainers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainerAvailabilities_Trainers_TrainerId1",
                table: "TrainerAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_TrainerAvailabilities_TrainerId1",
                table: "TrainerAvailabilities");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "TrainerId1",
                table: "TrainerAvailabilities");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Trainers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Trainers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
