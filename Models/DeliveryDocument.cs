using System;
using System.Collections.Generic;

namespace WarehouseEFApp.Models;

public partial class DeliveryDocument
{
    public int Id { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int? Supplier { get; set; }

    public int WarehouseId { get; set; }

    public virtual Person? SupplierNavigation { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;
}
