using Microsoft.AspNetCore.Mvc;

namespace MyEFCoreProject.Controllers;

[Route("cargohub")]
[ApiController]
public class Item_LineController : Controller
{
    private readonly IItem_LineService _item_LineService;

    public Item_LineController(IItem_LineService item_LineService)
    {
        _item_LineService = item_LineService;
    }

    [HttpGet("item_lines/{item_line_id}")]
    public async Task<IActionResult> ReadItem_Line(int item_line_id)
    {
        var serviceResult = await _item_LineService.ReadItem_Line(item_line_id, Request.Headers["API_KEY"]!);

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

    [HttpGet("item_lines")]
    public async Task<IActionResult> ReadItem_Lines()
    {
        var serviceResult = await _item_LineService.ReadItem_Lines(Request.Headers["API_KEY"]!);

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

    [HttpGet("item_lines/{item_line_id}/items")]
    public async Task<IActionResult> ReadItemsForItem_Line(int item_line_id)
    {
        var serviceResult = await _item_LineService.ReadItemsForItem_Line(item_line_id, Request.Headers["API_KEY"]!);

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

    [HttpPost("item_lines")]
    public async Task<IActionResult> CreateItem_Line([FromBody] Item_Line item_line)
    {
        var serviceResult = await _item_LineService.CreateItem_Line(item_line, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Item_line created successfully.");
        }
        else if (serviceResult.StatusCode == 409)
        {
            return Conflict(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("item_lines/{item_line_id}")]
    public async Task<IActionResult> UpdateItem_Line([FromBody] Item_Line item_line, int item_line_id)
    {
        var serviceResult = await _item_LineService.UpdateItem_Line(item_line, item_line_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Item_line updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpDelete("item_lines/{item_line_id}")]
    public async Task<IActionResult> DeleteItem_Line(int item_line_id)
    {
        var serviceResult = await _item_LineService.DeleteItem_Line(item_line_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Item_line deleted succesfully.");
        }
        else if (serviceResult.StatusCode == 400)
        {
            return BadRequest(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }
}