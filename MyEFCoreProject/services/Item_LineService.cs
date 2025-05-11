using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class Item_LineService : IItem_LineService
{
    private readonly DatabaseContext _context;

    public Item_LineService(DatabaseContext DbContext)
    {
        _context = DbContext;
    }

    public async Task<ServiceResult> ReadItem_Line(int item_line_id, string api_key)
    {
        try
        {
            var item_line = await _context.Item_Lines.FindAsync(item_line_id);

            if (item_line == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such item_line with id: {item_line_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such item_line with id: {item_line_id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching item_line", api_key);
            return new ServiceResult { Object = item_line, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch item_line with id {item_line_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadItem_Lines(string api_key)
    {
        try
        {
            var item_lines = await _context.Item_Lines.ToListAsync();

            if (!item_lines.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No item_lines found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No item_lines found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple item_lines", api_key);
            return new ServiceResult { Object = item_lines, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple item_lines - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadItemsForItem_Line(int item_line_id, string api_key)
    {
        try
        {
            var items = await _context.Items.Where(item => item.Item_Line == item_line_id).ToListAsync();

            if (!items.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No items for item_line found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No items for item_line found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching items for item_line", api_key);
            return new ServiceResult { Object = items, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch items for item_line - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateItem_Line(Item_Line item_line, string api_key)
    {
        try
        {
            if (_context.Item_Lines.Any(x => x.Id == item_line.Id))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Id {item_line.Id} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Id {item_line.Id} already in use" };
            }

            item_line.Created_At = DateTime.UtcNow;
            item_line.Updated_At = DateTime.UtcNow;
            _context.Item_Lines.Add(item_line);
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create item_line", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create item_line, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Item_line created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create item_line with id {item_line.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateItem_Line(Item_Line item_line, int item_line_id, string api_key)
    {
        try
        {
            var existingItem_Line = await _context.Item_Lines.FindAsync(item_line_id);
            if (existingItem_Line == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Item_line not found with id {item_line_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Item_line not found with id {item_line_id}" };
            }

            existingItem_Line.Name = item_line.Name;
            existingItem_Line.Description = item_line.Description;
            existingItem_Line.Updated_At = DateTime.UtcNow;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update item_line with id {item_line_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update item_line, please try again with id {item_line_id}" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated item_line succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update item_line with id {item_line.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteItem_Line(int item_line_id, string api_key)
    {
        try
        {
            var item_line = await _context.Item_Lines.FindAsync(item_line_id);
            if (item_line == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Item_line with id {item_line_id} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Item_line with id {item_line_id} already not in database" };
            }
            _context.Item_Lines.Remove(item_line);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete item_line with id {item_line_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete item_line with id {item_line_id}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted item_line succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to delete item_line with id {item_line_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
}

public interface IItem_LineService
{
    public Task<ServiceResult> ReadItem_Line(int item_line_id, string api_key);
    public Task<ServiceResult> ReadItem_Lines(string api_key);
    public Task<ServiceResult> ReadItemsForItem_Line(int item_line_id, string api_key);
    public Task<ServiceResult> CreateItem_Line(Item_Line item_line, string api_key);
    public Task<ServiceResult> UpdateItem_Line(Item_Line item_line, int item_line_id, string api_key);
    public Task<ServiceResult> DeleteItem_Line(int item_line_id, string api_key);
}