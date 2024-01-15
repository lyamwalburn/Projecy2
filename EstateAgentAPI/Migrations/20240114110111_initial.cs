using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateAgentAPI.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "buyer",
                columns: table => new
                {
                    BUYER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FIRST_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SURNAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    POSTCODE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PHONE = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buyer", x => x.BUYER_ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "buyer");
        }
    }
}
