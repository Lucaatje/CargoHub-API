using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class OrderService : IOrderService
{
    private readonly DatabaseContext _context;
    private readonly IItemService _itemService;
    private readonly IInventoryService _inventoryService;

    public OrderService(DatabaseContext DbContext, IItemService itemService, IInventoryService inventoryService)
    {
        _context = DbContext;
        _itemService = itemService;
        _inventoryService = inventoryService;
    }

    public async Task<ServiceResult> ReadOrder(int order_id, string api_key)
    {
        try
        {
            var order = await _context.Orders.FindAsync(order_id);

            if (order == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such order with id: {order_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such order with id: {order_id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching order", api_key);
            return new ServiceResult { Object = order, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch order with id {order_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadOrders(string api_key)
    {
        try
        {
            var orders = await _context.Orders.ToListAsync();

            if (!orders.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No orders found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No orders found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple orders", api_key);
            return new ServiceResult { Object = orders, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple orders - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadItemsInOrder(int order_id, string api_key)
    {
        try
        {
            var order = await _context.Orders.FindAsync(order_id);

            if (order == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such order with id: {order_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such order with id: {order_id}" };
            }

            var itemIds = order.Items.Select(item => item.Item_Id).ToList();
            var items = await _context.Items.Where(item => itemIds.Contains(item.UId)).ToListAsync();

            if (!items.Any())
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No items for order found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No items for order found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching items for order", api_key);
            return new ServiceResult { Object = items, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch items for order - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateOrder(Order order, string api_key)
    {
        try
        {
            if (_context.Orders.Any(x => x.Id == order.Id))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Id {order.Id} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Id {order.Id} already in use" };
            }

            order.Created_At = DateTime.UtcNow;
            order.Updated_At = DateTime.UtcNow;
            _context.Orders.Add(order);
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create order", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create order, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Order created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create order with id {order.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateOrder(Order order, int order_id, string api_key)
    {
        try
        {
            var existingOrder = await _context.Orders.FindAsync(order_id);
            if (existingOrder == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Order not found with id {order_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Order not found with id {order_id}" };
            }

            existingOrder.Source_Id = order.Source_Id;
            existingOrder.Order_Date = order.Order_Date;
            existingOrder.Request_Date = order.Request_Date;
            existingOrder.Reference = order.Reference;
            existingOrder.Reference_Extra = order.Reference_Extra;
            existingOrder.Order_Status = order.Order_Status;
            existingOrder.Notes = order.Notes;
            existingOrder.Shipping_Notes = order.Shipping_Notes;
            existingOrder.Picking_Notes = order.Picking_Notes;
            existingOrder.Warehouse_Id = order.Warehouse_Id;
            existingOrder.Ship_To = order.Ship_To;
            existingOrder.Bill_To = order.Bill_To;
            existingOrder.Shipment_Id = order.Shipment_Id;
            existingOrder.Total_Amount = order.Total_Amount;
            existingOrder.Total_Discount = order.Total_Discount;
            existingOrder.Total_Tax = order.Total_Tax;
            existingOrder.Total_Surcharge = order.Total_Surcharge;
            existingOrder.Updated_At = DateTime.UtcNow;
            existingOrder.Items = order.Items;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update order with id {order_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update order, please try again with id {order_id}" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated order succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update order with id {order.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateItemsInOrder(int order_id, List<PropertyItem> updated_items, string api_key)
    {
        try
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == order_id);

            if (order == null || order.Items == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: No such order with id: {order_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such order with id: {order_id}" };
            }

            foreach (PropertyItem currentItem in order.Items)
            {
                var found = updated_items.FirstOrDefault(item => item.Item_Id == currentItem.Item_Id);
                if (found == null)
                {
                    var result = await _itemService.ReadInventoriesForItem(currentItem.Item_Id, api_key);
                    if (result.Object == null)
                    {
                        await AuditLogService.LogActionAsync("PUT", $"WARNING: No inventories found for item {currentItem.Item_Id}", api_key);
                        continue;
                    }

                    var inventories = (List<Inventory>)result.Object;
                    var minInventory = inventories?.OrderByDescending(inventory => inventory.Total_Allocated).FirstOrDefault();
                    if (minInventory != null)
                    {
                        minInventory.Total_Allocated -= currentItem.Amount;
                        minInventory.Total_Expected = minInventory.Total_On_Hand + minInventory.Total_Ordered;
                        await _inventoryService.UpdateInventory(minInventory, minInventory.Id, api_key);
                    }
                }
            }

            foreach (PropertyItem updatedItem in updated_items)
            {
                var currentItem = order.Items.FirstOrDefault(item => item.Item_Id == updatedItem.Item_Id);
                if (currentItem != null)
                {
                    var result = await _itemService.ReadInventoriesForItem(updatedItem.Item_Id, api_key);
                    if (result.Object == null)
                    {
                        await AuditLogService.LogActionAsync("PUT", $"WARNING: No inventories found for updated item {updatedItem.Item_Id}", api_key);
                        continue;
                    }

                    var inventories = (List<Inventory>)result.Object;
                    var minInventory = inventories?.OrderBy(inventory => inventory.Total_Allocated).FirstOrDefault();
                    if (minInventory != null)
                    {
                        minInventory.Total_Allocated += updatedItem.Amount - currentItem.Amount;
                        minInventory.Total_Expected = minInventory.Total_On_Hand + minInventory.Total_Ordered;
                        await _inventoryService.UpdateInventory(minInventory, minInventory.Id, api_key);
                    }
                }
                else
                {
                    var result = await _itemService.ReadInventoriesForItem(updatedItem.Item_Id, api_key);
                    if (result.Object == null)
                    {
                        await AuditLogService.LogActionAsync("PUT", $"WARNING: No inventories found for new item {updatedItem.Item_Id}", api_key);
                        continue;
                    }

                    var inventories = (List<Inventory>)result.Object;
                    var minInventory = inventories?.OrderBy(inventory => inventory.Total_Allocated).FirstOrDefault();
                    if (minInventory != null)
                    {
                        minInventory.Total_Allocated += updatedItem.Amount;
                        minInventory.Total_Expected = minInventory.Total_On_Hand + minInventory.Total_Ordered;
                        await _inventoryService.UpdateInventory(minInventory, minInventory.Id, api_key);
                    }
                }
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated items in order successfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update items in order - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteOrder(int order_id, string api_key)
    {
        try
        {
            var order = await _context.Orders.FindAsync(order_id);
            if (order == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Order with id {order_id} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Order with id {order_id} already not in database" };
            }
            _context.Orders.Remove(order);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete order with id {order_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete order with id {order_id}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted order succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to delete order with id {order_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
}

public interface IOrderService
{
    public Task<ServiceResult> ReadOrder(int order_id, string api_key);
    public Task<ServiceResult> ReadOrders(string api_key);
    public Task<ServiceResult> ReadItemsInOrder(int order_id, string api_key);
    public Task<ServiceResult> CreateOrder(Order order, string api_key);
    public Task<ServiceResult> UpdateOrder(Order order, int order_id, string api_key);
    public Task<ServiceResult> UpdateItemsInOrder(int order_id, List<PropertyItem> updated_items, string api_key);
    public Task<ServiceResult> DeleteOrder(int order_id, string api_key);
}