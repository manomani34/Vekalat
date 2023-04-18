using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vekalat.Infrastructure.Data.Migrations.Migrations
{
    public partial class editblogrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blogs_BlogSubjectId",
                table: "Blogs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2023, 4, 14, 14, 11, 26, 731, DateTimeKind.Local).AddTicks(9441));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2023, 4, 14, 14, 11, 26, 731, DateTimeKind.Local).AddTicks(9478));

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogSubjectId",
                table: "Blogs",
                column: "BlogSubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Blogs_BlogSubjectId",
                table: "Blogs");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2023, 4, 13, 23, 14, 55, 98, DateTimeKind.Local).AddTicks(6469));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2023, 4, 13, 23, 14, 55, 98, DateTimeKind.Local).AddTicks(6520));

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogSubjectId",
                table: "Blogs",
                column: "BlogSubjectId",
                unique: true);
        }
    }
}
