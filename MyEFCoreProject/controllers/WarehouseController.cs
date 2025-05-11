using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MyEFCoreProject.Controllers;

[Route("cargohub")]
[ApiController]
public class WarehouseController : Controller
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet("warehouses/{warehouse_id}")]
    public async Task<IActionResult> ReadWarehouse(int warehouse_id)
    {
        var serviceResult = await _warehouseService.ReadWarehouse(warehouse_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok(serviceResult.Object);
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpGet("warehouses")]
    public async Task<IActionResult> ReadWarehouses()
    {
        var serviceResult = await _warehouseService.ReadWarehouses(Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok(serviceResult.Object);
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpGet("warehouses/{warehouse_id}/locations")]
    public async Task<IActionResult> ReadLocationsInWarehouse(int warehouse_id)
    {
        var serviceResult = await _warehouseService.ReadLocationsInWarehouse(warehouse_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok(serviceResult.Object);
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPost("warehouses")]
    public async Task<IActionResult> CreateWarehouse(Warehouse warehouse)
    {
        var serviceResult = await _warehouseService.CreateWarehouse(warehouse, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Warehouse created successfully.");
        }
        else if (serviceResult.StatusCode == 409)
        {
            return Conflict(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("warehouses/{warehouse_id}")]
    public async Task<IActionResult> UpdateWarehouse(Warehouse warehouse, int warehouse_id)
    {
        var serviceResult = await _warehouseService.UpdateWarehouse(warehouse, warehouse_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Warehouse updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpDelete("warehouses/{warehouse_id}")]
    public async Task<IActionResult> DeleteWarehouse(int warehouse_id)
    {
        var serviceResult = await _warehouseService.DeleteWarehouse(warehouse_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Warehouse deleted succesfully.");
        }
        else if (serviceResult.StatusCode == 400)
        {
            return BadRequest(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }
}