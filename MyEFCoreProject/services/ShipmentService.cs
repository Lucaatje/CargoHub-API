using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ShipmentService : IShipmentService
{
    private readonly DatabaseContext _context;

    public ShipmentService(DatabaseContext DbContext)
    {
        _context = DbContext;
    }
    public async Task<ServiceResult> ReadShipment(int shipment_id, string api_key)
    {
        try
        {
            var shipment = await _context.Shipments.FindAsync(shipment_id);

            if (shipment == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such shipment with id: {shipment_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such shipment with id: {shipment_id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching shipment", api_key);
            return new ServiceResult { Object = shipment, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch shipment with id {shipment_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> GetAllShipments(int page, string api_key)
    {
        try
        {
            const int defaultPageSize = 200; 

            var shipments =  await _context.Shipments
                                .AsNoTracking()
                                .Skip((page - 1) * defaultPageSize) 
                                .Take(defaultPageSize) 
                                .ToListAsync();
            if (!shipments.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No shipments found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No shipments found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple shipments", api_key);
            return new ServiceResult { Object = shipments, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple shipments - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateShipment(Shipment shipment, string api_key)
    {
        try
        {
            if (_context.Shipments.Any(x => x.Id == shipment.Id))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Id {shipment.Id} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Id {shipment.Id} already in use" };
            }

            shipment.Created_At = DateTime.UtcNow;
            shipment.Updated_At = DateTime.UtcNow;
            _context.Shipments.Add(shipment);
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create shipment", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create shipment, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Shipment created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create shipment with id {shipment.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateShipmentItems(List<PropertyItem> items, int shipment_id, string api_key)
    {
        try
        {
            var shipment = await _context.Shipments.FindAsync(shipment_id);

            if (shipment == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Shipment not found with id {shipment_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Shipment not found with id {shipment_id}" };
            }

            shipment.Items = items;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update items in shipment", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update items in shipment, please try again" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated items in shipment succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update items in shipment - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateShipmentOrder(Order order, int shipment_id, string api_key)
    {
        try
        {
            var shipment = await _context.Shipments.FindAsync(shipment_id);

            if (shipment == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Shipment not found with id {shipment_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Shipment not found with id {shipment_id}" };
            }

            shipment.Order_Id = order.Id;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update order in shipment", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update order in shipment, please try again" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated order in shipment succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update order in shipment - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
    public async Task<ServiceResult> UpdateShipment(Shipment shipment, int shipment_id, string api_key)
    {
        try
        {
            var existingShipment = await _context.Shipments.FindAsync(shipment_id);
            if (existingShipment == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Shipment not found with id {shipment_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Shipment not found with id {shipment_id}" };
            }

            existingShipment.Order_Date = shipment.Order_Date;
            existingShipment.Request_Date = shipment.Request_Date;
            existingShipment.Shipment_Date = shipment.Shipment_Date;
            existingShipment.Shipment_Type = shipment.Shipment_Type;
            existingShipment.Shipment_Status = shipment.Shipment_Status;
            existingShipment.Notes = shipment.Notes;
            existingShipment.Carrier_Code = shipment.Carrier_Code;
            existingShipment.Carrier_Description = shipment.Carrier_Code;
            existingShipment.Service_Code = shipment.Service_Code;
            existingShipment.Payment_Type = shipment.Payment_Type;
            existingShipment.Transfer_Mode = shipment.Transfer_Mode;
            existingShipment.Total_Package_Count = shipment.Total_Package_Count;
            existingShipment.Total_Package_Weight = shipment.Total_Package_Weight;
            existingShipment.Items = shipment.Items;
            existingShipment.Updated_At = DateTime.UtcNow;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update shipment with id {shipment_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update shipment, please try again with id {shipment_id}" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated shipment succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update shipment with id {shipment.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteShipment(int shipment_id, string api_key)
    {
        try
        {
            var shipment = await _context.Shipments.FindAsync(shipment_id);
            if (shipment == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Shipment with id {shipment_id} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Shipment with id {shipment_id} already not in database" };
            }
            _context.Shipments.Remove(shipment);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete shipment with id {shipment_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete shipment with id {shipment_id}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted shipment succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete shipment with id {shipment_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadShipmentItems(int shipment_id, string api_key)
    {
        try
        {
            if (await _context.Shipments.FindAsync(shipment_id) == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: Shipment not found with id {shipment_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Shipment not found with id {shipment_id}" };
            }

            var items = await _context.Shipments
                .Where(x => x.Id == shipment_id)
                .Select(x => x.Items)
                .FirstOrDefaultAsync();
            
            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching items in shipment", api_key);
            return new ServiceResult { Object = items ?? new List<PropertyItem>(), StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch items in shipment - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadShipmentOrder(int shipment_id, string api_key)
    {
        try
        {
            var shipment = await _context.Shipments.FindAsync(shipment_id);

            if (shipment == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: Shipment not found with id {shipment_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Shipment not found with id {shipment_id}" };
            }

            var order = await _context.Orders.FindAsync(shipment.Order_Id);

            if (order == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: Order not found with id {shipment.Order_Id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Order not found with id {shipment.Order_Id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching order in shipment", api_key);
            return new ServiceResult { Object = order, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch order in shipment - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<bool> CommitShipment(int transfer_id)
    {
        var Shipment = await _context.Shipments.FindAsync(transfer_id);
        if (Shipment == null)
        {
            return false;
        }

        foreach (var item_set in Shipment.Items)
        {
            var inventory = await _context.Inventories
                                          .FirstOrDefaultAsync(x => x.Item_Id == item_set.Item_Id);
            if (inventory == null)
            {
                return false;
            }
            inventory.Total_On_Hand += item_set.Amount;
            _context.Inventories.Update(inventory);
        }
        Shipment.Shipment_Status = "Delivered";
        await _context.SaveChangesAsync();
        return true;
    }
}

public interface IShipmentService
{
    public Task<ServiceResult> ReadShipment(int shipment_id, string api_key);
    public Task<ServiceResult> GetAllShipments(int page, string api_key);
    public Task<ServiceResult> CreateShipment(Shipment shipment, string api_key);
    public Task<ServiceResult> UpdateShipment(Shipment shipment, int shipment_id, string api_key);
    public Task<ServiceResult> DeleteShipment(int shipment_id, string api_key);

    public Task<ServiceResult> ReadShipmentItems(int shipment_id, string api_key);
    public Task<ServiceResult> ReadShipmentOrder(int shipment_id, string api_key);

    public Task<ServiceResult> UpdateShipmentOrder(Order order, int shipment_id, string api_key);

    public Task<ServiceResult> UpdateShipmentItems(List<PropertyItem> items, int shipment_id, string api_key);

    public Task<bool> CommitShipment(int trasnfer_id);
}