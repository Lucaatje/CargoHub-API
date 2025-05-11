using System;
using System.Collections.Generic;

public class Api_Key
{
    public int Id { get; set; }
    public string ApiKey { get; set; }
    public string App { get; set; }
    public Dictionary<string, bool> Permissions { get; set; }

}

public class ServiceResult
{
    public object Object { get; set; }
    public int StatusCode { get; set; }
    public string ErrorMessage { get; set; }
}

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Zip_Code { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public string Contact_Name { get; set; }
    public string Contact_Phone { get; set; }
    public string Contact_Email { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
}

public class Inventory
{
    public int Id { get; set; }
    public string Item_Id { get; set; }
    public string Description { get; set; }
    public string Item_Reference { get; set; }
    public List<int> Locations { get; set; }
    public int Total_On_Hand { get; set; }
    public int Total_Expected { get; set; }
    public int Total_Ordered { get; set; }
    public int Total_Allocated { get; set; }
    public int Total_Available { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
}

public class Item_Group
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
}

public class Item_Line
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
}

public class Item_Type
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
}

public class Item
{
    public string UId { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public string Short_Description { get; set; }
    public string Upc_Code { get; set; }
    public string Model_Number { get; set; }
    public string Commodity_Code { get; set; }
    public int Item_Line { get; set; }
    public int Item_Group { get; set; }
    public int Item_Type { get; set; }
    public int Unit_Purchase_Quantity { get; set; }
    public int Unit_Order_Quantity { get; set; }
    public int Pack_Order_Quantity { get; set; }
    public int Supplier_Id { get; set; }
    public string Supplier_Code { get; set; }
    public string Supplier_Part_Number { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
}

public class Location
{
    public int Id { get; set; }
    public int Warehouse_Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int Source_Id { get; set; }
    public string Order_Date { get; set; }
    public string Request_Date { get; set; }
    public string Reference { get; set; }
    public string Reference_Extra { get; set; }
    public string Order_Status { get; set; }
    public string Notes { get; set; }
    public string Shipping_Notes { get; set; }
    public string Picking_Notes { get; set; }
    public int Warehouse_Id { get; set; }
    public int? Ship_To { get; set; }
    public int? Bill_To { get; set; }
    public int? Shipment_Id { get; set; }
    public double Total_Amount { get; set; }
    public double Total_Discount { get; set; }
    public double Total_Tax { get; set; }
    public double Total_Surcharge { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
    public List<PropertyItem> Items { get; set; }
}

public class Shipment
{
    public int Id { get; set; }
    public int Order_Id { get; set; }
    public int Source_Id { get; set; }
    public string Order_Date { get; set; }
    public string Request_Date { get; set; }
    public string Shipment_Date { get; set; }
    public string Shipment_Type { get; set; }
    public string Shipment_Status { get; set; }
    public string Notes { get; set; }
    public string Carrier_Code { get; set; }
    public string Carrier_Description { get; set; }
    public string Service_Code { get; set; }
    public string Payment_Type { get; set; }
    public string Transfer_Mode { get; set; }
    public int Total_Package_Count { get; set; }
    public double Total_Package_Weight { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
    public List<PropertyItem> Items { get; set; }
}

public class Supplier
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Address_Extra { get; set; }
    public string City { get; set; }
    public string Zip_Code { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public string Contact_Name { get; set; }
    public string Phonenumber { get; set; }
    public string Reference { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
}

public class Transfer
{
    public int Id { get; set; }
    public string Reference { get; set; }
    public string? Transfer_From { get; set; }
    public string Transfer_To { get; set; }
    public string Transfer_Status { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
    public List<PropertyItem> Items { get; set; }
}

public class Warehouse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public Dictionary<string, string> Contact { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
}

public class PropertyItem
{
    public string Item_Id { get; set; }
    public int Amount { get; set; }
}