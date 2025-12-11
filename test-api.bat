@echo off
REM Warehouse API - Test Examples with curl
REM API базується на http://localhost:5000

echo.
echo ==== WAREHOUSE API - TEST EXAMPLES ====
echo.
echo Base URL: http://localhost:5000
echo Переконайтесь, що API запущена перед запуском цих команд
echo.

REM ==================== CATEGORIES TESTS ====================
echo.
echo ========== CATEGORIES TESTS ==========
echo.

echo 1. Get All Categories
echo =====================
curl -X GET http://localhost:5000/api/categories
echo.
echo.

echo 2. Create Category
echo ==================
curl -X POST http://localhost:5000/api/categories ^
  -H "Content-Type: application/json" ^
  -d "{\"name\": \"Електроніка\"}"
echo.
echo.

echo 3. Get Category by ID (ID=1)
echo ============================
curl -X GET http://localhost:5000/api/categories/1
echo.
echo.

echo 4. Update Category (ID=1)
echo =========================
curl -X PUT http://localhost:5000/api/categories/1 ^
  -H "Content-Type: application/json" ^
  -d "{\"name\": \"Оновлена Електроніка\"}"
echo.
echo.

echo 5. Delete Category (ID=1)
echo ==========================
curl -X DELETE http://localhost:5000/api/categories/1
echo.
echo.

REM ==================== PRODUCTS TESTS ====================
echo.
echo ========== PRODUCTS TESTS ==========
echo.

echo 1. Get All Products
echo ===================
curl -X GET http://localhost:5000/api/products
echo.
echo.

echo 2. Create Product
echo =================
curl -X POST http://localhost:5000/api/products ^
  -H "Content-Type: application/json" ^
  -d "{\"name\": \"Ноутбук HP\", \"categoryId\": 1, \"unit\": \"шт\"}"
echo.
echo.

echo 3. Get Product by ID (ID=1)
echo ============================
curl -X GET http://localhost:5000/api/products/1
echo.
echo.

echo 4. Get Products by Category (Category ID=1)
echo ============================================
curl -X GET http://localhost:5000/api/products/by-category/1
echo.
echo.

echo 5. Update Product (ID=1)
echo =========================
curl -X PUT http://localhost:5000/api/products/1 ^
  -H "Content-Type: application/json" ^
  -d "{\"name\": \"Ноутбук Dell\", \"categoryId\": 2, \"unit\": \"пк\"}"
echo.
echo.

echo 6. Delete Product (ID=1)
echo =========================
curl -X DELETE http://localhost:5000/api/products/1
echo.
echo.

echo.
echo ==== ТЕСТИ ЗАВЕРШЕНО ====
echo.
pause
