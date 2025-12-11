# Warehouse API - Test Examples with PowerShell
# API базується на http://localhost:5000

$baseUrl = "http://localhost:5000/api"
$headers = @{"Content-Type" = "application/json"}

Write-Host "========== WAREHOUSE API - TEST EXAMPLES ==========" -ForegroundColor Cyan
Write-Host "Base URL: $baseUrl" -ForegroundColor Yellow
Write-Host "Переконайтесь, що API запущена перед запуском цих команд" -ForegroundColor Yellow
Write-Host ""

# ==================== CATEGORIES TESTS ====================
Write-Host "========== CATEGORIES TESTS ==========" -ForegroundColor Green

Write-Host "1. Get All Categories" -ForegroundColor Cyan
$response = Invoke-RestMethod -Uri "$baseUrl/categories" -Method Get
$response | ConvertTo-Json | Write-Host
Write-Host ""

Write-Host "2. Create Category" -ForegroundColor Cyan
$body = @{name = "Електроніка"} | ConvertTo-Json
$response = Invoke-RestMethod -Uri "$baseUrl/categories" -Method Post -Headers $headers -Body $body
$response | ConvertTo-Json | Write-Host
$categoryId = $response.id
Write-Host "Created Category ID: $categoryId" -ForegroundColor Yellow
Write-Host ""

Write-Host "3. Get Category by ID (ID=$categoryId)" -ForegroundColor Cyan
$response = Invoke-RestMethod -Uri "$baseUrl/categories/$categoryId" -Method Get
$response | ConvertTo-Json | Write-Host
Write-Host ""

Write-Host "4. Update Category (ID=$categoryId)" -ForegroundColor Cyan
$body = @{name = "Оновлена Електроніка"} | ConvertTo-Json
$response = Invoke-RestMethod -Uri "$baseUrl/categories/$categoryId" -Method Put -Headers $headers -Body $body
$response | ConvertTo-Json | Write-Host
Write-Host ""

Write-Host "5. Delete Category (ID=$categoryId)" -ForegroundColor Cyan
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/categories/$categoryId" -Method Delete
    Write-Host "Category deleted successfully" -ForegroundColor Green
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# ==================== PRODUCTS TESTS ====================
Write-Host "========== PRODUCTS TESTS ==========" -ForegroundColor Green

Write-Host "1. Get All Products" -ForegroundColor Cyan
$response = Invoke-RestMethod -Uri "$baseUrl/products" -Method Get
$response | ConvertTo-Json | Write-Host
Write-Host ""

Write-Host "2. Create Product" -ForegroundColor Cyan
$body = @{
    name = "Ноутбук HP"
    categoryId = 1
    unit = "шт"
} | ConvertTo-Json
$response = Invoke-RestMethod -Uri "$baseUrl/products" -Method Post -Headers $headers -Body $body
$response | ConvertTo-Json | Write-Host
$productId = $response.id
Write-Host "Created Product ID: $productId" -ForegroundColor Yellow
Write-Host ""

Write-Host "3. Get Product by ID (ID=$productId)" -ForegroundColor Cyan
$response = Invoke-RestMethod -Uri "$baseUrl/products/$productId" -Method Get
$response | ConvertTo-Json | Write-Host
Write-Host ""

Write-Host "4. Get Products by Category (Category ID=1)" -ForegroundColor Cyan
$response = Invoke-RestMethod -Uri "$baseUrl/products/by-category/1" -Method Get
$response | ConvertTo-Json | Write-Host
Write-Host ""

Write-Host "5. Update Product (ID=$productId)" -ForegroundColor Cyan
$body = @{
    name = "Ноутбук Dell"
    categoryId = 1
    unit = "пк"
} | ConvertTo-Json
$response = Invoke-RestMethod -Uri "$baseUrl/products/$productId" -Method Put -Headers $headers -Body $body
$response | ConvertTo-Json | Write-Host
Write-Host ""

Write-Host "6. Delete Product (ID=$productId)" -ForegroundColor Cyan
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/products/$productId" -Method Delete
    Write-Host "Product deleted successfully" -ForegroundColor Green
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "========== ТЕСТИ ЗАВЕРШЕНО ==========" -ForegroundColor Cyan
