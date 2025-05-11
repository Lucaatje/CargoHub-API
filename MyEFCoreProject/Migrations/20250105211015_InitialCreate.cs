using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyEFCoreProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Api_Keys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApiKey = table.Column<string>(type: "TEXT", nullable: false),
                    App = table.Column<string>(type: "TEXT", nullable: false),
                    Permissions = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Api_Keys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Zip_Code = table.Column<string>(type: "TEXT", nullable: false),
                    Province = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Contact_Name = table.Column<string>(type: "TEXT", nullable: false),
                    Contact_Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Contact_Email = table.Column<string>(type: "TEXT", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Item_Id = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Item_Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Locations = table.Column<string>(type: "TEXT", nullable: false),
                    Total_On_Hand = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_Expected = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_Ordered = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_Allocated = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_Available = table.Column<int>(type: "INTEGER", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Item_Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Item_Lines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item_Lines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Item_Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    UId = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Short_Description = table.Column<string>(type: "TEXT", nullable: false),
                    Upc_Code = table.Column<string>(type: "TEXT", nullable: false),
                    Model_Number = table.Column<string>(type: "TEXT", nullable: false),
                    Commodity_Code = table.Column<string>(type: "TEXT", nullable: false),
                    Item_Line = table.Column<int>(type: "INTEGER", nullable: false),
                    Item_Group = table.Column<int>(type: "INTEGER", nullable: false),
                    Item_Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit_Purchase_Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit_Order_Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Pack_Order_Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Supplier_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Supplier_Code = table.Column<string>(type: "TEXT", nullable: false),
                    Supplier_Part_Number = table.Column<string>(type: "TEXT", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.UId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Warehouse_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Source_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Order_Date = table.Column<string>(type: "TEXT", nullable: false),
                    Request_Date = table.Column<string>(type: "TEXT", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Reference_Extra = table.Column<string>(type: "TEXT", nullable: false),
                    Order_Status = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    Shipping_Notes = table.Column<string>(type: "TEXT", nullable: false),
                    Picking_Notes = table.Column<string>(type: "TEXT", nullable: false),
                    Warehouse_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Ship_To = table.Column<string>(type: "TEXT", nullable: true),
                    Bill_To = table.Column<string>(type: "TEXT", nullable: true),
                    Shipment_Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Total_Amount = table.Column<double>(type: "REAL", nullable: false),
                    Total_Discount = table.Column<double>(type: "REAL", nullable: false),
                    Total_Tax = table.Column<double>(type: "REAL", nullable: false),
                    Total_Surcharge = table.Column<double>(type: "REAL", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Items = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Order_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Source_Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Order_Date = table.Column<string>(type: "TEXT", nullable: false),
                    Request_Date = table.Column<string>(type: "TEXT", nullable: false),
                    Shipment_Date = table.Column<string>(type: "TEXT", nullable: false),
                    Shipment_Type = table.Column<string>(type: "TEXT", nullable: false),
                    Shipment_Status = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    Carrier_Code = table.Column<string>(type: "TEXT", nullable: false),
                    Carrier_Description = table.Column<string>(type: "TEXT", nullable: false),
                    Service_Code = table.Column<string>(type: "TEXT", nullable: false),
                    Payment_Type = table.Column<string>(type: "TEXT", nullable: false),
                    Transfer_Mode = table.Column<string>(type: "TEXT", nullable: false),
                    Total_Package_Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Total_Package_Weight = table.Column<double>(type: "REAL", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Items = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Address_Extra = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Zip_Code = table.Column<string>(type: "TEXT", nullable: false),
                    Province = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Contact_Name = table.Column<string>(type: "TEXT", nullable: false),
                    Phonenumber = table.Column<string>(type: "TEXT", nullable: false),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Reference = table.Column<string>(type: "TEXT", nullable: false),
                    Transfer_From = table.Column<string>(type: "TEXT", nullable: true),
                    Transfer_To = table.Column<string>(type: "TEXT", nullable: false),
                    Transfer_Status = table.Column<string>(type: "TEXT", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Items = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Zip = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Province = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Contact = table.Column<string>(type: "TEXT", nullable: false),
                    Created_At = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated_At = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Api_Keys");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Item_Groups");

            migrationBuilder.DropTable(
                name: "Item_Lines");

            migrationBuilder.DropTable(
                name: "Item_Types");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}
