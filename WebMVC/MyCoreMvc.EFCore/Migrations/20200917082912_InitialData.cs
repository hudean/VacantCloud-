using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VaCant.EFCore.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Description", "PermissionName", "Value" },
                values: new object[,]
                {
                    { 1L, "系统权限", "Pages.Tenants", "" },
                    { 2L, "系统权限", "Pages.Users", "" },
                    { 3L, "系统权限", "Pages.Roles", "" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "Description", "DisplayName", "IsDelete", "LastModificationTime", "LastModifierUserId", "NormalizedName", "RoleName" },
                values: new object[] { 1L, new DateTime(2020, 9, 17, 16, 29, 11, 625, DateTimeKind.Local).AddTicks(9575), null, null, null, "Admin", "Admin", false, null, null, "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreationTime", "CreatorUserId", "Email", "IsActive", "IsDelete", "LastLoginErrorDateTime", "LoginErrorTimes", "Password", "PasswordSalt", "PhoneNum", "UserName" },
                values: new object[] { 1L, "深圳市宝安区", new DateTime(2020, 9, 17, 16, 29, 11, 622, DateTimeKind.Local).AddTicks(8250), null, "1468553034@qq.com", true, false, null, 0, "123qwe", "45be", "13538631840", "admin" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1L, 1L, 1L },
                    { 2L, 2L, 1L },
                    { 3L, 3L, 1L }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleId", "RoleId1", "UserId" },
                values: new object[] { 1L, 1, null, 1L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);
        }
    }
}
