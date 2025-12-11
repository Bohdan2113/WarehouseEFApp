namespace WarehouseEFApp.DTOs;

/// <summary>
/// DTO для пагінованого результату
/// </summary>
/// <typeparam name="T">Тип елементів у результаті</typeparam>
public class PaginatedResultDTO<T>
{
    /// <summary>
    /// Список даних поточної сторінки
    /// </summary>
    public IEnumerable<T> Data { get; set; } = new List<T>();

    /// <summary>
    /// Поточний номер сторінки (1-based)
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Розмір сторінки (кількість елементів на сторінці)
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Загальна кількість елементів
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Загальна кількість сторінок
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Чи є наступна сторінка
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;

    /// <summary>
    /// Чи є попередня сторінка
    /// </summary>
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// Конструктор з параметрами
    /// </summary>
    public PaginatedResultDTO(IEnumerable<T> data, int currentPage, int pageSize, int totalCount)
    {
        Data = data;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
