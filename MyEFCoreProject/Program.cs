using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IItem_GroupService, Item_GroupService>();
builder.Services.AddScoped<IItem_LineService, Item_LineService>();
builder.Services.AddScoped<IItem_TypeService, Item_TypeService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<ApiKeyService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseRouting();

app.UseSession();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

// Add authorization middleware if required
// app.UseAuthorization(); 
// to run on the ssh: 
// cd Cargohub-inf2 ->
// cd MyEFCoreProject ->
// nohup dotnet run --urls "http://0.0.0.0:5072" > output.log 2>&1 &
app.Run();