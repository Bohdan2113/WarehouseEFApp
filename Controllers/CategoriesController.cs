using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseEFApp.Constants;
using WarehouseEFApp.Context;
using WarehouseEFApp.DTOs;
using WarehouseEFApp.Models;

namespace WarehouseEFApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly WarehouseDbContext _context;
    private readonly IMapper _mapper;

    // Constructor з dependency injection
    public CategoriesController(WarehouseDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResultDTO<CategoryReadDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResultDTO<CategoryReadDTO>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize)
    {
        // Validate Parameters
        if (page < PaginationConstants.MinPageNumber)
            page = PaginationConstants.MinPageNumber;

        if (pageSize < PaginationConstants.MinPageSize)
            pageSize = PaginationConstants.MinPageSize;

        if (pageSize > PaginationConstants.MaxPageSize)
            pageSize = PaginationConstants.MaxPageSize;

        var totalCount = await _context.Categories.CountAsync();

        var categories = await _context.Categories
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = _mapper.Map<IEnumerable<CategoryReadDTO>>(categories);
        var result = new PaginatedResultDTO<CategoryReadDTO>(dtos, page, pageSize, totalCount);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryReadDTO>> GetById(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound(new { message = $"Category with ID {id} not found" });

        var dto = _mapper.Map<CategoryReadDTO>(category);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryReadDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoryReadDTO>> Create([FromBody] CategoryCreateUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check name uniqueness
        if (await _context.Categories.AnyAsync(c => c.Name == dto.Name))
            return Conflict(new { message = "Category with this name already exists" });

        var category = _mapper.Map<Category>(dto);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<CategoryReadDTO>(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, resultDto);
    }

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
            return NotFound(new { message = $"Catgory with ID {id} not found" });

        // Check name uniqueness
        if (category.Name != dto.Name && await _context.Categories.AnyAsync(c => c.Name == dto.Name))
            return Conflict(new { message = "Category with this name already exists" });

        _mapper.Map(dto, category);
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<CategoryReadDTO>(category);
        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound(new { message = $"Category with ID {id} not found" });

        // Check if category has products
        if (await _context.Products.AnyAsync(p => p.CategoryId == id))
            return BadRequest(new { message = "You can not delete category thet has products" });

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
