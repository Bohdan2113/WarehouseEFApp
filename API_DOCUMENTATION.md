# Warehouse API Documentation

## Overview

Warehouse REST API - система для управління складом з повноцінним CRUD функціоналом для категорій та продуктів.

**Версія**: 1.0  
**Base URL**: `http://localhost:5000`  
**Документація Swagger**: `http://localhost:5000/swagger`

---

## Архітектура

### Шари застосування:

1. **Controllers** - HTTP endpoints для обробки запитів
2. **DTOs (Data Transfer Objects)** - об'єкти передачі даних з валідацією
3. **Models** - Entity Framework моделі для бази даних
4. **Context** - WarehouseDbContext для управління DB
5. **Mappings** - AutoMapper профіль для трансформації Entity ↔ DTO

### Технологічний стек:

- **Framework**: ASP.NET Core 9.0
- **ORM**: Entity Framework Core 9.0
- **Database**: PostgreSQL
- **API Documentation**: Swagger/OpenAPI 6.4.0
- **Mapping**: AutoMapper 12.0.1
- **Validation**: Data Annotations

---

## API Endpoints

### Categories Controller

#### 1. Get All Categories

```http
GET /api/categories
```

**Response**: 200 OK

```json
[
  {
    "id": 1,
    "name": "Електроніка"
  },
  {
    "id": 2,
    "name": "Одяг"
  }
]
```

---

#### 2. Get Category by ID

```http
GET /api/categories/{id}
```

**Parameters**:

- `id` (int, required) - ID категорії

**Response**: 200 OK

```json
{
  "id": 1,
  "name": "Електроніка"
}
```

**Response**: 404 Not Found

```json
{
  "message": "Категорія з ID 999 не знайдено"
}
```

---

#### 3. Create Category

```http
POST /api/categories
Content-Type: application/json
```

**Request Body**:

```json
{
  "name": "Нова Категорія"
}
```

**Validation Rules**:

- `name` - обов'язково, від 2 до 100 символів, кирилиця

**Response**: 201 Created

```json
{
  "id": 3,
  "name": "Нова Категорія"
}
```

**Response**: 400 Bad Request (неправильні дані)

```json
{
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": ["The field Name must be between 2 and 100 characters long."]
  }
}
```

**Response**: 409 Conflict (категорія вже існує)

```json
{
  "message": "Категорія з такою назвою вже існує"
}
```

---

#### 4. Update Category

```http
PUT /api/categories/{id}
Content-Type: application/json
```

**Parameters**:

- `id` (int, required) - ID категорії для оновлення

**Request Body**:

```json
{
  "name": "Оновлена Категорія"
}
```

**Response**: 200 OK

```json
{
  "id": 1,
  "name": "Оновлена Категорія"
}
```

**Response**: 404 Not Found

```json
{
  "message": "Категорія з ID 999 не знайдено"
}
```

---

#### 5. Delete Category

```http
DELETE /api/categories/{id}
```

**Parameters**:

- `id` (int, required) - ID категорії для видалення

**Response**: 204 No Content

**Response**: 404 Not Found

```json
{
  "message": "Категорія з ID 999 не знайдено"
}
```

---

### Products Controller

#### 1. Get All Products

```http
GET /api/products
```

**Response**: 200 OK

```json
[
  {
    "id": 1,
    "name": "Ноутбук",
    "categoryId": 1,
    "categoryName": "Електроніка",
    "unit": "шт",
    "dateAdded": "2024-12-11"
  }
]
```

---

#### 2. Get Product by ID

```http
GET /api/products/{id}
```

**Response**: 200 OK

```json
{
  "id": 1,
  "name": "Ноутбук",
  "categoryId": 1,
  "categoryName": "Електроніка",
  "unit": "шт",
  "dateAdded": "2024-12-11"
}
```

**Response**: 404 Not Found

```json
{
  "message": "Продукт з ID 999 не знайдено"
}
```

---

#### 3. Get Products by Category

```http
GET /api/products/by-category/{categoryId}
```

**Parameters**:

