using System.ComponentModel.DataAnnotations;

namespace WarehouseEFApp.DTOs;

public class ProductReadDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public DateOnly DateAdded { get; set; }
}

public class ProductCreateDTO
{
    [Required(ErrorMessage = "Name is mandatory")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Name has to be from 2 to 150 symbols")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "ID is mandatory")]
    [Range(1, int.MaxValue, ErrorMessage = "ID has to be grater than 0")]
    public int CategoryId { get; set; }
}

public class ProductUpdateDTO
{
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Name has to be from 2 to 150 symbols")]
    public string? Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "ID has to be grater than 0")]
    public int? CategoryId { get; set; }
}
