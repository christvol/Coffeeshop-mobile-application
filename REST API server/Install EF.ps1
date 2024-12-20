clear
# Настройка кодировки UTF-8 для консоли
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Получение текущей директории, где лежит скрипт
$currentDirectory = (Get-Location).Path

# Переход в каталог текущей папки
Set-Location -Path $currentDirectory

# Установка базового пакета Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore

# Установка провайдера для Microsoft SQL Server
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

# Установка инструментов для миграций
dotnet add package Microsoft.EntityFrameworkCore.Tools

# Установка глобального инструмента dotnet-ef
dotnet tool install --global dotnet-ef
