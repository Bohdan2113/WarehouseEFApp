using System;
using System.Collections.Generic;

namespace WarehouseEFApp.Models;

public partial class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Position { get; set; }

    public virtual ICollection<DeliveryDocument> DeliveryDocuments { get; set; } = new List<DeliveryDocument>();

    public virtual ICollection<ShippingDocument> ShippingDocuments { get; set; } = new List<ShippingDocument>();

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
