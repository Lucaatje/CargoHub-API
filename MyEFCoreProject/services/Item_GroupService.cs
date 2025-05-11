using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class Item_GroupService : IItem_GroupService
{
    private readonly DatabaseContext _context;

    public Item_GroupService(DatabaseContext DbContext)
    {
        _context = DbContext;
    }

    public async Task<ServiceResult> ReadItem_Group(int item_group_id, string api_key)
    {
        try
        {
            var item_group = await _context.Item_Groups.FindAsync(item_group_id);

            if (item_group == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such item_group with id: {item_group_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such item_group with id: {item_group_id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching item_group", api_key);
            return new ServiceResult { Object = item_group, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch item_group with id {item_group_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadItem_Groups(string api_key)
    {
        try
        {
            var item_groups = await _context.Item_Groups.ToListAsync();

            if (!item_groups.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No item_groups found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No item_groups found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple item_groups", api_key);
            return new ServiceResult { Object = item_groups, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple item_groups - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadItemsForItem_Group(int item_group_id, string api_key)
    {
        try
        {
            var items = await _context.Items.Where(item => item.Item_Group == item_group_id).ToListAsync();

            if (!items.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No items for item_group found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No items for item_group found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching items for item_group", api_key);
            return new ServiceResult { Object = items, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch items for item_group - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateItem_Group(Item_Group item_group, string api_key)
    {
        try
        {
            if (_context.Item_Groups.Any(x => x.Id == item_group.Id))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Id {item_group.Id} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Id {item_group.Id} already in use" };
            }

            item_group.Created_At = DateTime.UtcNow;
            item_group.Updated_At = DateTime.UtcNow;
            _context.Item_Groups.Add(item_group);
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create item_group", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create item_group, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Item_group created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create item_group with id {item_group.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateItem_Group(Item_Group item_group, int item_group_id, string api_key)
    {
        try
        {
            var existingItem_Group = await _context.Item_Groups.FindAsync(item_group_id);
            if (existingItem_Group == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Item_group not found with id {item_group_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Item_group not found with id {item_group_id}" };
            }

            existingItem_Group.Name = item_group.Name;
            existingItem_Group.Description = item_group.Description;
            existingItem_Group.Updated_At = DateTime.UtcNow;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update item_group with id {item_group_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update item_group, please try again with id {item_group_id}" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated item_group succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update item_group with id {item_group.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteItem_Group(int item_group_id, string api_key)
    {
        try
        {
            var item_group = await _context.Item_Groups.FindAsync(item_group_id);
            if (item_group == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Item_group with id {item_group_id} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Item_group with id {item_group_id} already not in database" };
            }
            _context.Item_Groups.Remove(item_group);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete item_group with id {item_group_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete item_group with id {item_group_id}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted item_group succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to delete item_group with id {item_group_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
}

public interface IItem_GroupService
{
    public Task<ServiceResult> ReadItem_Group(int item_group_id, string api_key);
    public Task<ServiceResult> ReadItem_Groups(string api_key);
    public Task<ServiceResult> ReadItemsForItem_Group(int item_group_id, string api_key);
    public Task<ServiceResult> CreateItem_Group(Item_Group item_group, string api_key);
    public Task<ServiceResult> UpdateItem_Group(Item_Group item_group, int item_group_id, string api_key);
    public Task<ServiceResult> DeleteItem_Group(int item_group_id, string api_key);
}