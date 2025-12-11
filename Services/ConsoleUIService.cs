using WarehouseEFApp.Models;

namespace WarehouseEFApp.Services;


public class ConsoleUIService
{
    public void ShowMainMenu(string currentFramework = "EF")
    {
        string frameworkDisplay = currentFramework == "EF" ? "Entity Framework" : "ADO.NET";
        Console.WriteLine($"\n╔════════════════════════════════════════╗");
        Console.WriteLine($"║   PERSONNEL MANAGEMENT SYSTEM         ║");
        Console.WriteLine($"║   Framework: {frameworkDisplay,-20} ║");
        Console.WriteLine("╚════════════════════════════════════════╝\n");
        Console.WriteLine("  [1] Add new person");
        Console.WriteLine("  [2] View all people");
        Console.WriteLine("  [3] View person by ID");
        Console.WriteLine("  [4] Update person");
        Console.WriteLine("  [5] Delete person");
        Console.WriteLine("  [6] Toggle framework (current: " + frameworkDisplay + ")");
        Console.WriteLine("  [7] Seed users");
        Console.WriteLine("  [0] Exit\n");
        Console.Write("Choose an operation [0-7]: ");
    }

    public void ShowSuccessMessage(string message)
    {
        Console.WriteLine($"\n✅ {message}");
    }

    public void ShowErrorMessage(string message)
    {
        Console.WriteLine($"\n❌ {message}");
    }

    public void ShowTable(IEnumerable<Person> people)
    {
        if (!people.Any())
        {
            ShowErrorMessage("No people found.");
            return;
        }

        Console.WriteLine($"\n{"ID",-4} | {"Ім'я",-15} | {"Прізвище",-15} | {"Посада",-30}");
        Console.WriteLine(new string('─', 70));

        foreach (var person in people)
        {
            Console.WriteLine($"{person.Id,-4} | {person.FirstName,-15} | {person.LastName,-15} | {person.Position,-30}");
        }

        Console.WriteLine($"\nTotal people: {people.Count()}");
    }

    public void ShowPersonDetails(Person person)
    {
        Console.WriteLine($"\n✅ Person found:");
        Console.WriteLine($"   ID: {person.Id}");
        Console.WriteLine($"   First Name: {person.FirstName}");
        Console.WriteLine($"   Last Name: {person.LastName}");
        Console.WriteLine($"   Position: {person.Position}");
    }

    public void PressEnterToContinue()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}
