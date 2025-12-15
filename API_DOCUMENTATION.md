## Running the API

```bash
# Побудування
dotnet build

# Запуск
dotnet run

# API буде доступна на:
# HTTP: http://localhost:5000
# HTTPS: https://localhost:5001
# Swagger: http://localhost:5000/swagger
```

---

## Dependencies

Встановлені NuGet пакети:

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
```

---

## Project Structure

```
WarehouseEFApp/
├── Controllers/                    # REST контролери
│   ├── CategoriesController.cs     # CRUD для категорій
│   └── ProductsController.cs       # CRUD для продуктів
├── DTOs/                           # Data Transfer Objects
│   ├── CategoryDTO.cs              # CategoryReadDTO, CategoryCreateUpdateDTO
│   ├── ProductDTO.cs               # ProductReadDTO, ProductCreateDTO, ProductUpdateDTO
│   └── PaginatedResultDTO.cs       # Пагінований результат
├── Mappings/                       # AutoMapper конфігурація
│   └── MappingProfile.cs           # Маппінги Entity ↔ DTO
├── Constants/                      # Константи
│   └── PaginationConstants.cs      # Константи для пагінації
├── Models/                         # Entity Framework моделі (DB First)
│   ├── Category.cs
│   ├── Product.cs
│   └── ... (інші таблиці)
├── Context/                        # DbContext
│   └── WarehouseDbContext.cs       # Конфігурація EF Core
├── Properties/
│   └── launchSettings.json         # Налаштування запуску (порти)
├── Services/                       # Бізнес логіка (з попередніх завдань)
│   ├── PersonService.cs            # EF CRUD
│   ├── PersonAdoService.cs         # ADO.NET CRUD
│   └── ...
├── Migrations/                     # EF Core міграції (якщо були)
├── Program.cs                      # Конфігурація ASP.NET Core
├── appsettings.json                # Рядок з'єднання до БД
├── README.md                       # Інструкція по запуску
├── ARCHITECTURE_GUIDE.md           # 📚 ДЕТАЛЬНЕ ПОЯСНЕННЯ АРХІТЕКТУРИ
├── VISUAL_DIAGRAMS.md              # 📊 ASCII діаграми потоків
├── WarehouseAPI.postman_collection.json  # Postman тести
├── test-api.bat                    # Batch скрипт для тестування
├── test-api.ps1                    # PowerShell скрипт для тестування
└── WarehouseEFApp.csproj           # Project file з залежностями
```

### 📚 Важливі файли для навчання

- **[ARCHITECTURE_GUIDE.md](ARCHITECTURE_GUIDE.md)** - Як працює AutoMapper, DI, Swagger (ОБОВ'ЯЗКОВО прочитати!)
- **[VISUAL_DIAGRAMS.md](VISUAL_DIAGRAMS.md)** - Схеми потоків даних
