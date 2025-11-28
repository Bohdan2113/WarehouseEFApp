using System;
using System.Collections.Generic;

namespace WarehouseEFApp.Models;

public partial class TransferDocument
{
    public int Id { get; set; }

    public DateOnly TransferDate { get; set; }

    public int SourceWarehouseId { get; set; }

    public int ReceivingWarehouseId { get; set; }

    public virtual Warehouse ReceivingWarehouse { get; set; } = null!;

    public virtual Warehouse SourceWarehouse { get; set; } = null!;
}
