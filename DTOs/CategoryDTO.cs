using System.ComponentModel.DataAnnotations;

namespace WarehouseEFApp.DTOs;

/// <summary>
/// DTO для категорії продукту при читанні
/// </summary>
public class CategoryReadDTO
{
    /// <summary>
    /// Унікальний ідентифікатор категорії
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Назва категорії (унікальна)
    /// </summary>
    public string Name { get; set; } = null!;
}

/// <summary>
/// DTO для創建 та оновлення категорії
/// </summary>
public class CategoryCreateUpdateDTO
{
    /// <summary>
    /// Назва категорії (обов'язкова, макс 100 символів)
    /// </summary>
    [Required(ErrorMessage = "Назва категорії є обов'язковою")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Назва повинна бути від 2 до 100 символів")]
    [RegularExpression(@"^[а-яА-ЯіїєґҐ\s-]+$", ErrorMessage = "Назва повинна містити лише кириличні символи")]
    public string Name { get; set; } = null!;
}
