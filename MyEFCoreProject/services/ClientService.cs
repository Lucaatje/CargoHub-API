using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ClientService : IClientService
{
    private readonly DatabaseContext _context;
    //private readonly IAuditLogService _auditLogService;

    public ClientService(DatabaseContext DbContext)
    {
        _context = DbContext;
        //_auditLogService = auditLogService;
    }

    public async Task<ServiceResult> ReadClient(int client_id, string api_key)
    {
        try
        {
            var client = await _context.Clients.FindAsync(client_id);

            if (client == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such client with id: {client_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such client with id: {client_id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching client", api_key);
            return new ServiceResult { Object = client, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch client with id {client_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadClients(string api_key)
    {
        try
        {
            var clients = await _context.Clients.ToListAsync();

            if (!clients.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No clients found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No clients found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple clients", api_key);
            return new ServiceResult { Object = clients, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple clients - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateClient(Client client, string api_key)
    {
        try
        {
            if (_context.Clients.Any(x => x.Id == client.Id))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Id {client.Id} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Id {client.Id} already in use" };
            }

            client.Created_At = DateTime.UtcNow;
            client.Updated_At = DateTime.UtcNow;
            _context.Clients.Add(client);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create client", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create client, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Client created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create client with id {client.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateClient(Client client, int client_id, string api_key)
    {
        try
        {
            var existingClient = await _context.Clients.FindAsync(client_id);
            if (existingClient == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Client not found with id {client_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Client not found with id {client_id}" };
            }

            existingClient.Name = client.Name;
            existingClient.Address = client.Address;
            existingClient.City = client.City;
            existingClient.Zip_Code = client.Zip_Code;
            existingClient.Province = client.Province;
            existingClient.Country = client.Country;
            existingClient.Contact_Name = client.Contact_Name;
            existingClient.Contact_Phone = client.Contact_Phone;
            existingClient.Contact_Email = client.Contact_Email;
            existingClient.Updated_At = DateTime.UtcNow;
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update client with id {client_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update client, please try again with id {client_id}" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated client succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update client with id {client.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteClient(int client_id, string api_key)
    {
        try
        {
            var client = await _context.Clients.FindAsync(client_id);
            if (client == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Client with id {client_id} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Client with id {client_id} already not in database" };
            }
            _context.Clients.Remove(client);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete client with id {client_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete client with id {client_id}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted client succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to delete client with id {client_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadClientsOrder(int client_id, string api_key)
    {
        try
        {
            var orders = await _context.Orders.Where(x => x.Bill_To == client_id | x.Ship_To == client_id).ToListAsync();
            if (!orders.Any())
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No orders found for client with id {client_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"404 NOT FOUND: No orders found for client with id {client_id}" };
            }

            await AuditLogService.LogActionAsync("GET", $"200 OK: Fetching orders for client with id {client_id}", api_key);
            return new ServiceResult { Object = orders, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to delete client with id {client_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
}

public interface IClientService
{
    public Task<ServiceResult> ReadClient(int client_id, string api_key);
    public Task<ServiceResult> ReadClients(string api_key);
    public Task<ServiceResult> CreateClient(Client client, string api_key);
    public Task<ServiceResult> UpdateClient(Client client, int client_id, string api_key);
    public Task<ServiceResult> DeleteClient(int client_id, string api_key);

    public Task<ServiceResult> ReadClientsOrder(int client_id, string api_key);
}