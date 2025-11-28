using WarehouseEFApp.Context;
using WarehouseEFApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WarehouseEFApp.Services;

/// <summary>
/// Service for CRUD operations on Person (DB First approach)
/// </summary>
public class PersonService
{
    private readonly WarehouseDbContext _context;

    public PersonService(WarehouseDbContext context)
    {
        _context = context;
    }

    // ============ CREATE ============
    public async Task<Person> CreateAsync(string firstName, string lastName, string position)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("First name and last name cannot be empty");

        var person = new Person
        {
            FirstName = firstName,
            LastName = lastName,
            Position = position
        };

        _context.People.Add(person);
        await _context.SaveChangesAsync();
        return person;
    }

    // ============ READ ============
    public async Task<Person?> GetByIdAsync(int id)
    {
        return await _context.People.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _context.People.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToListAsync();
    }

    public async Task<IEnumerable<Person>> SearchAsync(string searchTerm)
    {
        return await _context.People
            .Where(p => EF.Functions.ILike(p.FirstName, $"%{searchTerm}%") ||
                        EF.Functions.ILike(p.LastName, $"%{searchTerm}%") ||
                        (p.Position != null && EF.Functions.ILike(p.Position, $"%{searchTerm}%")))
            .OrderBy(p => p.LastName)
            .ToListAsync();
    }

    // ============ UPDATE ============
    public async Task<Person> UpdateAsync(int id, string? firstName = null, string? lastName = null, string? position = null)
    {
        var person = await _context.People.FirstOrDefaultAsync(p => p.Id == id);
        if (person == null)
            throw new KeyNotFoundException($"Person with ID {id} not found");

        if (!string.IsNullOrWhiteSpace(firstName))
            person.FirstName = firstName;

        if (!string.IsNullOrWhiteSpace(lastName))
            person.LastName = lastName;

        if (!string.IsNullOrWhiteSpace(position))
            person.Position = position;

        _context.People.Update(person);
        await _context.SaveChangesAsync();
        return person;
    }

    // ============ DELETE ============
    public async Task DeleteAsync(int id)
    {
        var person = await _context.People.FirstOrDefaultAsync(p => p.Id == id);
        if (person == null)
            throw new KeyNotFoundException($"Person with ID {id} not found");

        _context.People.Remove(person);
        await _context.SaveChangesAsync();
    }

    // ============ ADDITIONAL ============
    public async Task<int> GetTotalCountAsync()
    {
        return await _context.People.CountAsync();
    }
}
