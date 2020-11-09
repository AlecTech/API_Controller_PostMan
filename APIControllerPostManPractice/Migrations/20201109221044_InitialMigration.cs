using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIControllerPostManPractice.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(40)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    Quantity = table.Column<int>(type: "int(10)", nullable: false),
                    Discontinued = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "product",
                columns: new[] { "ID", "Discontinued", "Name", "Quantity" },
                values: new object[,]
                {
                    { -1, false, "Mixer", 10 },
                    { -2, false, "Rice Cooker", 10 },
                    { -3, false, "Wrench Set", 10 },
                    { -4, false, "Floor Jack", 10 },
                    { -5, false, "Screwdriver", 10 },
                    { -6, false, "Table", 2 },
                    { -7, false, "Chair", 3 },
                    { -8, true, "Sofa", 1 },
                    { -9, false, "Cupboard", 10 },
                    { -10, false, "Hummer", 10 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product");
        }
    }
}
