# ✅ ВІДПОВІДІ НА ВАШІ ПИТАННЯ

Ви задали 3 основні питання:

---

## 1️⃣ Як працює Mapping (AutoMapper)?

### Що це?

**Mapping** - це автоматичне перетворення об'єкта одного типу в інший.

### Для чого?

Замість ручного копіювання полів:

```csharp
// ❌ БЕЗ mapping (погано - скучно, помилки):
var dto = new ProductDTO();
dto.Id = product.Id;
dto.Name = product.Name;
dto.CategoryId = product.CategoryId;

// ✅ З mapping (добре - автоматично):
var dto = _mapper.Map<ProductDTO>(product);
```

### Як підключати?

#### Крок 1: Встановити пакет

```bash
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

#### Крок 2: Зареєструвати в Program.cs

```csharp
builder.Services.AddAutoMapper(typeof(MappingProfile));
// ↑ Один раз на старті
```

#### Крок 3: Використовувати в контролері

```csharp
public class ProductsController
{
    private readonly IMapper _mapper;  // ← Dependency Injection

    public ProductsController(IMapper mapper)
    {
        _mapper = mapper;  // ← ASP.NET підставляє
    }

    public async Task<ActionResult> GetAll()
    {
        var products = await _context.Products.ToListAsync();
        var dtos = _mapper.Map<IEnumerable<ProductDTO>>(products);  // ← Маппінг!
        return Ok(dtos);
    }
}
```

#### Крок 4: Конфігурувати маппінги

```csharp
// Mappings/MappingProfile.cs
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Entity → DTO
        CreateMap<Product, ProductReadDTO>()
            // Спеціальна логіка для CategoryName з related entity
            .ForMember(dest => dest.CategoryName,
                       opt => opt.MapFrom(src => src.Category.Name));

        // DTO → Entity
        CreateMap<ProductCreateDTO, Product>();
    }
}
```

### Де в нашому проекті?

- **Конфіг:** [Mappings/MappingProfile.cs](Mappings/MappingProfile.cs)
- **Реєстрація:** [Program.cs](Program.cs) рядок 18
- **Використання:** [Controllers/ProductsController.cs](Controllers/ProductsController.cs) рядки 50, 150

### Детальне пояснення:

👉 **[ARCHITECTURE_GUIDE.md - Розділ 1](ARCHITECTURE_GUIDE.md#1️⃣-automapper---що-це-і-як-це-працює)**

---

## 2️⃣ Де і коли були встановлені залежності?

### Де встановлені?

У файлі **[WarehouseEFApp.csproj](WarehouseEFApp.csproj)**:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
  <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="9.0.0" />
  <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
  <PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
</ItemGroup>
```

### Коли встановлені?

**Автоматично** при першому запуску:

```bash
dotnet run
# ↓ Автоматично запускається:
# dotnet restore  ← Завантажує пакети з nuget.org
```

### Де зберігаються на комп'ютері?

```
C:\Users\<username>\.nuget\packages\
    ├── microsoft.entityframeworkcore.design\9.0.0\
    ├── npgsql.entityframeworkcore.postgresql\9.0.0\
    ├── swashbuckle.aspnetcore\6.4.0\
    └── automapper\12.0.1\
```

### Як встановити вручну?

```bash
dotnet add package Swashbuckle.AspNetCore
# ↑ Додасть у .csproj і завантажить
```

### Що ці пакети роблять?

| Пакет                          | Для чого                   |
| ------------------------------ | -------------------------- |
| **EntityFrameworkCore.Design** | Міграції, scaffolding з БД |
| **Npgsql**                     | Драйвер PostgreSQL для EF  |
| **Swashbuckle.AspNetCore**     | Swagger документація       |
| **AutoMapper**                 | Mapping Entity ↔ DTO       |

---

## 3️⃣ Як підключається Swagger? (кожна стрічка)

### Де конфігурується?

В файлі **[Program.cs](Program.cs)** рядки 22-50

### Кожна стрічка пояснюється:

```csharp
// ========== ЧАСТИНА 1: РЕЄСТРАЦІЯ (що додати) ==========

builder.Services.AddSwaggerGen(c =>
{
    // 1. Створити документацію для версії v1
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Warehouse API",           // ← Назва у браузері
        Version = "v1",                    // ← Версія API
        Description = "REST API для управління складом",
        Contact = new OpenApiContact
        {
            Name = "Warehouse System",
            Email = "support@warehouse.local"
        }
    });
    // ↑ Це метадані що буде в Swagger UI

    // 2. Додати XML коментарі з кода
    var xmlFile = "WarehouseEFApp.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
    // ↑ Коментарії /// <summary> появляться в Swagger

    // 3. Додати підтримку JWT токенів (для майбутнього)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    // ↑ Дозволяємо користувачам у Swagger вводити токен
});

// ========== ЧАСТИНА 2: MIDDLEWARE (як показати) ==========

if (app.Environment.IsDevelopment())
{
    // 1. Увімкнути JSON endpoint (/swagger/v1/swagger.json)
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "swagger/{documentName}/swagger.json";
        // ↑ URL буде: /swagger/v1/swagger.json
    });

    // 2. Увімкнути UI (/swagger)
    app.UseSwaggerUI(c =>
    {
        // 2а. Де взяти JSON
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Warehouse API v1");

        // 2б. На якому URL показувати
        c.RoutePrefix = "swagger";
        // ↑ UI буде на http://localhost:5000/swagger

        // 2в. Назва вкладки браузера
        c.DocumentTitle = "Warehouse API - Swagger";
    });
}
// ↑ Тільки у Development режимі, не у Production
```

### Як це працює?

