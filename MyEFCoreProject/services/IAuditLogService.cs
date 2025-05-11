public static class AuditLogService
{
    private static readonly string _actionLogFilePath = Path.Combine(Directory.GetCurrentDirectory(), "audit_action_log.txt");
    private static readonly string _apikeyLogFilePath = Path.Combine(Directory.GetCurrentDirectory(), "audit_apikey_log.txt");

    // public AuditLogService()
    // {
    //     // Set the path where the log file will be stored
    //     _actionLogFilePath = Path.Combine(Directory.GetCurrentDirectory(), "audit_action_log.txt");
    //     _apikeyLogFilePath = Path.Combine(Directory.GetCurrentDirectory(), "audit_apikey_log.txt");
    // }

    public static async Task LogActionAsync(string action, string description, string apiKey)
    {
        // Prepare the log entry
        var actionLogEntry = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | Action: {action} | Description: {description} | ApiKey: {apiKey}\n";

        // Ensure the directory exists
        var actionLogDirectory = Path.GetDirectoryName(_actionLogFilePath);
        if (!Directory.Exists(actionLogDirectory))
        {
            Directory.CreateDirectory(actionLogDirectory);
        }

        // Write the log entry to the file
        await File.AppendAllTextAsync(_actionLogFilePath, actionLogEntry);
    }

    public static async Task LogAPIKeyAsync(string action, string description, string apiKey)
    {
        // Prepare the log entry
        var apikeyLogEntry = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} | Action: {action} | Description: {description} | ApiKey: {apiKey}\n";

        // Ensure the directory exists
        var apikeyLogDirectory = Path.GetDirectoryName(_apikeyLogFilePath);
        if (!Directory.Exists(apikeyLogDirectory))
        {
            Directory.CreateDirectory(apikeyLogDirectory);
        }

        // Write the log entry to the file
        await File.AppendAllTextAsync(_apikeyLogFilePath, apikeyLogEntry);
    }
}

// public interface IAuditLogService
// {
//     Task LogActionAsync(string action, string description, string apiKey);
//     Task LogAPIKeyAsync(string action, string description, string apiKey);
// }