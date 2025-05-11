using Microsoft.AspNetCore.Mvc;

namespace MyEFCoreProject.Controllers;

[Route("cargohub")]
[ApiController]
public class ShipmentController : Controller
{
    private readonly IShipmentService _shipmentService;

    public ShipmentController(IShipmentService shipmentService)
    {
        _shipmentService = shipmentService;
    }

    [HttpGet("shipments/{shipment_id}")]
    public async Task<IActionResult> ReadShipment(int shipment_id)
    {
        var serviceResult = await _shipmentService.ReadShipment(shipment_id, Request.Headers["API_KEY"]!);

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

    [HttpGet("shipments")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1)
    {
        var serviceResult = await _shipmentService.GetAllShipments(page, Request.Headers["API_KEY"]!);

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

    [HttpGet("shipments/{shipment_id}/items")]
    public async Task<IActionResult> ReadShipmentItems(int shipment_id)
    {
        var serviceResult = await _shipmentService.ReadShipmentItems(shipment_id, Request.Headers["API_KEY"]!);

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

    [HttpPost("shipments")]
    public async Task<IActionResult> CreateShipment([FromBody] Shipment shipment)
    {
        var serviceResult = await _shipmentService.CreateShipment(shipment, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Shipment created successfully.");
        }
        else if (serviceResult.StatusCode == 409)
        {
            return Conflict(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpGet("shipments/{shipment_id}/orders")]
    public async Task<IActionResult> ReadShipmentOrder(int shipment_id)
    {
        var serviceResult = await _shipmentService.ReadShipmentOrder(shipment_id, Request.Headers["API_KEY"]!);

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

    [HttpPut("shipments/{shipment_id}")]
    public async Task<IActionResult> UpdateShipment([FromBody] Shipment shipment, int shipment_id)
    {
        var serviceResult = await _shipmentService.UpdateShipment(shipment, shipment_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Shipment updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("shipments/{shipment_id}/orders")]
    public async Task<IActionResult> UpdateShipmentOrder([FromBody] Order order, int shipment_id)
    {
        var serviceResult = await _shipmentService.UpdateShipmentOrder(order, shipment_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Order updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("shipments/{shipment_id}/items")]
    public async Task<IActionResult> UpdateShipmentItems([FromBody] List<PropertyItem> items, int shipment_id)
    {
        var serviceResult = await _shipmentService.UpdateShipmentItems(items, shipment_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Items updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpDelete("shipments/{shipment_id}")]
    public async Task<IActionResult> DeleteShipment(int shipment_id)
    {
        var serviceResult = await _shipmentService.DeleteShipment(shipment_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Shipment deleted succesfully.");
        }
        else if (serviceResult.StatusCode == 400)
        {
            return BadRequest(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("shipments/Commit/{shipment_id}")]
    public async Task<IActionResult> CommitShipment(int shipment_id)
    {
        var result = await _shipmentService.CommitShipment(shipment_id);
        if (result)
        {
            return Ok("Shipment Commited");
        }
        return BadRequest("Failed to commit");
    }
}