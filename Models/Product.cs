using System;
using System.Collections.Generic;

namespace WarehouseEFApp.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }

    public DateOnly DateAdded { get; set; }

    public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<TransactionLine> TransactionLines { get; set; } = new List<TransactionLine>();
}
