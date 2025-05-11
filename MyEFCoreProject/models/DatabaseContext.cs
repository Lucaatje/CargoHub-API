using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

public class DatabaseContext : DbContext
{
    public DbSet<Api_Key> Api_Keys { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Item_Group> Item_Groups { get; set; }
    public DbSet<Item_Line> Item_Lines { get; set; }
    public DbSet<Item_Type> Item_Types { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>()
            .HasKey(i => i.UId);

        var permissionsConverter = new ValueConverter<Dictionary<string, bool>, string>(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<Dictionary<string, bool>>(v) 
        );

        // modelBuilder.Entity<Api_Key>()
        //     .Property(a => a.Permissions)
        //     .HasConversion(permissionsConverter);
        
        var contactsConverter = new ValueConverter<Dictionary<string, string>, string>(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v)
        );

        modelBuilder.Entity<Warehouse>()
            .Property(a => a.Contact)
            .HasConversion(contactsConverter);
        
        var itemsConverter = new ValueConverter<List<PropertyItem>, string>(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<List<PropertyItem>>(v)
        );

        modelBuilder.Entity<Shipment>()
            .Property(a => a.Items)
            .HasConversion(itemsConverter);
            
        
        modelBuilder.Entity<Order>()
            .Property(a => a.Items)
            .HasConversion(itemsConverter);
        
        modelBuilder.Entity<Transfer>()
            .Property(a => a.Items)
            .HasConversion(itemsConverter);
        
        

    }
}