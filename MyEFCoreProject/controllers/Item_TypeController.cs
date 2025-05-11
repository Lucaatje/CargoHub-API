using Microsoft.AspNetCore.Mvc;

namespace MyEFCoreProject.Controllers;

[Route("cargohub")]
[ApiController]
public class Item_TypeController : Controller
{
    private readonly IItem_TypeService _item_TypeService;

    public Item_TypeController(IItem_TypeService item_TypeService)
    {
        _item_TypeService = item_TypeService;
    }

    [HttpGet("item_types/{item_type_id}")]
    public async Task<IActionResult> ReadItem_Type(int item_type_id)
    {
        var serviceResult = await _item_TypeService.ReadItem_Type(item_type_id, Request.Headers["API_KEY"]!);

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

    [HttpGet("item_types")]
    public async Task<IActionResult> ReadItem_Types()
    {
        var serviceResult = await _item_TypeService.ReadItem_Types(Request.Headers["API_KEY"]!);

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

    [HttpGet("item_types/{item_type_id}/items")]
    public async Task<IActionResult> ReadItemsForItem_Type(int item_type_id)
    {
        var serviceResult = await _item_TypeService.ReadItemsForItem_Type(item_type_id, Request.Headers["API_KEY"]!);

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

    [HttpPost("item_types")]
    public async Task<IActionResult> CreateItem_Type([FromBody] Item_Type item_type)
    {
        var serviceResult = await _item_TypeService.CreateItem_Type(item_type, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Item_type created successfully.");
        }
        else if (serviceResult.StatusCode == 409)
        {
            return Conflict(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("item_types/{item_type_id}")]
    public async Task<IActionResult> UpdateItem_Type([FromBody] Item_Type item_type, int item_type_id)
    {
        var serviceResult = await _item_TypeService.UpdateItem_Type(item_type, item_type_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Item_type updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpDelete("item_types/{item_type_id}")]
    public async Task<IActionResult> DeleteItem_Type(int item_type_id)
    {
        var serviceResult = await _item_TypeService.DeleteItem_Type(item_type_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Item_type deleted succesfully.");
        }
        else if (serviceResult.StatusCode == 400)
        {
            return BadRequest(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }
}