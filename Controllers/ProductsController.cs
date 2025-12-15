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
public class ProductsController : ControllerBase
{
    private readonly WarehouseDbContext _context;
    private readonly IMapper _mapper;

    /// Constructor з dependency injection
    public ProductsController(WarehouseDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResultDTO<ProductReadDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResultDTO<ProductReadDTO>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize)
    {
        // Validate params
        if (page < PaginationConstants.MinPageNumber)
            page = PaginationConstants.MinPageNumber;

        if (pageSize < PaginationConstants.MinPageSize)
            pageSize = PaginationConstants.MinPageSize;

        if (pageSize > PaginationConstants.MaxPageSize)
            pageSize = PaginationConstants.MaxPageSize;

        var totalCount = await _context.Products.CountAsync();

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

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductReadDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductReadDTO>> GetById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound(new { message = $"Product with ID {id} not found" });

        var dto = _mapper.Map<ProductReadDTO>(product);
        return Ok(dto);
    }

    [HttpGet("by-category/{categoryId}")]
    [ProducesResponseType(typeof(PaginatedResultDTO<ProductReadDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaginatedResultDTO<ProductReadDTO>>> GetByCategory(
        int categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = PaginationConstants.DefaultPageSize)
    {
        // Validate oarams
        if (page < PaginationConstants.MinPageNumber)
            page = PaginationConstants.MinPageNumber;

        if (pageSize < PaginationConstants.MinPageSize)
            pageSize = PaginationConstants.MinPageSize;

        if (pageSize > PaginationConstants.MaxPageSize)
            pageSize = PaginationConstants.MaxPageSize;

        if (!await _context.Categories.AnyAsync(c => c.Id == categoryId))
            return NotFound(new { message = $"Category with ID {categoryId} not found" });

        var totalCount = await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .CountAsync();

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

    [HttpPost]
    [ProducesResponseType(typeof(ProductReadDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ProductReadDTO>> Create([FromBody] ProductCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category == null)
            return NotFound(new { message = $"Category with ID {dto.CategoryId} not found" });

        if (await _context.Products.AnyAsync(p => p.Name == dto.Name && p.CategoryId == dto.CategoryId))
            return Conflict(new { message = "Product with this name already exists in this category" });

        var product = _mapper.Map<Product>(dto);
        product.DateAdded = DateOnly.FromDateTime(DateTime.Now);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<ProductReadDTO>(product);
        resultDto.CategoryName = category.Name;
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, resultDto);
    }

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
            return NotFound(new { message = $"Product with ID {id} not found" });

        // Check if cotegory exists
        if (dto.CategoryId.HasValue && dto.CategoryId != product.CategoryId)
        {
            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
                return NotFound(new { message = $"Category with ID {dto.CategoryId} not found" });
        }

        var newName = dto.Name ?? product.Name;
        var newCategoryId = dto.CategoryId ?? product.CategoryId;

        if ((dto.Name != null || dto.CategoryId.HasValue) &&
            await _context.Products.AnyAsync(p =>
                p.Id != id &&
                p.Name == newName &&
                p.CategoryId == newCategoryId))
        {
            return Conflict(new { message = "Product with this name already exists" });
        }

        _mapper.Map(dto, product);
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<ProductReadDTO>(product);
        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound(new { message = $"Product with ID {id} not found" });

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
