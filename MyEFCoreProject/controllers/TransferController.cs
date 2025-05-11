using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MyEFCoreProject.Controllers;

[Route("cargohub")]
[ApiController]
public class TransferController : Controller
{
    private readonly ITransferService _transferService;

    public TransferController(ITransferService transferService)
    {
        _transferService = transferService;
    }

    [HttpGet("transfers/{transfer_id}")]
    public async Task<IActionResult> ReadTransfer(int transfer_id)
    {
        var serviceResult = await _transferService.ReadTransfer(transfer_id, Request.Headers["API_KEY"]!);

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

    [HttpGet("transfers")]
    public async Task<IActionResult> ReadTransfers()
    {
        var serviceResult = await _transferService.ReadTransfers(Request.Headers["API_KEY"]!);

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

    [HttpGet("transfers/{transfer_id}/items")]
    public async Task<IActionResult> ReadTransferItems(int transfer_id)
    {
        var serviceResult = await _transferService.ReadTransferItems(transfer_id, Request.Headers["API_KEY"]!);

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

    [HttpPost("transfers")]
    public async Task<IActionResult> CreateTransfer(Transfer transfer)
    {
        var serviceResult = await _transferService.CreateTransfer(transfer, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Transfer created successfully.");
        }
        else if (serviceResult.StatusCode == 409)
        {
            return Conflict(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("transfers/{transfer_id}")]
    public async Task<IActionResult> UpdateTransfer(Transfer transfer, int transfer_id)
    {
        var serviceResult = await _transferService.UpdateTransfer(transfer, transfer_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Transfer updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpDelete("transfers/{transfer_id}")]
    public async Task<IActionResult> DeleteTransfer(int transfer_id)
    {
        var serviceResult = await _transferService.DeleteTransfer(transfer_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Transfer deleted succesfully.");
        }
        else if (serviceResult.StatusCode == 400)
        {
            return BadRequest(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("transfers/Commit/{transfer_id}")]
    public async Task<IActionResult> CommitTransfer(int transfer_id)
    {
        var result = await _transferService.CommitTransfer(transfer_id);
        if (result)
        {
            return Ok("Transfer Commited");
        }
        return BadRequest("Failed to commit");
    }
}