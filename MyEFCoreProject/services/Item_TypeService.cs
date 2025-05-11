using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class Item_TypeService : IItem_TypeService
{
    private readonly DatabaseContext _context;

    public Item_TypeService(DatabaseContext DbContext)
    {
        _context = DbContext;
    }

    public async Task<ServiceResult> ReadItem_Type(int item_type_id, string api_key)
    {
        try
        {
            var item_type = await _context.Item_Types.FindAsync(item_type_id);

            if (item_type == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such item_type with id: {item_type_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such item_type with id: {item_type_id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching item_type", api_key);
            return new ServiceResult { Object = item_type, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch item_type with id {item_type_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadItem_Types(string api_key)
    {
        try
        {
            var item_types = await _context.Item_Types.ToListAsync();

            if (!item_types.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No item_types found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No item_types found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple item_types", api_key);
            return new ServiceResult { Object = item_types, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple item_types - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadItemsForItem_Type(int item_type_id, string api_key)
    {
        try
        {
            var items = await _context.Items.Where(item => item.Item_Type == item_type_id).ToListAsync();

            if (!items.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No items for item_type found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No items for item_type found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching items for item_type", api_key);
            return new ServiceResult { Object = items, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch items for item_type - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateItem_Type(Item_Type item_type, string api_key)
    {
        try
        {
            if (_context.Item_Types.Any(x => x.Id == item_type.Id))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Id {item_type.Id} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Id {item_type.Id} already in use" };
            }

            item_type.Created_At = DateTime.UtcNow;
            item_type.Updated_At = DateTime.UtcNow;
            _context.Item_Types.Add(item_type);
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create item_type", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create item_type, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Item_type created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create item_type with id {item_type.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateItem_Type(Item_Type item_type, int item_type_id, string api_key)
    {
        try
        {
            var existingItem_Type = await _context.Item_Types.FindAsync(item_type_id);
            if (existingItem_Type == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Item_type not found with id {item_type_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Item_type not found with id {item_type_id}" };
            }

            existingItem_Type.Name = item_type.Name;
            existingItem_Type.Description = item_type.Description;
            existingItem_Type.Updated_At = DateTime.UtcNow;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update item_type with id {item_type_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update item_type, please try again with id {item_type_id}" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated item_type succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update item_type with id {item_type.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteItem_Type(int item_type_id, string api_key)
    {
        try
        {
            var item_type = await _context.Item_Types.FindAsync(item_type_id);
            if (item_type == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Item_type with id {item_type_id} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Item_type with id {item_type_id} already not in database" };
            }
            _context.Item_Types.Remove(item_type);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete item_type with id {item_type_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete item_type with id {item_type_id}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted item_type succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to delete item_type with id {item_type_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
}

public interface IItem_TypeService
{
    public Task<ServiceResult> ReadItem_Type(int item_type_id, string api_key);
    
    public Task<ServiceResult> ReadItem_Types(string api_key);
    public Task<ServiceResult> ReadItemsForItem_Type(int item_type_id, string api_key);
    public Task<ServiceResult> CreateItem_Type(Item_Type item_type, string api_key);
    public Task<ServiceResult> UpdateItem_Type(Item_Type item_type, int item_type_id, string api_key);
    public Task<ServiceResult> DeleteItem_Type(int item_type_id, string api_key);
}