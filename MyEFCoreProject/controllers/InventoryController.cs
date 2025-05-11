using Microsoft.AspNetCore.Mvc;

namespace MyEFCoreProject.Controllers;

[Route("cargohub")]
[ApiController]
public class InventoryController : Controller
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet("inventories/{inventory_id}")]
    public async Task<IActionResult> ReadInventory(int inventory_id)
    {
        var serviceResult = await _inventoryService.ReadInventory(inventory_id, Request.Headers["API_KEY"]!);

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

    [HttpGet("inventories")]
    public async Task<IActionResult> ReadInventories()
    {
        var serviceResult = await _inventoryService.ReadInventories(Request.Headers["API_KEY"]!);

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

    [HttpPost("inventories")]
    public async Task<IActionResult> CreateInventory([FromBody] Inventory inventory)
    {
        var serviceResult = await _inventoryService.CreateInventory(inventory, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Inventory created successfully.");
        }
        else if (serviceResult.StatusCode == 409)
        {
            return Conflict(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("inventories/{inventory_id}")]
    public async Task<IActionResult> UpdateInventory([FromBody] Inventory inventory, int inventory_id)
    {
        var serviceResult = await _inventoryService.UpdateInventory(inventory, inventory_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Inventory updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpDelete("inventories/{inventory_id}")]
    public async Task<IActionResult> DeleteInventory(int inventory_id)
    {
        var serviceResult = await _inventoryService.DeleteInventory(inventory_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Inventory deleted succesfully.");
        }
        else if (serviceResult.StatusCode == 400)
        {
            return BadRequest(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }
}