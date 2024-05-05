using WebApplication5.Dto;
using WebApplication5.Repositories;

namespace WebApplication5.Services;

public class WarehouseService : IWarehouseService
{
    private IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }
    
    public async Task<bool> ProductExist(int idProduct)
    {
        return await _warehouseRepository.ProductExist(idProduct);
    }

    public async Task<bool> WarehouseExist(int warehouseId)
    {
        return await _warehouseRepository.WarehouseExist(warehouseId);
    }

    public async Task<int> ProductWithDateAndAmount(int idProduct, int amount, DateTime createdAt)
    {
        return await _warehouseRepository.ProductWithDateAndAmount(idProduct, amount, createdAt);
    }

    public async Task<bool> NotCompleted(int orderId)
    {
        return await _warehouseRepository.NotCompleted(orderId);
    }

    public async Task<int> UpdateFulfilledAt(int orderId)
    {
        return await _warehouseRepository.UpdateFulfilledAt(orderId);
    }

    public async Task<int> InsertToPW(Product product, int orderId)
    {
        return await _warehouseRepository.InsertToPW(product, orderId);
    }

    public async Task<int> GetPrimaryKey(Product product, int orderId)
    {
        return await _warehouseRepository.GetPrimaryKey(product, orderId);
    }
}