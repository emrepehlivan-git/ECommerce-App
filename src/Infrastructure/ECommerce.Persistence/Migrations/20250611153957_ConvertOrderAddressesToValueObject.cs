using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ConvertOrderAddressesToValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "billing_address",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "shipping_address",
                table: "orders");

            migrationBuilder.AddColumn<string>(
                name: "BillingCity",
                table: "orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingCountry",
                table: "orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingState",
                table: "orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingStreet",
                table: "orders",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BillingZipCode",
                table: "orders",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingCity",
                table: "orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingCountry",
                table: "orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingState",
                table: "orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingStreet",
                table: "orders",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippingZipCode",
                table: "orders",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingCity",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "BillingCountry",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "BillingState",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "BillingStreet",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "BillingZipCode",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "ShippingCity",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "ShippingCountry",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "ShippingState",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "ShippingStreet",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "ShippingZipCode",
                table: "orders");

            migrationBuilder.AddColumn<string>(
                name: "billing_address",
                table: "orders",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "shipping_address",
                table: "orders",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
