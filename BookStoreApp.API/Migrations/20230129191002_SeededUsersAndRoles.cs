using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8673270e-8783-5e1a-b0c1-98fb2373c8f9", null, "User", "USER" },
                    { "c837465e-2435-8w4p-d1c4-12tw8620l2t1", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "9iuyg654-2315-9u7y-s6c2-87devc61v4kl", 0, "ccc913c7-3c9c-4b4e-914a-87515b42675f", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAELrIKEgvTjg6kBLGPSbnR3Utz47+4SmI4CO6ggl1PcDkUemaKgfhwa9tdDTTjNY1vw==", null, false, "86151234-73a3-417a-888f-e5662d28384a", false, "user@bookstore.com" },
                    { "y63nfyte-v6dr-284f-k8u9-1qfdr456vnhg", 0, "ce3d103a-80a4-403d-b152-a99771e3080a", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEFVqfhRPbI+613i23/NcQ0XWphgSeLEGVLqT8c/tjrobAfFqjYK51JoqpLhqpCmbAQ==", null, false, "5d4a8578-ebb5-46dc-81b7-52d0085d0aaa", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "8673270e-8783-5e1a-b0c1-98fb2373c8f9", "9iuyg654-2315-9u7y-s6c2-87devc61v4kl" },
                    { "c837465e-2435-8w4p-d1c4-12tw8620l2t1", "y63nfyte-v6dr-284f-k8u9-1qfdr456vnhg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8673270e-8783-5e1a-b0c1-98fb2373c8f9", "9iuyg654-2315-9u7y-s6c2-87devc61v4kl" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c837465e-2435-8w4p-d1c4-12tw8620l2t1", "y63nfyte-v6dr-284f-k8u9-1qfdr456vnhg" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8673270e-8783-5e1a-b0c1-98fb2373c8f9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c837465e-2435-8w4p-d1c4-12tw8620l2t1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9iuyg654-2315-9u7y-s6c2-87devc61v4kl");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "y63nfyte-v6dr-284f-k8u9-1qfdr456vnhg");
        }
    }
}
