using System;
using System.Collections.Generic;

namespace WarehouseEFApp.Models;

public partial class ShippingDocument
{
    public int Id { get; set; }

    public DateOnly ShippingDate { get; set; }

    public int? Recipient { get; set; }

    public int WarehouseId { get; set; }

    public virtual Person? RecipientNavigation { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;
}
