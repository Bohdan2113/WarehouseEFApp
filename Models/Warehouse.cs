using System;
using System.Collections.Generic;

namespace WarehouseEFApp.Models;

public partial class Warehouse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int? ResponsiblePersonId { get; set; }

    public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();

    public virtual ICollection<DeliveryDocument> DeliveryDocuments { get; set; } = new List<DeliveryDocument>();

    public virtual Person? ResponsiblePerson { get; set; }

    public virtual ICollection<ShippingDocument> ShippingDocuments { get; set; } = new List<ShippingDocument>();

    public virtual ICollection<TransferDocument> TransferDocumentReceivingWarehouses { get; set; } = new List<TransferDocument>();

    public virtual ICollection<TransferDocument> TransferDocumentSourceWarehouses { get; set; } = new List<TransferDocument>();
}
