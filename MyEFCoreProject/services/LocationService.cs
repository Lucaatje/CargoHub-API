using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class LocationService : ILocationService
{
    private readonly DatabaseContext _context;

    public LocationService(DatabaseContext DbContext)
    {
        _context = DbContext;
    }

    public async Task<ServiceResult> ReadLocation(int location_id, string api_key)
    {
        try
        {
            var location = await _context.Locations.FindAsync(location_id);

            if (location == null)
            {
                await AuditLogService.LogActionAsync("GET", $"404 NOT FOUND: No such location with id: {location_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"No such location with id: {location_id}" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching location", api_key);
            return new ServiceResult { Object = location, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to fetch location with id {location_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> ReadLocations(string api_key)
    {
        try
        {
            var locations = await _context.Locations.ToListAsync();

            if (!locations.Any())
            {
                await AuditLogService.LogActionAsync("GET", "404 NOT FOUND: No locations found", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = "No locations found" };
            }

            await AuditLogService.LogActionAsync("GET", "200 OK: Fetching multiple locations", api_key);
            return new ServiceResult { Object = locations, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("GET", $"500 INTERNAL SERVER ERROR: Failed to Fetch multiple locations - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> CreateLocation(Location location, string api_key)
    {
        try
        {
            if (_context.Locations.Any(x => x.Id == location.Id))
            {
                await AuditLogService.LogActionAsync("POST", $"409 ALREADY EXISTS: Id {location.Id} already in use", api_key);
                return new ServiceResult { StatusCode = 409, ErrorMessage = $"Id {location.Id} already in use" };
            }

            location.Created_At = DateTime.UtcNow;
            location.Updated_At = DateTime.UtcNow;
            _context.Locations.Add(location);
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("POST", "500 INTERNAL SERVER ERROR: Failed to create location", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = "Failed to create location, please try again" };
            }

            await AuditLogService.LogActionAsync("POST", "200 OK: Location created succesfully", api_key );
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to create location with id {location.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> UpdateLocation(Location location, int location_id, string api_key)
    {
        try
        {
            var existingLocation = await _context.Locations.FindAsync(location_id);
            if (existingLocation == null)
            {
                await AuditLogService.LogActionAsync("PUT", $"404 NOT FOUND: Location not found with id {location_id}", api_key);
                return new ServiceResult { StatusCode = 404, ErrorMessage = $"Location not found with id {location_id}" };
            }

            existingLocation.Warehouse_Id = location.Warehouse_Id;
            existingLocation.Code = location.Code;
            existingLocation.Name = location.Name;
            existingLocation.Updated_At = DateTime.UtcNow;
            int n = await _context.SaveChangesAsync();

            if (n == 0)
            {
                await AuditLogService.LogActionAsync("PUT", $"500 INTERNAL SERVER ERROR: Failed to update location with id {location_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to update location, please try again with id {location_id}" };
            }

            await AuditLogService.LogActionAsync("PUT", "200 OK: Updated location succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to update location with id {location.Id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }

    public async Task<ServiceResult> DeleteLocation(int location_id, string api_key)
    {
        try
        {
            var location = await _context.Locations.FindAsync(location_id);
            if (location == null)
            {
                await AuditLogService.LogActionAsync("DELETE", $"400 BADREQUEST: Location with id {location_id} already not in database", api_key);
                return new ServiceResult { StatusCode = 400, ErrorMessage = $"Location with id {location_id} already not in database" };
            }
            _context.Locations.Remove(location);
            int n = await _context.SaveChangesAsync();
            
            if (n == 0)
            {
                await AuditLogService.LogActionAsync("DELETE", $"500 INTERNAL SERVER ERROR: Failed to delete location with id {location_id}", api_key);
                return new ServiceResult { StatusCode = 500, ErrorMessage = $"Failed to delete location with id {location_id}, please try again" };
            }

            await AuditLogService.LogActionAsync("DELETE", "200 OK: Deleted location succesfully", api_key);
            return new ServiceResult { StatusCode = 200 };
        }
        catch (Exception ex)
        {
            await AuditLogService.LogActionAsync("POST", $"500 INTERNAL SERVER ERROR: Failed to delete location with id {location_id} - {ex.Message}", api_key);
            return new ServiceResult { StatusCode = 500, ErrorMessage = ex.Message };
        }
    }
}

public interface ILocationService
{
    public Task<ServiceResult> ReadLocation(int location_id, string api_key);
    public Task<ServiceResult> ReadLocations(string api_key);
    public Task<ServiceResult> CreateLocation(Location location, string api_key);
    public Task<ServiceResult> UpdateLocation(Location location, int location_id, string api_key);
    public Task<ServiceResult> DeleteLocation(int location_id, string api_key);
}