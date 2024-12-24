clear
# Настройка кодировки UTF-8 для консоли
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Путь к проекту
$projectPath = "C:\Users\kmeen\source\repos\Мобильное приложение\Сервер REST-API"

# Переход в каталог проекта
Set-Location -Path $projectPath

# Установка базового пакета Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore

# Установка провайдера для Microsoft SQL Server
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# Установка инструментов для миграций
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Установка глобального инструмента dotnet-ef
dotnet tool install --global dotnet-ef