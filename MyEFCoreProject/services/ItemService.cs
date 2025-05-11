using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

public class ItemService : IItemService
{
    private readonly DatabaseContext _context;

    public ItemService(DatabaseContext DbContext)
    {
        _context = DbContext;
    }

    public async Task<ServiceResult> ReadItem(string item_uid, string api_key)
    {
        try
        {
            var item = await _context.Items.FindAsync(item_uid);

            if (item == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such item with uid: {item_uid}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such item with uid: {item_uid}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching item", api_key);
            return new ServiceResult { Object = item, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch item with uid {item_uid} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadItems(string api_key)
    {
        try
        {
            var items = await _context.Items.ToListAsync();

            if (!items.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No items found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No items found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple items", api_key);
            return new ServiceResult { Object = items, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple items - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadInventoriesForItem(string item_uid, string api_key)
    {
        try
        {
            var inventories = await _context.Inventories.Where(inventory => inventory.Item_Id == item_uid).ToListAsync();

            if (!inventories.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No inventories for item found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No inventories for item found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching inventories for item", api_key);
            return new ServiceResult { Object = inventories, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch inventories for item - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadInventoryTotalsForItem(string item_uid, string api_key)
    {
        try
        {
            if (await _context.Items.FindAsync(item_uid) == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such item with uid: {item_uid}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such item with uid: {item_uid}" };
            }

            var inventoryTotals = await _context.Inventories.Where(inventory => inventory.Item_Id == item_uid)
                .GroupBy(inventory => inventory.Item_Id)
                .Select(group => new
                {
                    total_expected = group.Sum(inventory => inventory.Total_Expected),
                    total_ordered = group.Sum(inventory => inventory.Total_Ordered),
                    total_allocated = group.Sum(inventory => inventory.Total_Allocated),
                    total_available = group.Sum(inventory => inventory.Total_Available)
                }).FirstOrDefaultAsync() ?? new { total_expected = 0, total_ordered = 0, total_allocated = 0, total_available = 0 };
            
            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching inventory totals for item", api_key);
            return new ServiceResult { Object = inventoryTotals, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch inventory totals for item - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateItem(Item item, string api_key)
    {
        try
        {
            if (_context.Items.Any(x => x.UId == item.UId))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Uid {item.UId} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Uid {item.UId} already in use" };
            }

            item.Created_At = DateTime.UtcNow;
            item.Updated_At = DateTime.UtcNow;
            _context.Items.Add(item);
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create item", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create item, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Item created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create item with uid {item.UId} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateItem(Item item, string item_uid, string api_key)
    {
        try
        {
            var existingItem = await _context.Items.FindAsync(item_uid);
            if (existingItem == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Item not found with uid {item_uid}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Item not found with uid {item_uid}" };
            }

            existingItem.Code = item.Code;
            existingItem.Description = item.Description;
            existingItem.Short_Description = item.Short_Description;
            existingItem.Upc_Code = item.Upc_Code;
            existingItem.Model_Number = item.Model_Number;
            existingItem.Commodity_Code = item.Commodity_Code;
            existingItem.Item_Line = item.Item_Line;
            existingItem.Item_Group = item.Item_Group;
            existingItem.Item_Type = item.Item_Type;
            existingItem.Unit_Purchase_Quantity = item.Unit_Purchase_Quantity;
            existingItem.Unit_Order_Quantity = item.Unit_Order_Quantity;
            existingItem.Pack_Order_Quantity = item.Pack_Order_Quantity;
            existingItem.Supplier_Id = item.Supplier_Id;
            existingItem.Supplier_Code = item.Supplier_Code;
            existingItem.Supplier_Part_Number = item.Supplier_Part_Number;
            existingItem.Updated_At = DateTime.UtcNow;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update item with uid {item_uid}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update item, please try again with uid {item_uid}" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated item succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update item with uid {item.UId} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteItem(string item_uid, string api_key)
    {
        try
        {
            var item = await _context.Items.FindAsync(item_uid);
            if (item == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Item with uid {item_uid} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Item with uid {item_uid} already not in database" };
            }
            _context.Items.Remove(item);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete item with uid {item_uid}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete item with uid {item_uid}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted item succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to delete item with uid {item_uid} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
}

public interface IItemService
{
    public Task<ServiceResult> ReadItem(string item_uid, string api_key);
    public Task<ServiceResult> ReadItems(string api_key);
    public Task<ServiceResult> ReadInventoriesForItem(string item_uid, string api_key);
    public Task<ServiceResult> ReadInventoryTotalsForItem(string item_uid, string api_key);
    public Task<ServiceResult> CreateItem(Item item, string api_key);
    public Task<ServiceResult> UpdateItem(Item item, string item_uid, string api_key);
    public Task<ServiceResult> DeleteItem(string item_uid, string api_key);
}