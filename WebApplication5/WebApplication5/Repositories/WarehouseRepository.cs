using System.Data.SqlClient;
using WebApplication5.Dto;

namespace WebApplication5.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<bool> ProductExist(int idProduct)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Product WHERE IdProduct = @idProduct";
        cmd.Parameters.AddWithValue("@idProduct", idProduct);
        if (await cmd.ExecuteScalarAsync() is not null)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> WarehouseExist(int warehouseId)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Warehouse WHERE IdWarehouse = @warehouseId";
        cmd.Parameters.AddWithValue("@warehouseId", warehouseId);
        if (await cmd.ExecuteScalarAsync() is not null)
        {
            return true;
        }

        return false;
    }

    public async Task<int> ProductWithDateAndAmount(int idProduct, int amount, DateTime createdAt)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdOrder FROM [Order] WHERE IdProduct = @idProduct AND Amount = @amount AND CreatedAt < @createdAt";
        cmd.Parameters.AddWithValue("@idProduct", idProduct);
        cmd.Parameters.AddWithValue("@amount", amount);
        cmd.Parameters.AddWithValue("@createdAt", createdAt);
        var result = await cmd.ExecuteScalarAsync();
        if (result is not null)
        {
            return (int)result;
        }

        return Int32.MaxValue;
    }

    public async Task<bool> NotCompleted(int orderId)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Product_Warehouse WHERE IdOrder = @orderId";
        cmd.Parameters.AddWithValue("@orderId", orderId);
        if (await cmd.ExecuteScalarAsync() is not null)
        {
            return false;
        }

        return true;
    }

    public async Task<int> UpdateFulfilledAt(int orderId)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "UPDATE [Order] SET FulfilledAt = @currdate WHERE IdOrder = @orderId";
        cmd.Parameters.AddWithValue("@currdate", DateTime.Now);
        cmd.Parameters.AddWithValue("@orderId", orderId);

        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<decimal> GetPrice(int idProduct)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT Price FROM Product WHERE IdProduct = @idProduct";
        cmd.Parameters.AddWithValue("@idProduct", idProduct);
        var resault = await cmd.ExecuteScalarAsync();
        if (resault is not null)
        {
            return (decimal)resault;
        }
        return 0;
    }

    public async Task<int> InsertToPW(Product product, int orderId)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        decimal cost = await GetPrice(product.IdProduct);
        decimal price = cost * product.Amount;
        cmd.Connection = con;
        cmd.CommandText = "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES(@warehouseId, @idProduct, @orderId, @amount, @price, @createdAt)";
        cmd.Parameters.AddWithValue("@warehouseId", product.IdWarehouse);
        cmd.Parameters.AddWithValue("@idProduct", product.IdProduct);
        cmd.Parameters.AddWithValue("@orderId", orderId);
        cmd.Parameters.AddWithValue("@amount", product.Amount);
        cmd.Parameters.AddWithValue("@price", price);
        cmd.Parameters.AddWithValue("@createdAt", product.CreatedAt);


        return await cmd.ExecuteNonQueryAsync();
    }

    public async Task<int> GetPrimaryKey(Product product, int orderId)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:Default"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdProductWarehouse FROM Product_Warehouse WHERE IdWarehouse = @warehouseId AND IdProduct = @idProduct AND IdOrder = @orderId";
        cmd.Parameters.AddWithValue("@warehouseId", product.IdWarehouse);
        cmd.Parameters.AddWithValue("@idProduct", product.IdProduct);
        cmd.Parameters.AddWithValue("@orderId", orderId);
        var resault = await cmd.ExecuteScalarAsync();
        if (resault is not null)
        {
            return (int)resault;
        }
        return 0;
    }
}