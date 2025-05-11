using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class SupplierService : ISupplierService
{
    private readonly DatabaseContext _context;

    public SupplierService(DatabaseContext DbContext)
    {
        _context = DbContext;
    }

    public async Task<ServiceResult> ReadSupplier(int supplier_id, string api_key)
    {
        try
        {
            var supplier = await _context.Suppliers.FindAsync(supplier_id);

            if (supplier == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such supplier with id: {supplier_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such supplier with id: {supplier_id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching supplier", api_key);
            return new ServiceResult { Object = supplier, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch supplier with id {supplier_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadSuppliers(string api_key)
    {
        try
        {
            var suppliers = await _context.Suppliers.ToListAsync();

            if (!suppliers.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No suppliers found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No suppliers found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple suppliers", api_key);
            return new ServiceResult { Object = suppliers, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple suppliers - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadItemsForSupplier(int supplier_id, string api_key)
    {
        try
        {
            if (await _context.Shipments.FindAsync(supplier_id) == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: Supplier not found with id {supplier_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Supplier not found with id {supplier_id}" };
            }

            var items = await _context.Items.Where(item => item.Supplier_Id == supplier_id).ToListAsync();

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching items for supplier", api_key);
            return new ServiceResult { Object = items, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch items for supplier - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateSupplier(Supplier supplier, string api_key)
    {
        try
        {
            if (_context.Suppliers.Any(x => x.Id == supplier.Id))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Id {supplier.Id} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Id {supplier.Id} already in use" };
            }

            supplier.Created_At = DateTime.UtcNow;
            supplier.Updated_At = DateTime.UtcNow;
            _context.Suppliers.Add(supplier);
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create supplier", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create supplier, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Supplier created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create supplier with id {supplier.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateSupplier(Supplier supplier, int supplier_id, string api_key)
    {
        try
        {
            var existingSupplier = await _context.Suppliers.FindAsync(supplier_id);
            if (existingSupplier == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Supplier not found with id {supplier_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Supplier not found with id {supplier_id}" };
            }

            existingSupplier.Code = supplier.Code;
            existingSupplier.Name = supplier.Name;
            existingSupplier.Address = supplier.Address;
            existingSupplier.Address_Extra = supplier.Address_Extra;
            existingSupplier.City = supplier.City;
            existingSupplier.Zip_Code = supplier.Zip_Code;
            existingSupplier.Province = supplier.Province;
            existingSupplier.Country = supplier.Country;
            existingSupplier.Contact_Name = supplier.Contact_Name;
            existingSupplier.Phonenumber = supplier.Phonenumber;
            existingSupplier.Reference = supplier.Reference;
            existingSupplier.Updated_At = DateTime.UtcNow;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update supplier with id {supplier_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update supplier, please try again with id {supplier_id}" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated supplier succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update supplier with id {supplier.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteSupplier(int supplier_id, string api_key)
    {
        try
        {
            var supplier = await _context.Suppliers.FindAsync(supplier_id);
            if (supplier == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Supplier with id {supplier_id} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Supplier with id {supplier_id} already not in database" };
            }
            _context.Suppliers.Remove(supplier);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete supplier with id {supplier_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete supplier with id {supplier_id}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted supplier succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete supplier with id {supplier_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
}

public interface ISupplierService
{
    public Task<ServiceResult> ReadSupplier(int supplier_id, string api_key);
    public Task<ServiceResult> ReadSuppliers(string api_key);
    public Task<ServiceResult> ReadItemsForSupplier(int supplier_id, string api_key);
    public Task<ServiceResult> CreateSupplier(Supplier supplier, string api_key);
    public Task<ServiceResult> UpdateSupplier(Supplier supplier, int supplier_id, string api_key);
    public Task<ServiceResult> DeleteSupplier(int supplier_id, string api_key);
}