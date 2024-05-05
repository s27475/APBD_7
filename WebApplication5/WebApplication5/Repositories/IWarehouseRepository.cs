using WebApplication5.Dto;

namespace WebApplication5.Repositories;

public interface IWarehouseRepository
{
    public Task<bool> ProductExist(int idProduct);
    public Task<bool> WarehouseExist(int warehouseId);
    public Task<int> ProductWithDateAndAmount(int idProduct, int amount, DateTime createdAt);
    public Task<bool> NotCompleted(int orderId);
    public Task<int> UpdateFulfilledAt(int orderId);
    public Task<decimal> GetPrice(int idProduct);
    public Task<int> InsertToPW(Product product, int orderId);
    public Task<int> GetPrimaryKey(Product product, int orderId);
}