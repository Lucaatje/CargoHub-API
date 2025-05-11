using Microsoft.AspNetCore.Mvc;

namespace MyEFCoreProject.Controllers;

[Route("cargohub")]
[ApiController]
public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }
  
    [HttpGet("suppliers/{supplier_id}")]
    public async Task<IActionResult> ReadSupplier(int supplier_id)
    {
        var serviceResult = await _supplierService.ReadSupplier(supplier_id, Request.Headers["API_KEY"]!);

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

    [HttpGet("suppliers")]
    public async Task<IActionResult> ReadSuppliers()
    {
        var serviceResult = await _supplierService.ReadSuppliers(Request.Headers["API_KEY"]!);

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

    [HttpGet("suppliers/{supplier_id}/items")]
    public async Task<IActionResult> ReadItemsForSuppliers(int supplier_id)
    {
        var serviceResult = await _supplierService.ReadItemsForSupplier(supplier_id, Request.Headers["API_KEY"]!);

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

    [HttpPost("suppliers")]
    public async Task<IActionResult> CreateSupplier(Supplier supplier)
    {
        var serviceResult = await _supplierService.CreateSupplier(supplier, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Supplier created successfully.");
        }
        else if (serviceResult.StatusCode == 409)
        {
            return Conflict(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("suppliers/{supplier_id}")]
    public async Task<IActionResult> UpdateSupplier(Supplier supplier, int supplier_id)
    {
        var serviceResult = await _supplierService.UpdateSupplier(supplier, supplier_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Supplier updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpDelete("suppliers/{supplier_id}")]
    public async Task<IActionResult> DeleteSupplier(int supplier_id)
    {
        var serviceResult = await _supplierService.DeleteSupplier(supplier_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Supplier deleted succesfully.");
        }
        else if (serviceResult.StatusCode == 400)
        {
            return BadRequest(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }
}