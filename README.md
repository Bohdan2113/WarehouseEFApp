# Warehouse API - ASP.NET Core Web API

Повнофункціональний REST API для управління складом з CRUD операціями для категорій та продуктів.

## Вимоги

- **.NET 9.0 SDK** або вище
- **PostgreSQL** 12+ (або модифікуйте `appsettings.json` для іншої БД)
- **Postman** (опціонально, для тестування API)

## Швидкий старт

### 1. Налаштування бази даних

Переконайтесь, що в `appsettings.json` правильно налаштовано з'єднання:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=warehouse;Username=postgres;Password=your_password"
}
```

### 2. Запуск API

```bash
# Перейти до папки проекту
cd WarehouseEFApp

# Запустити
dotnet run

# або запустити в режимі development
dotnet run --configuration Debug
```

API буде доступна за адресою:

- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger UI**: http://localhost:5000/swagger

### 3. Тестування з Postman

1. Відкрийте Postman
2. Натисніть **File → Import**
3. Виберіть файл `WarehouseAPI.postman_collection.json`
4. В колекції встановіть змінну `base_url` = `http://localhost:5000`
5. Запускайте запити

## API Endpoints

### Categories

- `GET /api/categories` - отримати всі категорії
- `GET /api/categories/{id}` - отримати категорію за ID
- `POST /api/categories` - створити категорію
- `PUT /api/categories/{id}` - оновити категорію
- `DELETE /api/categories/{id}` - видалити категорію

### Products

- `GET /api/products` - отримати всі продукти
- `GET /api/products/{id}` - отримати продукт за ID
- `GET /api/products/by-category/{categoryId}` - продукти у категорії
- `POST /api/products` - створити продукт
- `PUT /api/products/{id}` - оновити продукт
- `DELETE /api/products/{id}` - видалити продукт

## Структура проекту

```
WarehouseEFApp/
├── Controllers/              # REST контролери
│   ├── CategoriesController.cs
│   └── ProductsController.cs
├── DTOs/                     # Data Transfer Objects з валідацією
│   ├── CategoryDTO.cs
│   └── ProductDTO.cs
├── Mappings/                 # AutoMapper профіль
│   └── MappingProfile.cs
├── Models/                   # Entity Framework моделі
├── Context/                  # DbContext
├── Properties/
│   └── launchSettings.json   # Налаштування запуску
├── Program.cs                # Конфігурація ASP.NET Core
├── appsettings.json          # Налаштування застосування
├── API_DOCUMENTATION.md      # Повна документація API
├── WarehouseAPI.postman_collection.json  # Postman колекція
└── WarehouseEFApp.csproj     # Project file
```

## Ключові особливості

✅ **CRUD операції** для Categories та Products  
✅ **Swagger/OpenAPI документація** з інтерактивним UI  
✅ **AutoMapper** для маппінгу Entity ↔ DTO  
✅ **Dependency Injection** для всіх сервісів  
✅ **Валідація моделей** з Data Annotations  
✅ **Правильні HTTP статус коди** (200, 201, 204, 400, 404, 409)  
✅ **Обробка помилок** з информативними повідомленнями  
✅ **Асинхронне програмування** з async/await  
✅ **CORS підтримка** для клієнтських застосунків  
✅ **Postman колекція** для тестування

## Валідація даних

### Категорія

- **Name**: обов'язково, 2-100 символів, тільки кирилиця

### Продукт

- **Name**: обов'язково, 2-150 символів
- **CategoryId**: обов'язково, мінімум 1
- **Unit**: обов'язково, 1-20 символів

## Команди для разробки

```bash
# Побудування проекту
dotnet build

# Запуск тестів (якщо є)
dotnet test

# Очистка build артефактів
dotnet clean

# Публікація для production
dotnet publish -c Release
```

## Приклади запитів

### Створити категорію

```bash
curl -X POST http://localhost:5000/api/categories \
  -H "Content-Type: application/json" \
  -d '{"name": "Електроніка"}'
```

### Отримати всі продукти

```bash
curl http://localhost:5000/api/products
```

### Створити продукт

```bash
curl -X POST http://localhost:5000/api/products \
  -H "Content-Type: application/json" \
  -d '{"name": "Ноутбук", "categoryId": 1, "unit": "шт"}'
```

## Troubleshooting

### Помилка з'єднання до БД

- Перевірте, чи запущена PostgreSQL
- Перевірте рядок з'єднання в `appsettings.json`
- Перевірте права доступу БД

### Swagger UI не завантажується

- Переконайтесь, що запущено в режимі Development
- Перевірте порт 5000 (може бути занято)

### CORS помилки при запитах з браузера

- CORS вже налаштовано в Program.cs (AllowAll)
- Для production змініть на конкретні домени

## Документація

Детальна документація API доступна в файлі `API_DOCUMENTATION.md`

## Архітектура

Проект використовує **Layered Architecture**:

1. **Controllers** - обробка HTTP запитів
2. **DTOs** - трансформація та валідація даних
3. **Services** (опціонально) - бізнес логіка
4. **Repository** (опціонально) - доступ до БД
5. **Models** - ORM сутності

## Технологічний стек

- **ASP.NET Core 9.0** - веб-фреймворк
- **Entity Framework Core 9.0** - ORM
- **PostgreSQL** - база даних (з Npgsql)
- **Swagger 6.4.0** - API документація
- **AutoMapper 12.0.1** - DTO маппінг

## License

MIT

## Контакти

**Розроблювач**: Lab 3 KPZ  
**Дата**: 11 December 2024  
**Статус**: ✅ Production Ready

---

**Для запуску**: `dotnet run`  
**Swagger**: http://localhost:5000/swagger  
**Postman Collection**: Імпортуйте `WarehouseAPI.postman_collection.json`
