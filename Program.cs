using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehouseEFApp.Context;
using WarehouseEFApp.Services;

// Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// DI
var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddDbContext<WarehouseDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

services.AddScoped<PersonService>();
services.AddScoped<PersonAdoService>();
services.AddScoped<ConsoleUIService>();
services.AddScoped<PersonConsoleService>();

var serviceProvider = services.BuildServiceProvider();

var uiService = serviceProvider.GetRequiredService<ConsoleUIService>();
var personConsoleService = serviceProvider.GetRequiredService<PersonConsoleService>();


string currentFramework = "EF"; 
bool running = true;

while (running)
{
    uiService.ShowMainMenu(currentFramework);
    string choice = Console.ReadLine() ?? "";

    try
    {
        switch (choice)
        {
            case "1":
                await personConsoleService.CreateAsync(currentFramework);
                break;
            case "2":
                await personConsoleService.ViewAllAsync(currentFramework);
                break;
            case "3":
                await personConsoleService.ViewByIdAsync(currentFramework);
                break;
            case "4":
                await personConsoleService.UpdateAsync(currentFramework);
                break;
            case "5":
                await personConsoleService.DeleteAsync(currentFramework);
                break;
            case "6":
                currentFramework = currentFramework == "EF" ? "ADO" : "EF";
                string newFrameworkName = currentFramework == "EF" ? "Entity Framework" : "ADO.NET";
                uiService.ShowSuccessMessage($"Switched to {newFrameworkName}");
                uiService.PressEnterToContinue();
                break;
            case "0":
                running = false;
                Console.WriteLine("\nGoodbye!");
                break;
            default:
                uiService.ShowErrorMessage("Invalid choice. Try again.");
                uiService.PressEnterToContinue();
                break;
        }
    }
    catch (Exception ex)
    {
        uiService.ShowErrorMessage(ex.Message);
        uiService.PressEnterToContinue();
    }
}