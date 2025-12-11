using System.ComponentModel.DataAnnotations;

namespace WarehouseEFApp.DTOs;

/// <summary>
/// DTO для продукту при читанні
/// </summary>
public class ProductReadDTO
{
    /// <summary>
    /// Унікальний ідентифікатор продукту
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Назва продукту
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// ID категорії
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Назва категорії (для інформації)
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Дата додавання продукту
    /// </summary>
    public DateOnly DateAdded { get; set; }
}

/// <summary>
/// DTO для створення продукту
/// </summary>
public class ProductCreateDTO
{
    /// <summary>
    /// Назва продукту (обов'язкова, макс 150 символів)
    /// </summary>
    [Required(ErrorMessage = "Назва продукту є обов'язковою")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Назва повинна бути від 2 до 150 символів")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// ID категорії (обов'язковий)
    /// </summary>
    [Required(ErrorMessage = "ID категорії є обов'язковим")]
    [Range(1, int.MaxValue, ErrorMessage = "ID категорії повинен бути більше 0")]
    public int CategoryId { get; set; }
}

/// <summary>
/// DTO для оновлення продукту
/// </summary>
public class ProductUpdateDTO
{
    /// <summary>
    /// Назва продукту (опціональна)
    /// </summary>
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Назва повинна бути від 2 до 150 символів")]
    public string? Name { get; set; }

    /// <summary>
    /// ID категорії (опціональний)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "ID категорії повинен бути більше 0")]
    public int? CategoryId { get; set; }
}
