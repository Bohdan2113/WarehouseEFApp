using System;
using System.Collections.Generic;

namespace WarehouseEFApp.Models;

public partial class Availability
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public decimal Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
