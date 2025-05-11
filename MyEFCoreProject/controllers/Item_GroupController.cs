using Microsoft.AspNetCore.Mvc;

namespace MyEFCoreProject.Controllers;

[Route("cargohub")]
[ApiController]
public class Item_GroupController : Controller
{
    private readonly IItem_GroupService _item_GroupService;

    public Item_GroupController(IItem_GroupService item_GroupService)
    {
        _item_GroupService = item_GroupService;
    }

    [HttpGet("item_groups/{item_group_id}")]
    public async Task<IActionResult> ReadItem_Group(int item_group_id)
    {
        var serviceResult = await _item_GroupService.ReadItem_Group(item_group_id, Request.Headers["API_KEY"]!);

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

    [HttpGet("item_groups")]
    public async Task<IActionResult> ReadItem_Groups()
    {
        var serviceResult = await _item_GroupService.ReadItem_Groups(Request.Headers["API_KEY"]!);

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

    [HttpGet("item_groups/{item_group_id}/items")]
    public async Task<IActionResult> ReadItemsForItem_Group(int item_group_id)
    {
        var serviceResult = await _item_GroupService.ReadItemsForItem_Group(item_group_id, Request.Headers["API_KEY"]!);

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

    [HttpPost("item_groups")]
    public async Task<IActionResult> CreateItem_Group([FromBody] Item_Group item_group)
    {
        var serviceResult = await _item_GroupService.CreateItem_Group(item_group, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Item_group created successfully.");
        }
        else if (serviceResult.StatusCode == 409)
        {
            return Conflict(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("item_groups/{item_group_id}")]
    public async Task<IActionResult> UpdateItem_Group([FromBody] Item_Group item_group, int item_group_id)
    {
        var serviceResult = await _item_GroupService.UpdateItem_Group(item_group, item_group_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Item_group updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpDelete("item_groups/{item_group_id}")]
    public async Task<IActionResult> DeleteItem_Group(int item_group_id)
    {
        var serviceResult = await _item_GroupService.DeleteItem_Group(item_group_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Item_group deleted succesfully.");
        }
        else if (serviceResult.StatusCode == 400)
        {
            return BadRequest(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }
}