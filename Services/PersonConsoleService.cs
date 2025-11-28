using WarehouseEFApp.Models;

namespace WarehouseEFApp.Services;

/// <summary>
/// Service for handling Person operations via the console
/// </summary>
public class PersonConsoleService
{
    private readonly PersonService _personService;
    private readonly PersonAdoService _personAdoService;
    private readonly ConsoleUIService _uiService;

    public PersonConsoleService(PersonService personService, PersonAdoService personAdoService, ConsoleUIService uiService)
    {
        _personService = personService;
        _personAdoService = personAdoService;
        _uiService = uiService;
    }

    public async Task CreateAsync(string framework = "EF")
    {
        Console.WriteLine("\n--- ADD NEW PERSON ---");

        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Enter position: ");
        string position = Console.ReadLine() ?? "";

        if (framework == "ADO")
        {
            var person = new Person { FirstName = firstName, LastName = lastName, Position = position };
            _personAdoService.Add(person);
            _uiService.ShowSuccessMessage($"Person added successfully (ADO.NET)!\n   Name: {firstName} {lastName}\n   Position: {position}");
        }
        else
        {
            var person = await _personService.CreateAsync(firstName, lastName, position);
            _uiService.ShowSuccessMessage($"Person added successfully (EF)!\n   ID: {person.Id}\n   Name: {person.FirstName} {person.LastName}\n   Position: {person.Position}");
        }
    }

    public async Task ViewAllAsync(string framework = "EF")
    {
        Console.WriteLine("\n--- ALL PEOPLE ---");
        List<Person> people;

        if (framework == "ADO")
        {
            people = _personAdoService.GetAll();
        }
        else
        {
            people = (await _personService.GetAllAsync()).ToList();
        }

        _uiService.ShowTable(people);
    }

    public async Task ViewByIdAsync(string framework = "EF")
    {
        Console.WriteLine("\n--- FIND PERSON BY ID ---");

        Console.Write("Enter ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            _uiService.ShowErrorMessage("Invalid ID.");
            return;
        }

        Person? person = null;
        if (framework == "ADO")
        {
            person = _personAdoService.GetAll().FirstOrDefault(p => p.Id == id);
        }
        else
        {
            person = await _personService.GetByIdAsync(id);
        }

        if (person == null)
        {
            _uiService.ShowErrorMessage($"Person with ID {id} not found.");
            return;
        }

        _uiService.ShowPersonDetails(person);
    }

    public async Task UpdateAsync(string framework = "EF")
    {
        Console.WriteLine("\n--- UPDATE PERSON ---");

        Console.Write("Enter ID of the person to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            _uiService.ShowErrorMessage("Invalid ID.");
            return;
        }

        Person? existingPerson = null;
        if (framework == "ADO")
        {
            existingPerson = _personAdoService.GetAll().FirstOrDefault(p => p.Id == id);
        }
        else
        {
            existingPerson = await _personService.GetByIdAsync(id);
        }

        if (existingPerson == null)
        {
            _uiService.ShowErrorMessage($"Person with ID {id} not found.");
            return;
        }

        Console.WriteLine($"\nCurrent data:");
        Console.WriteLine($"   First Name: {existingPerson.FirstName}");
        Console.WriteLine($"   Last Name: {existingPerson.LastName}");
        Console.WriteLine($"   Position: {existingPerson.Position}");

        Console.Write("\nEnter new first name (press Enter to skip): ");
        string? newFirstName = Console.ReadLine();
        newFirstName = string.IsNullOrWhiteSpace(newFirstName) ? null : newFirstName;

        Console.Write("Enter new last name (press Enter to skip): ");
        string? newLastName = Console.ReadLine();
        newLastName = string.IsNullOrWhiteSpace(newLastName) ? null : newLastName;

        Console.Write("Enter new position (press Enter to skip): ");
        string? newPosition = Console.ReadLine();
        newPosition = string.IsNullOrWhiteSpace(newPosition) ? null : newPosition;

        if (framework == "ADO")
        {
            existingPerson.FirstName = newFirstName ?? existingPerson.FirstName;
            existingPerson.LastName = newLastName ?? existingPerson.LastName;
            existingPerson.Position = newPosition ?? existingPerson.Position;
            _personAdoService.Update(existingPerson);
            _uiService.ShowSuccessMessage($"Person updated successfully (ADO.NET)!\n   First Name: {existingPerson.FirstName}\n   Last Name: {existingPerson.LastName}\n   Position: {existingPerson.Position}");
        }
        else
        {
            var updated = await _personService.UpdateAsync(id, newFirstName, newLastName, newPosition);
            _uiService.ShowSuccessMessage($"Person updated successfully (EF)!\n   First Name: {updated.FirstName}\n   Last Name: {updated.LastName}\n   Position: {updated.Position}");
        }
    }

    public async Task DeleteAsync(string framework = "EF")
    {
        Console.WriteLine("\n--- DELETE PERSON ---");

        int totalCount;
        if (framework == "ADO")
        {
            totalCount = _personAdoService.GetAll().Count;
        }
        else
        {
            totalCount = await _personService.GetTotalCountAsync();
        }

        Console.Write("\nEnter ID of the person to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            _uiService.ShowErrorMessage("Invalid ID.");
            return;
        }

        Person? person = null;
        if (framework == "ADO")
        {
            person = _personAdoService.GetAll().FirstOrDefault(p => p.Id == id);
        }
        else
        {
            person = await _personService.GetByIdAsync(id);
        }

        if (person == null)
        {
            _uiService.ShowErrorMessage($"Person with ID {id} not found.");
            return;
        }

        Console.WriteLine($"\nPerson to delete:");
        Console.WriteLine($"   ID: {person.Id}");
        Console.WriteLine($"   Name: {person.FirstName} {person.LastName}");
        Console.WriteLine($"   Position: {person.Position}");

        Console.Write("\nAre you sure? (y/n): ");
        string confirm = Console.ReadLine()?.ToLower() ?? "";

        if (confirm == "y")
        {
            if (framework == "ADO")
            {
                _personAdoService.Delete(id);
                _uiService.ShowSuccessMessage("Person deleted successfully (ADO.NET)!");
            }
            else
            {
                await _personService.DeleteAsync(id);
                _uiService.ShowSuccessMessage("Person deleted successfully (EF)!");
            }
        }
        else
        {
            _uiService.ShowErrorMessage("Deletion cancelled.");
        }
    }
}