- `categoryId` (int, required) - ID категорії

**Response**: 200 OK

```json
[
  {
    "id": 1,
    "name": "Ноутбук",
    "categoryId": 1,
    "categoryName": "Електроніка",
    "unit": "шт",
    "dateAdded": "2024-12-11"
  }
]
```

**Response**: 404 Not Found

```json
{
  "message": "Категорія з ID 999 не знайдена"
}
```

---

#### 4. Create Product

```http
POST /api/products
Content-Type: application/json
```

**Request Body**:

```json
{
  "name": "Ноутбук HP",
  "categoryId": 1,
  "unit": "шт"
}
```

**Validation Rules**:

- `name` - обов'язково, від 2 до 150 символів
- `categoryId` - обов'язково, мінімум 1
- `unit` - обов'язково, від 1 до 20 символів

**Response**: 201 Created

```json
{
  "id": 5,
  "name": "Ноутбук HP",
  "categoryId": 1,
  "categoryName": "Електроніка",
  "unit": "шт",
  "dateAdded": "2024-12-11"
}
```

**Response**: 404 Not Found

```json
{
  "message": "Категорія з ID 999 не знайдена"
}
```

**Response**: 409 Conflict

```json
{
  "message": "Продукт з такою назвою вже існує в цій категорії"
}
```

---

#### 5. Update Product

```http
PUT /api/products/{id}
Content-Type: application/json
```

**Parameters**:

- `id` (int, required) - ID продукту

**Request Body** (усі поля опційні для часткового оновлення):

```json
{
  "name": "Ноутбук Dell",
  "categoryId": 2,
  "unit": "пк"
}
```

**Response**: 200 OK

```json
{
  "id": 1,
  "name": "Ноутбук Dell",
  "categoryId": 2,
  "categoryName": "Комп'ютерна техніка",
  "unit": "пк",
  "dateAdded": "2024-12-11"
}
```

---

#### 6. Delete Product

```http
DELETE /api/products/{id}
```

**Response**: 204 No Content

---

## Використання з Postman

1. **Імпортуйте колекцію**: `WarehouseAPI.postman_collection.json`
2. **Встановіть змінну**: `base_url` = `http://localhost:5000`
3. **Запускайте запити** з готовими шаблонами

---

## Error Handling

### HTTP Status Codes:

- **200 OK** - успішна операція
- **201 Created** - ресурс створено
- **204 No Content** - успішне видалення
- **400 Bad Request** - помилка валідації
- **404 Not Found** - ресурс не знайдено
- **409 Conflict** - конфлікт (дублікат тощо)
- **500 Internal Server Error** - серверна помилка

### Приклади помилок:

```json
{
  "message": "Описання помилки"
}
```

---

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
├── Controllers/
│   ├── CategoriesController.cs
│   └── ProductsController.cs
├── DTOs/
│   ├── CategoryDTO.cs
│   └── ProductDTO.cs
├── Mappings/
│   └── MappingProfile.cs
├── Models/
│   ├── Category.cs
│   ├── Product.cs
│   └── ... (інші моделі)
├── Context/
│   └── WarehouseDbContext.cs
├── Program.cs
├── appsettings.json
└── WarehouseAPI.postman_collection.json
```

---

## Validation Rules Summary

### Category

| Field | Type   | Rules                                |
| ----- | ------ | ------------------------------------ |
| Name  | string | Required, 2-100 chars, Cyrillic only |

### Product

| Field      | Type   | Rules                 |
| ---------- | ------ | --------------------- |
| Name       | string | Required, 2-150 chars |
| CategoryId | int    | Required, ≥ 1         |
| Unit       | string | Required, 1-20 chars  |

---

## Future Enhancements

- [ ] JWT Authentication
- [ ] Role-based Access Control
- [ ] Advanced filtering & pagination
- [ ] Logging middleware
- [ ] Custom error handling middleware
- [ ] Database seeding
- [ ] Integration tests
- [ ] API versioning (v1, v2, etc.)

---

**Last Updated**: 11 Dec 2024  
**Status**: Production Ready ✅
