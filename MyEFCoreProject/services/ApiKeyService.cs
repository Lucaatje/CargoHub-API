using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class ApiKeyService
{
    private readonly DatabaseContext _dbContext;

    public ApiKeyService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string GenerateApiKey()
    {
        return Guid.NewGuid().ToString(); 
    }

    public string HashApiKey(string apiKey)
    {
        return BCrypt.Net.BCrypt.HashPassword(apiKey);
    }

    public async Task<string> CreateAndSaveApiKeyAsync(string appName, Dictionary<string, bool> permissions)
    {
        var rawApiKey = GenerateApiKey();
        var hashedApiKey = HashApiKey(rawApiKey);

        var newApiKey = new Api_Key
        {
            ApiKey = hashedApiKey,
            App = appName,
            Permissions = permissions
        };

        _dbContext.Api_Keys.Add(newApiKey);
        await _dbContext.SaveChangesAsync();

        Console.WriteLine($"The raw api key is: {rawApiKey}");

        return rawApiKey;
    }
}