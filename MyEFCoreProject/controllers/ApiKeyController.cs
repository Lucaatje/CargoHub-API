using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("cargohub/apikey")]
public class ApiKeyController : ControllerBase
{
    private readonly ApiKeyService _apiKeyService;

    public ApiKeyController(ApiKeyService apiKeyService)
    {
        _apiKeyService = apiKeyService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateApiKey([FromBody] ApiKeyRequest request)
    {
        var rawApiKey = await _apiKeyService.CreateAndSaveApiKeyAsync(
            request.AppName,
            new Dictionary<string, bool>
            {
                { "Read", request.Permissions.Read },
                { "Write", request.Permissions.Write },
                {"Update", request.Permissions.Update},
                {"Delete", request.Permissions.Delete}
            });

        return Ok(new { ApiKey = rawApiKey });
    }
}

public class ApiKeyRequest
{
    public string AppName { get; set; }
    public PermissionsDto Permissions { get; set; }
}

public class PermissionsDto
{
    public bool Read { get; set; }
    public bool Write { get; set; }

    public bool Update {get; set;}

    public bool Delete {get; set;}
}
