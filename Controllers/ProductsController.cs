using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseEFApp.Constants;
using WarehouseEFApp.Context;
using WarehouseEFApp.DTOs;
using WarehouseEFApp.Models;

namespace WarehouseEFApp.Controllers;

/// <summary>
/// API контролер для управління продуктами
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly WarehouseDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор з dependency injection
    /// </summary>
    public ProductsController(WarehouseDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Отримати всі продукти з пагінацією
    /// </summary>
    /// <param name="page">Номер сторінки (за замовчуванням 1)</param>
    /// <param name="pageSize">Розмір сторінки (за замовчуванням 10, макс 100)</param>
    /// <returns>Пагінований список продуктів</returns>
    /// <response code="200">Успішно повернено список продуктів</response>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResultDTO<ProductReadDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResultDTO<ProductReadDTO>>> GetAll(
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
        var totalCount = await _context.Products.CountAsync();

        // Отримати дані поточної сторінки
        var products = await _context.Products
            .Include(p => p.Category)
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = _mapper.Map<IEnumerable<ProductReadDTO>>(products);
        var result = new PaginatedResultDTO<ProductReadDTO>(dtos, page, pageSize, totalCount);

        return Ok(result);
    }

    /// <summary>
    /// Отримати продукт за ID
    /// </summary>
    /// <param name="id">ID продукту</param>
    /// <returns>Продукт з вказаним ID</returns>
    /// <response code="200">Продукт знайдено</response>
    /// <response code="404">Продукт не знайдено</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductReadDTO>> GetById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound(new { message = $"Продукт з ID {id} не знайдено" });

        var dto = _mapper.Map<ProductReadDTO>(product);
        return Ok(dto);
    }

    /// <summary>
    /// <summary>
    /// Отримати продукти за категорією з пагінацією
    /// </summary>
    /// <param name="categoryId">ID категорії</param>
    /// <param name="page">Номер сторінки (за замовчуванням 1)</param>
    /// <param name="pageSize">Розмір сторінки (за замовчуванням 10, макс 100)</param>
    /// <returns>Пагінований список продуктів у категорії</returns>
    /// <response code="200">Успішно повернено список продуктів</response>
    /// <response code="404">Категорія не знайдена</response>
    [HttpGet("by-category/{categoryId}")]
    [ProducesResponseType(typeof(PaginatedResultDTO<ProductReadDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaginatedResultDTO<ProductReadDTO>>> GetByCategory(
        int categoryId,
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

        // Перевірити, чи категорія існує
        if (!await _context.Categories.AnyAsync(c => c.Id == categoryId))
            return NotFound(new { message = $"Категорія з ID {categoryId} не знайдена" });

        // Отримати загальну кількість записів
        var totalCount = await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .CountAsync();

        // Отримати дані поточної сторінки
        var products = await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = _mapper.Map<IEnumerable<ProductReadDTO>>(products);
        var result = new PaginatedResultDTO<ProductReadDTO>(dtos, page, pageSize, totalCount);

        return Ok(result);
    }

    /// <summary>
    /// Створити новий продукт
    /// </summary>
    /// <param name="dto">Дані продукту для створення</param>
    /// <returns>Створений продукт</returns>
    /// <response code="201">Продукт успішно створено</response>
    /// <response code="400">Некоректні дані</response>
    /// <response code="404">Категорія не знайдена</response>
    /// <response code="409">Продукт з такою назвою вже існує в цій категорії</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProductReadDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProductReadDTO>> Create([FromBody] ProductCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Перевірити існування категорії
        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category == null)
            return NotFound(new { message = $"Категорія з ID {dto.CategoryId} не знайдена" });

        // Перевірити унікальність назви в категорії
        if (await _context.Products.AnyAsync(p => p.Name == dto.Name && p.CategoryId == dto.CategoryId))
            return Conflict(new { message = "Продукт з такою назвою вже існує в цій категорії" });

        var product = _mapper.Map<Product>(dto);
        product.DateAdded = DateOnly.FromDateTime(DateTime.Now);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<ProductReadDTO>(product);
        resultDto.CategoryName = category.Name;
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, resultDto);
    }

    /// <summary>
    /// Оновити продукт
    /// </summary>
    /// <param name="id">ID продукту</param>
    /// <param name="dto">Оновлені дані продукту</param>
    /// <returns>Оновлений продукт</returns>
    /// <response code="200">Продукт успішно оновлено</response>
    /// <response code="400">Некоректні дані</response>
    /// <response code="404">Продукт або категорія не знайдено</response>
    /// <response code="409">Назва вже використовується іншим продуктом в цій категорії</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProductReadDTO>> Update(int id, [FromBody] ProductUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound(new { message = $"Продукт з ID {id} не знайдено" });

        // Перевірити категорію, якщо змінюється
        if (dto.CategoryId.HasValue && dto.CategoryId != product.CategoryId)
        {
            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
                return NotFound(new { message = $"Категорія з ID {dto.CategoryId} не знайдена" });
        }

        // Перевірити унікальність назви (якщо змінюється)
        var newName = dto.Name ?? product.Name;
        var newCategoryId = dto.CategoryId ?? product.CategoryId;

        if ((dto.Name != null || dto.CategoryId.HasValue) &&
            await _context.Products.AnyAsync(p =>
                p.Id != id &&
                p.Name == newName &&
                p.CategoryId == newCategoryId))
        {
            return Conflict(new { message = "Продукт з такою назвою вже існує в цій категорії" });
        }

        _mapper.Map(dto, product);
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<ProductReadDTO>(product);
        return Ok(resultDto);
    }

    /// <summary>
    /// Видалити продукт
    /// </summary>
    /// <param name="id">ID продукту</param>
    /// <returns>Статус видалення</returns>
    /// <response code="204">Продукт успішно видалено</response>
    /// <response code="404">Продукт не знайдено</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound(new { message = $"Продукт з ID {id} не знайдено" });

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
