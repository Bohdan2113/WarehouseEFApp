using System;
using System.Collections.Generic;

namespace WarehouseEFApp.Models;

public partial class TransactionLine
{
    public int Id { get; set; }

    public int TransactionId { get; set; }

    public int ProductId { get; set; }

    public decimal Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}
