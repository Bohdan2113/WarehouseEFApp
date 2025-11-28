using System;
using System.Collections.Generic;

namespace WarehouseEFApp.Models;

public partial class VProductMovementAnalysis
{
    public int? ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? CategoryName { get; set; }

    public decimal? TotalDelivered { get; set; }

    public decimal? TotalShipped { get; set; }

    public decimal? CriticalDeliveriesQty { get; set; }
}
