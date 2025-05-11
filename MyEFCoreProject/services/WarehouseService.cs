using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class WarehouseService : IWarehouseService
{
    private readonly DatabaseContext _context;

    public WarehouseService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ServiceResult> ReadWarehouse(int warehouse_id, string api_key)
    {
        try
        {
            var warehouse = await _context.Warehouses.FindAsync(warehouse_id);

            if (warehouse == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such warehouse with id: {warehouse_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such warehouse with id: {warehouse_id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching warehouse", api_key);
            return new ServiceResult { Object = warehouse, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch warehouse with id {warehouse_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadWarehouses(string api_key)
    {
        try
        {
            var warehouses = await _context.Warehouses.ToListAsync();

            if (!warehouses.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No warehouses found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No warehouses found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple warehouses", api_key);
            return new ServiceResult { Object = warehouses, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple warehouses - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadLocationsInWarehouse(int warehouse_id, string api_key)
    {
        try
        {
            if (await _context.Warehouses.FindAsync(warehouse_id) == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such warehouse with id: {warehouse_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such warehouse with id: {warehouse_id}" };
            }

            var locations = await _context.Locations.Where(location => location.Warehouse_Id == warehouse_id).ToListAsync();

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching locations for warehouse", api_key);
            return new ServiceResult { Object = locations, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed fetching locations for warehouse - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateWarehouse(Warehouse warehouse, string api_key)
    {
        try
        {
            if (_context.Warehouses.Any(x => x.Id == warehouse.Id))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Id {warehouse.Id} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Id {warehouse.Id} already in use" };
            }

            warehouse.Created_At = DateTime.UtcNow;
            warehouse.Updated_At = DateTime.UtcNow;
            _context.Warehouses.Add(warehouse);
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create warehouse", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create warehouse, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Warehouse created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create warehouse with id {warehouse.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateWarehouse(Warehouse warehouse, int warehouse_id, string api_key)
    {
        try
        {
            var existingWarehouse = await _context.Warehouses.FindAsync(warehouse_id);
            if (existingWarehouse == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Transfer not found with id {warehouse_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Transfer not found with id {warehouse_id}" };
            }

            existingWarehouse.Code = warehouse.Code;
            existingWarehouse.Name = warehouse.Name;
            existingWarehouse.Address = warehouse.Address;
            existingWarehouse.Zip = warehouse.Zip;
            existingWarehouse.City = warehouse.City;
            existingWarehouse.Province = warehouse.Province;
            existingWarehouse.Country = warehouse.Country;
            existingWarehouse.Contact = warehouse.Contact;
            existingWarehouse.Updated_At = DateTime.UtcNow;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update warehouse with id {warehouse_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update warehouse, please try again with id {warehouse_id}" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated warehouse succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update warehouse with id {warehouse.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteWarehouse(int warehouse_id, string api_key)
    {
        try
        {
            var warehouse = await _context.Warehouses.FindAsync(warehouse_id);
            if (warehouse == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Warehouse with id {warehouse_id} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Warehouse with id {warehouse_id} already not in database" };
            }
            _context.Warehouses.Remove(warehouse);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete warehouse with id {warehouse_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete warehouse with id {warehouse_id}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted warehouse succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete warehouse with id {warehouse_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
}

public interface IWarehouseService
{
    public Task<ServiceResult> ReadWarehouse(int warehouse_id, string api_key);

    public Task<ServiceResult> ReadWarehouses(string api_key);
    public Task<ServiceResult> ReadLocationsInWarehouse(int warehouse_id, string api_key);
    public Task<ServiceResult> CreateWarehouse(Warehouse warehouse, string api_key);
    public Task<ServiceResult> UpdateWarehouse(Warehouse warehouse, int warehouse_id, string api_key);
    public Task<ServiceResult> DeleteWarehouse(int warehouse_id, string api_key);
}