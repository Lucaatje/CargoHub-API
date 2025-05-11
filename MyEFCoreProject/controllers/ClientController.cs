using Microsoft.AspNetCore.Mvc;

namespace MyEFCoreProject.Controllers;

[Route("cargohub")]
[ApiController]
public class ClientController : Controller
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }
    
    [HttpGet("clients/{client_id}")]
    public async Task<IActionResult> ReadClient(int client_id)
    {
        var serviceResult = await _clientService.ReadClient(client_id, Request.Headers["API_KEY"]!);

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

    [HttpGet("clients")]
    public async Task<IActionResult> ReadClients()
    {
        var serviceResult = await _clientService.ReadClients(Request.Headers["API_KEY"]!);

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

    [HttpGet("clients/{client_id}/orders")]
    public async Task<IActionResult> ReadClientsOrder(int client_id)
    {
        var serviceResult = await _clientService.ReadClientsOrder(client_id, Request.Headers["API_KEY"]!);

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

    [HttpPost("clients")]
    public async Task<IActionResult> CreateClient([FromBody] Client client)
    {
        var serviceResult = await _clientService.CreateClient(client, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Client created successfully.");
        }
        else if (serviceResult.StatusCode == 409)
        {
            return Conflict(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpPut("clients/{client_id}")]
    public async Task<IActionResult> UpdateClient([FromBody] Client client, int client_id)
    {
        var serviceResult = await _clientService.UpdateClient(client, client_id, Request.Headers["API_KEY"]!);

        if (serviceResult.StatusCode == 200)
        {
            return Ok("Client updated successfully.");
        }
        else if (serviceResult.StatusCode == 404)
        {
            return NotFound(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }

    [HttpDelete("clients/{client_id}")]
    public async Task<IActionResult> DeleteClient(int client_id)
    {
        var serviceResult = await _clientService.DeleteClient(client_id, Request.Headers["API_KEY"]!);
        
        if (serviceResult.StatusCode == 200)
        {
            return Ok("Client deleted succesfully.");
        }
        else if (serviceResult.StatusCode == 400)
        {
            return BadRequest(serviceResult.ErrorMessage);
        }
        return StatusCode(500, serviceResult.ErrorMessage);
    }
}