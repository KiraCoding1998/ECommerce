using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Server.Migrations
{
    /// <inheritdoc />
    public partial class ManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PiecesAvaliable = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientProducts",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientHas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProducts", x => new { x.ClientId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ClientProducts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "IsAdmin", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[] { 1, true, new byte[] { 202, 54, 174, 172, 252, 69, 222, 169, 90, 80, 236, 74, 8, 203, 195, 116, 88, 0, 236, 73, 114, 247, 241, 221, 113, 224, 214, 27, 235, 172, 118, 6, 47, 51, 149, 201, 168, 77, 228, 124, 176, 107, 253, 114, 247, 14, 106, 0, 58, 70, 11, 155, 185, 35, 103, 113, 223, 218, 178, 28, 114, 214, 234, 176 }, new byte[] { 72, 97, 168, 45, 73, 159, 30, 174, 94, 78, 244, 231, 247, 248, 52, 223, 63, 174, 174, 220, 17, 201, 217, 123, 16, 27, 234, 226, 63, 110, 211, 249, 150, 131, 148, 195, 187, 142, 2, 228, 180, 166, 120, 201, 30, 76, 249, 120, 221, 55, 221, 227, 118, 252, 112, 122, 114, 35, 211, 180, 42, 179, 186, 218, 135, 4, 104, 129, 121, 34, 199, 52, 16, 223, 188, 69, 34, 100, 28, 191, 64, 6, 123, 161, 169, 220, 233, 27, 74, 216, 108, 13, 25, 77, 235, 21, 8, 118, 44, 171, 194, 251, 189, 217, 172, 172, 245, 236, 57, 247, 190, 70, 4, 101, 100, 244, 173, 37, 85, 215, 224, 212, 141, 144, 47, 153, 242, 79 }, "@adminUserName" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProducts_ProductId",
                table: "ClientProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientProducts");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
