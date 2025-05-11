using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class CommitTests {

    [Fact]
    public async Task CommitTransfer_ValidTransfer_UpdatesInventoryAndReturnsTrue()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new DatabaseContext(options);
        var transfer = new Transfer
        {
            Id = 1,
            Reference = "ABC123",  
            Transfer_To = "LocationB",
            Items = new List<PropertyItem>
            {
                new PropertyItem { Item_Id = "1", Amount = 10 }
            },
            Transfer_Status = "Pending"
        };
        context.Transfers.Add(transfer);
        context.Inventories.Add(new Inventory
        {
            Item_Id = "1",
            Total_On_Hand = 5,
            Locations = new List<int> { 1 },
            Description = "Test Item", 
            Item_Reference = "REF123" 
        });
        await context.SaveChangesAsync();

        var service = new TransferService(context);

        var result = await service.CommitTransfer(1);

        Assert.True(result);
        var updatedInventory = await context.Inventories.FirstOrDefaultAsync(x => x.Item_Id == "1");
        Assert.Equal(15, updatedInventory.Total_On_Hand);
        var updatedTransfer = await context.Transfers.FindAsync(1);
        Assert.Equal("Completed", updatedTransfer.Transfer_Status);
    }

    [Fact]
    public async Task CommitTransfer_TransferNotFound_ReturnsFalse()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new DatabaseContext(options);
        var service = new TransferService(context);

        var result = await service.CommitTransfer(1);

        Assert.False(result);
    }

    [Fact]
    public async Task CommitTransfer_InventoryNotFound_ReturnsFalse()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new DatabaseContext(options);
        var transfer = new Transfer
        {
            Id = 2,
            Reference = "T123",
            Transfer_To = "Warehouse B",
            Items = new List<PropertyItem>
            {
                new PropertyItem{ Item_Id = "1", Amount = 10 }
            },
            Transfer_Status = "Pending"
        };
        context.Transfers.Add(transfer);
        await context.SaveChangesAsync();

        var service = new TransferService(context);

        var result = await service.CommitTransfer(1);

        Assert.False(result);
    }

}

