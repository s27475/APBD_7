using Microsoft.AspNetCore.Mvc;
using WebApplication5.Dto;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(Product product)
    {
        bool productExist = await _warehouseService.ProductExist(product.IdProduct);
        if (!productExist)
        {
            return NotFound($"Nie mozna znalezc produktu o id: {product.IdProduct} ");
        }

        bool warehouseExist = await _warehouseService.WarehouseExist(product.IdWarehouse);
        if (!warehouseExist)
        {
            return NotFound($"Nie mozna znalezc magazynu o id: {product.IdWarehouse}");
        }

        int orderId = await _warehouseService.ProductWithDateAndAmount(product.IdProduct, product.Amount, product.CreatedAt);

        if (orderId == Int32.MaxValue)
        {
            return NotFound("Nie mozna znalezc zamówienia");
        }

        bool notInCompleted = await _warehouseService.NotCompleted(orderId);

        if (!notInCompleted)
        {
            return NotFound($"Zamowienie id: {orderId} jest skompletowane");
        }

        await _warehouseService.UpdateFulfilledAt(orderId);

        await _warehouseService.InsertToPW(product, orderId);

        int pk = await _warehouseService.GetPrimaryKey(product, orderId);

        if (pk == 0)
        {
            return NotFound("Nie mozna znalezc danego zamowienia");
        }

        return Ok($"Klucz glowny do tego zamowienia to: {pk}");

    }
}