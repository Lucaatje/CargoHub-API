using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class InventoryService : IInventoryService
{
    private readonly DatabaseContext _context;

    public InventoryService(DatabaseContext DbContext)
    {
        _context = DbContext;
    }

    public async Task<ServiceResult> ReadInventory(int inventory_id, string api_key)
    {
        try
        {
            var inventory = await _context.Inventories.FindAsync(inventory_id);

            if (inventory == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such inventory with id: {inventory_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such inventory with id: {inventory_id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching inventory", api_key);
            return new ServiceResult { Object = inventory, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch inventory with id {inventory_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadInventories(string api_key)
    {
        try
        {
            var inventories = await _context.Inventories.ToListAsync();

            if (!inventories.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No inventories found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No inventories found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple inventories", api_key);
            return new ServiceResult { Object = inventories, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple inventories - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateInventory(Inventory inventory, string api_key)
    {
        try
        {
            if (_context.Inventories.Any(x => x.Id == inventory.Id))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Id {inventory.Id} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Id {inventory.Id} already in use" };
            }

            inventory.Created_At = DateTime.UtcNow;
            inventory.Updated_At = DateTime.UtcNow;
            _context.Inventories.Add(inventory);
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create inventory", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create inventory, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Inventory created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create inventory with id {inventory.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateInventory(Inventory inventory, int inventory_id, string api_key)
    {
        try
        {
            var existingInventory = await _context.Inventories.FindAsync(inventory_id);
            if (existingInventory == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Inventory not found with id {inventory_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Inventory not found with id {inventory_id}" };
            }

            existingInventory.Item_Id = inventory.Item_Id;
            existingInventory.Description = inventory.Description;
            existingInventory.Item_Reference = inventory.Item_Reference;
            existingInventory.Locations = inventory.Locations;
            existingInventory.Total_On_Hand = inventory.Total_On_Hand;
            existingInventory.Total_Expected = inventory.Total_Expected;
            existingInventory.Total_Ordered = inventory.Total_Ordered;
            existingInventory.Total_Allocated = inventory.Total_Allocated;
            existingInventory.Total_Available = inventory.Total_Available;
            existingInventory.Updated_At = DateTime.UtcNow;
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update inventory with id {inventory_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update inventory, please try again with id {inventory_id}" };
            }
            
            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated inventory succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update inventory with id {inventory.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteInventory(int inventory_id, string api_key)
    {
        try
        {
            var inventory = await _context.Inventories.FindAsync(inventory_id);
            if (inventory == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Inventory with id {inventory_id} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Inventory with id {inventory_id} already not in database" };
            }
            _context.Inventories.Remove(inventory);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete inventory with id {inventory_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete inventory with id {inventory_id}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted inventory succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to delete inventory with id {inventory_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
}

public interface IInventoryService
{
    public Task<ServiceResult> ReadInventory(int inventory_id, string api_key);
    public Task<ServiceResult> ReadInventories(string api_key);
    public Task<ServiceResult> CreateInventory(Inventory inventory, string api_key);
    public Task<ServiceResult> UpdateInventory(Inventory inventory, int inventory_id, string api_key);
    public Task<ServiceResult> DeleteInventory(int inventory_id, string api_key);
}