```
1. При старті (Program.cs)
   ↓
2. AddSwaggerGen() - реєструємо генератор Swagger
   ↓
3. builder.Build() - створюємо app
   ↓
4. При HTTP запиті на /swagger
   ↓
5. SwaggerUIMiddleware перехоплює
   ↓
6. Показує HTML сторінку з UI
   ↓
7. UI завантажує JSON з /swagger/v1/swagger.json
   ↓
8. UI показує красиву документацію
```

### Як дивитись Swagger?

```bash
# 1. Запустити API
dotnet run

# 2. Відкрити у браузері
http://localhost:5000/swagger
```

### Що там можна робити?

- ✅ Дивитися документацію всіх endpoints
- ✅ Дивитися параметри та типи
- ✅ Тестувати запити прямо у браузері
- ✅ Дивитися примери відповідей

### Де в коді потрібна [ProducesResponseType]?

```csharp
// Controllers/ProductsController.cs

[HttpGet]
[ProducesResponseType(typeof(PaginatedResultDTO<ProductReadDTO>), StatusCodes.Status200OK)]
// ↑ Swagger знає, що повертаємо 200 із цим типом
[ProducesResponseType(StatusCodes.Status404NotFound)]
// ↑ Також можемо повернути 404
public async Task<ActionResult<...>> GetAll()
{
    // ...
}
```

### Детальне пояснення:

👉 **[ARCHITECTURE_GUIDE.md - Розділ Swagger](ARCHITECTURE_GUIDE.md#-частина-2-реєстрація-сервісів-di)**

---

## 4️⃣ Dependency Injection (БОНУС)

### Де підключаються?

В **Program.cs** рядки 14-22 (builder.Services.Add\*):

```csharp
// Database
builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseNpgsql(connectionString));
// ↑ DbContext буде Scoped (один на запит)

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
// ↑ IMapper буде Scoped

// Controllers
builder.Services.AddControllers();
// ↑ Реєструємо контролери
```

### Де створюються?

**Не вручну!** ASP.NET створює автоматично при запиті:

```csharp
public class ProductsController
{
    public ProductsController(
        WarehouseDbContext context,  // ← ASP.NET шукає у DI контейнері
        IMapper mapper)              // ← ASP.NET шукає у DI контейнері
    {
        // ASP.NET сам створив нові інстанси! ✅
        // Контроллеру не потрібно їх створювати
    }
}
```

### Картина DI контейнера:

```
Program.cs старт:
    ↓
builder.Services.AddDbContext<WarehouseDbContext>()
    ↓
DI контейнер: {
    Type: WarehouseDbContext
    Lifecycle: Scoped  ← Один на запит
    Factory: options.UseNpgsql(...)
}

...

builder.Services.AddAutoMapper(typeof(MappingProfile))
    ↓
DI контейнер: {
    Type: IMapper
    Lifecycle: Scoped
    Factory: new Mapper(MappingProfile)
}

=============================

При HTTP запиті:

1. ASP.NET аналізує ProductsController(WarehouseDbContext, IMapper)
   ↓
2. DI контейнер шукає WarehouseDbContext
   ↓
3. Знайшов! Створює новий інстанс (Scoped)
   ↓
4. DI контейнер шукає IMapper
   ↓
5. Знайшов! Повертає з контейнера (Scoped)
   ↓
6. ProductsController отримав залежності ✅
```

### Детальне пояснення:

👉 **[ARCHITECTURE_GUIDE.md - Розділ DI](ARCHITECTURE_GUIDE.md#🔄-dependency-injection-di---як-це-працює)**

---

## 📚 ГДЕ ПРОЧИТАТИ БІЛЬШЕ?

| Питання                        | Файл                                                                                                               |
| ------------------------------ | ------------------------------------------------------------------------------------------------------------------ |
| **Як працює AutoMapper**       | [ARCHITECTURE_GUIDE.md - Розділ 1](ARCHITECTURE_GUIDE.md#1️⃣-automapper---що-це-і-як-це-працює)                     |
| **Залежності (пакети)**        | [ARCHITECTURE_GUIDE.md - Розділ 2](ARCHITECTURE_GUIDE.md#-залежності-nuget-пакети---де-вони-встановлені)           |
| **Program.cs (кожна стрічка)** | [ARCHITECTURE_GUIDE.md - Розділ 3](ARCHITECTURE_GUIDE.md#-частина-3-programcs---детальне-пояснення-кожної-стрічки) |
| **DI детально**                | [ARCHITECTURE_GUIDE.md - DI розділ](ARCHITECTURE_GUIDE.md#🔄-dependency-injection-di---як-це-працює)               |
| **Swagger детально**           | [ARCHITECTURE_GUIDE.md - Swagger](ARCHITECTURE_GUIDE.md#-частина-2-реєстрація-сервісів-di)                         |
| **ASCII схеми**                | [VISUAL_DIAGRAMS.md](VISUAL_DIAGRAMS.md)                                                                           |

---

## 🚀 ПРАКТИКА

### Задача 1: Розуміння Mapping

1. Відкрийте [Mappings/MappingProfile.cs](Mappings/MappingProfile.cs)
2. Поясніть кожен CreateMap
3. Запустіть Postman запит POST /api/products
4. Спостерігайте як DTO перетворюється на Entity

### Задача 2: Розуміння DI

1. Запустіть API
2. Додайте breakpoint у конструктор ProductsController
3. Дивіться як параметри заповнюються
4. Вдячіть ASP.NET! 😊

### Задача 3: Розуміння Swagger

1. Запустіть API
2. Відкрийте http://localhost:5000/swagger
3. Натисніть на GET /api/products
4. Клікніть "Try it out" та запустіть запит
5. Дивіться результат

---

**На це все питання! 🎉**

Якщо залишилось щось незрозумілого - читайте файли документації - там все детально!
