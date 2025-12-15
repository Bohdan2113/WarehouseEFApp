using System.ComponentModel.DataAnnotations;

namespace WarehouseEFApp.DTOs;

public class CategoryReadDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class CategoryCreateUpdateDTO
{
    [Required(ErrorMessage = "Name is mandatory")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name has to be from 2 to 100 ")]
    public string Name { get; set; } = null!;
}
