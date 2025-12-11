using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseEFApp.Constants;
using WarehouseEFApp.Context;
using WarehouseEFApp.DTOs;
using WarehouseEFApp.Models;

namespace WarehouseEFApp.Controllers;

/// <summary>
/// API контролер для управління категоріями продуктів
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly WarehouseDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор з dependency injection
    /// </summary>
    public CategoriesController(WarehouseDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Отримати всі категорії з пагінацією
    /// </summary>
    /// <param name="page">Номер сторінки (за замовчуванням 1)</param>
    /// <param name="pageSize">Розмір сторінки (за замовчуванням 10, макс 100)</param>
    /// <returns>Пагінований список категорій</returns>
    /// <response code="200">Успішно повернено список категорій</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResultDTO<CategoryReadDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResultDTO<CategoryReadDTO>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize)
    {
        // Валідація параметрів
        if (page < PaginationConstants.MinPageNumber)
            page = PaginationConstants.MinPageNumber;

        if (pageSize < PaginationConstants.MinPageSize)
            pageSize = PaginationConstants.MinPageSize;

        if (pageSize > PaginationConstants.MaxPageSize)
            pageSize = PaginationConstants.MaxPageSize;

        // Отримати загальну кількість записів
        var totalCount = await _context.Categories.CountAsync();

        // Отримати дані поточної сторінки
        var categories = await _context.Categories
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = _mapper.Map<IEnumerable<CategoryReadDTO>>(categories);
        var result = new PaginatedResultDTO<CategoryReadDTO>(dtos, page, pageSize, totalCount);

        return Ok(result);
    }

    /// <summary>
    /// Отримати категорію за ID
    /// </summary>
    /// <param name="id">ID категорії</param>
    /// <returns>Категорія з вказаним ID</returns>
    /// <response code="200">Категорія знайдена</response>
    /// <response code="404">Категорія не знайдена</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryReadDTO>> GetById(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound(new { message = $"Категорія з ID {id} не знайдена" });

        var dto = _mapper.Map<CategoryReadDTO>(category);
        return Ok(dto);
    }

    /// <summary>
    /// Створити нову категорію
    /// </summary>
    /// <param name="dto">Дані категорії для створення</param>
    /// <returns>Створена категорія</returns>
    /// <response code="201">Категорія успішно створена</response>
    /// <response code="400">Некоректні дані</response>
    /// <response code="409">Категорія з такою назвою вже існує</response>
    [HttpPost]
    [ProducesResponseType(typeof(CategoryReadDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoryReadDTO>> Create([FromBody] CategoryCreateUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Перевірити унікальність назви
        if (await _context.Categories.AnyAsync(c => c.Name == dto.Name))
            return Conflict(new { message = "Категорія з такою назвою вже існує" });

        var category = _mapper.Map<Category>(dto);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<CategoryReadDTO>(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, resultDto);
    }

    /// <summary>
    /// Оновити категорію
    /// </summary>
    /// <param name="id">ID категорії</param>
    /// <param name="dto">Оновлені дані категорії</param>
    /// <returns>Оновлена категорія</returns>
    /// <response code="200">Категорія успішно оновлена</response>
    /// <response code="400">Некоректні дані</response>
    /// <response code="404">Категорія не знайдена</response>
    /// <response code="409">Назва вже використовується іншою категорією</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CategoryReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoryReadDTO>> Update(int id, [FromBody] CategoryCreateUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound(new { message = $"Категорія з ID {id} не знайдена" });

        // Перевірити унікальність назви (якщо змінюється)
        if (category.Name != dto.Name && await _context.Categories.AnyAsync(c => c.Name == dto.Name))
            return Conflict(new { message = "Категорія з такою назвою вже існує" });

        _mapper.Map(dto, category);
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<CategoryReadDTO>(category);
        return Ok(resultDto);
    }

    /// <summary>
    /// Видалити категорію
    /// </summary>
    /// <param name="id">ID категорії</param>
    /// <returns>Статус видалення</returns>
    /// <response code="204">Категорія успішно видалена</response>
    /// <response code="404">Категорія не знайдена</response>
    /// <response code="400">Не можна видалити категорію з продуктами</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound(new { message = $"Категорія з ID {id} не знайдена" });

        // Перевірити, чи є продукти в цій категорії
        if (await _context.Products.AnyAsync(p => p.CategoryId == id))
            return BadRequest(new { message = "Не можна видалити категорію, яка містить продукти" });

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
