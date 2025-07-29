using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortalReal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatefk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_ApplicationUserId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_ApplicationUserId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "JobApplications");

            migrationBuilder.AlterColumn<string>(
                name: "JobseekerId",
                table: "JobApplications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobseekerId",
                table: "JobApplications",
                column: "JobseekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_JobseekerId",
                table: "JobApplications",
                column: "JobseekerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_AspNetUsers_JobseekerId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_JobseekerId",
                table: "JobApplications");

            migrationBuilder.AlterColumn<string>(
                name: "JobseekerId",
                table: "JobApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "JobApplications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_ApplicationUserId",
                table: "JobApplications",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_AspNetUsers_ApplicationUserId",
                table: "JobApplications",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
