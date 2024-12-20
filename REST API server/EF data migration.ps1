clear
# Настройка кодировки UTF-8 для консоли
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Путь до решения
$solutionPath = "C:\Users\kmeen\source\repos\Coffeeshop mobile application"

# Путь к проектам
$restApiProjectPath = Join-Path $solutionPath "REST API server"
$mobileAppProjectPath = Join-Path $solutionPath "Mobile application"

# Пути к папкам Classes\DB для каждого проекта
$restApiDbPath = Join-Path $restApiProjectPath "Classes\DB"
$mobileAppDbPath = Join-Path $mobileAppProjectPath "Classes\DB"

# Проверяем версию dotnet-ef
dotnet ef --version

# Создаём папки Classes\DB, если их нет
if (!(Test-Path -Path $restApiDbPath)) {
    New-Item -ItemType Directory -Path $restApiDbPath -Force | Out-Null
    Write-Host "Created directory: $restApiDbPath"
}

if (!(Test-Path -Path $mobileAppDbPath)) {
    New-Item -ItemType Directory -Path $mobileAppDbPath -Force | Out-Null
    Write-Host "Created directory: $mobileAppDbPath"
}

# Удаляем все файлы и папки в целевых директориях
Remove-Item -Path (Join-Path $restApiDbPath "*") -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path (Join-Path $mobileAppDbPath "*") -Recurse -Force -ErrorAction SilentlyContinue

# Переходим в папку REST API проекта
Set-Location -Path $restApiProjectPath

# Scaffold DbContext
dotnet ef dbcontext scaffold `
    "Server=KRISTINAVIVO\SQLEXPRESS;Database=CoffeeShop;Trusted_Connection=True;TrustServerCertificate=True;" `
    Microsoft.EntityFrameworkCore.SqlServer `
    --output-dir Classes\DB `
    --no-pluralize `
    --project (Join-Path $restApiProjectPath "REST API server.csproj")

# Копируем файлы из REST API в Mobile Application
Copy-Item -Path (Join-Path $restApiDbPath "*") `
    -Destination $mobileAppDbPath `
    -Recurse -Force
