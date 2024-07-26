using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreAPI.Migrations
{
    public partial class AddFunctionAndProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE FUNCTION dbo.GetProductsJson(@CompanyId BIGINT, @StoreId BIGINT)
                RETURNS NVARCHAR(MAX)
                AS
                BEGIN
                    DECLARE @json NVARCHAR(MAX);

                    SELECT @json = (SELECT 
                                        p.Id AS ProductId,
                                        p.Name AS ProductName,
                                        p.Value AS ProductValue
                                    FROM Products p
                                    INNER JOIN Stores s ON p.StoreId = s.Id
                                    WHERE s.CompanyId = @CompanyId AND p.StoreId = @StoreId
                                    FOR JSON PATH, WITHOUT_ARRAY_WRAPPER);

                    RETURN @json;
                END
            ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE dbo.InsertProduct
                    @Name NVARCHAR(100),
                    @Value DECIMAL(18, 2),
                    @StoreId INT,
                    @NewProductId INT OUTPUT
                AS
                BEGIN
                    INSERT INTO Products (Name, Value, StoreId)
                    VALUES (@Name, @Value, @StoreId);
                    
                    SET @NewProductId = SCOPE_IDENTITY();
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS dbo.GetProductsJson");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.InsertProduct");
        }
    }
}