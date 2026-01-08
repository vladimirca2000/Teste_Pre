using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeuTeste.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Email", x => x.Email);
                    table.UniqueConstraint("AK_Users_Username", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "IsDelete", "CreatedAt", "CreatedUser" },
                values: new object[,]
                {
                    { 1, "Eletrônicos", false, new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 2, "Livros", false, new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 3, "Roupas", false, new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 4, "Alimentos", false, new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), "system" },
                    { 5, "Casa e Jardim", false, new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), "system" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Username", "Email", "PasswordHash", "Role", "IsActive", "IsDelete", "CreatedAt", "CreatedUser" },
                values: new object[] { 1, "admin", "vladimirca2000@gmail.com", "fTH3iZVqKFLpI+C3/wHNVfVDLqX2gDfXlYNQKrWlqJU=", 2, true, false, new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), "system" });

            // Inserir 50 produtos
            var products = new List<object>();
            int productId = 1;

            // Eletrônicos (10)
            for (int i = 0; i < 10; i++)
            {
                products.Add(new { Id = productId++, Name = $"Eletrônico {i + 1}", CategoryId = 1, Price = 100.00m + (i * 50), IsDelete = false, CreatedAt = new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), CreatedUser = "system" });
            }

            // Livros (10)
            for (int i = 0; i < 10; i++)
            {
                products.Add(new { Id = productId++, Name = $"Livro {i + 1}", CategoryId = 2, Price = 30.00m + (i * 10), IsDelete = false, CreatedAt = new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), CreatedUser = "system" });
            }

            // Roupas (10)
            for (int i = 0; i < 10; i++)
            {
                products.Add(new { Id = productId++, Name = $"Roupa {i + 1}", CategoryId = 3, Price = 50.00m + (i * 15), IsDelete = false, CreatedAt = new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), CreatedUser = "system" });
            }

            // Alimentos (10)
            for (int i = 0; i < 10; i++)
            {
                products.Add(new { Id = productId++, Name = $"Alimento {i + 1}", CategoryId = 4, Price = 10.00m + (i * 5), IsDelete = false, CreatedAt = new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), CreatedUser = "system" });
            }

            // Casa e Jardim (10)
            for (int i = 0; i < 10; i++)
            {
                products.Add(new { Id = productId++, Name = $"Item Casa e Jardim {i + 1}", CategoryId = 5, Price = 25.00m + (i * 20), IsDelete = false, CreatedAt = new DateTime(2026, 1, 8, 10, 0, 0, 0, DateTimeKind.Utc), CreatedUser = "system" });
            }

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "CategoryId", "Price", "IsDelete", "CreatedAt", "CreatedUser" },
                values: products.ToArray());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
