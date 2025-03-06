clear
# Настройка кодировки UTF-8 для консоли
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Проверяем версию dotnet-ef
dotnet ef --version

# Путь к проекту и файл проекта
$currentDirectory = (Get-Location).Path
$projectFilePath = Join-Path -Path $currentDirectory -ChildPath "DB migration tool.csproj"

# Проверка наличия пакетов и установка при необходимости
function Ensure-PackageInstalled {
    param (
        [string]$packageName
    )
    $output = dotnet list package | Select-String $packageName
    if (-Not $output) {
        Write-Output "Пакет $packageName не найден. Устанавливаем..."
        dotnet add package $packageName
    } else {
        Write-Output "Пакет $packageName уже установлен."
    }
}

Write-Output "Проверяем и устанавливаем необходимые пакеты..."
Set-Location -Path $currentDirectory
try {
    Ensure-PackageInstalled -packageName "Microsoft.EntityFrameworkCore"
    Ensure-PackageInstalled -packageName "Microsoft.EntityFrameworkCore.SqlServer"
    Ensure-PackageInstalled -packageName "Microsoft.EntityFrameworkCore.Tools"
    Ensure-PackageInstalled -packageName "Microsoft.EntityFrameworkCore.Design"
    Write-Output "Необходимые пакеты успешно проверены и установлены."
} catch {
    Write-Output "Ошибка при установке пакетов: ${_}"
    exit 1
}

# Проверяем существование файла проекта
if (-Not (Test-Path -Path $projectFilePath)) {
    Write-Output "Ошибка: файл проекта $projectFilePath не найден. Скрипт остановлен."
    exit 1
}

# Определяем пути для DB
$serverDbPath = Join-Path -Path $currentDirectory -ChildPath "Classes\DB"

# Проверяем существование папки и выводим предупреждение
if (Test-Path -Path $serverDbPath) {
    Write-Output "Папка $serverDbPath уже существует. Её содержимое будет перезаписано."
    $response = Read-Host "Вы уверены, что хотите продолжить? (д/н / y/n)"
    if ($response -notin @("д", "y", "Y", "Д")) {
        Write-Output "Операция отменена пользователем."
        exit 0
    }

    # Удаляем содержимое папки
    Write-Output "Удаляем содержимое папки $serverDbPath..."
    try {
        Remove-Item -Path "$serverDbPath\*" -Recurse -Force
        Write-Output "Содержимое папки $serverDbPath успешно удалено."
    } catch {
        Write-Output "Ошибка при удалении содержимого папки $serverDbPath : ${_}"
        exit 1
    }
} else {
    # Создаём папку, если её нет
    Write-Output "Папка $serverDbPath не существует. Создаем папку..."
    New-Item -Path $serverDbPath -ItemType Directory | Out-Null
    if (Test-Path -Path $serverDbPath) {
        Write-Output "Папка $serverDbPath успешно создана."
    } else {
        Write-Output "Ошибка при создании папки $serverDbPath."
        exit 1
    }
}

# Scaffold DbContext
Write-Output "Выполняем scaffold DbContext..."
try {
    $output = dotnet ef dbcontext scaffold "Server=LAPTOP-BBFM8MMD\SQLEXPRESS;Database=CoffeeShop;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer `
        --output-dir Classes\DB --no-pluralize --project "$projectFilePath" 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Output "Scaffold DbContext успешно выполнен."

        # Определяем путь к общему проекту относительно решения
        $solutionDirectory = Join-Path -Path $currentDirectory -ChildPath ".."
        $commonProjectDbPath = Join-Path -Path $solutionDirectory -ChildPath "Common\Classes\DB"

        # Проверяем существование папки и очищаем содержимое
        if (-Not (Test-Path -Path $commonProjectDbPath)) {
            Write-Output "Папка $commonProjectDbPath не существует. Создаем папку..."
            New-Item -Path $commonProjectDbPath -ItemType Directory | Out-Null
            if (Test-Path -Path $commonProjectDbPath) {
                Write-Output "Папка $commonProjectDbPath успешно создана."
            } else {
                Write-Output "Ошибка при создании папки $commonProjectDbPath."
                exit 1
            }
        } else {
            Write-Output "Папка $commonProjectDbPath уже существует. Очищаем содержимое..."
            try {
                Remove-Item -Path "$commonProjectDbPath\*" -Recurse -Force
                Write-Output "Содержимое папки $commonProjectDbPath успешно удалено."
            } catch {
                Write-Output "Ошибка при удалении содержимого папки $commonProjectDbPath : ${_}"
                exit 1
            }
        }

        # Копируем сгенерированные файлы
        Write-Output "Копируем файлы из $serverDbPath в $commonProjectDbPath..."
        try {
            Copy-Item -Path "$serverDbPath\*" -Destination "$commonProjectDbPath" -Recurse -Force
            Write-Output "Файлы успешно скопированы в $commonProjectDbPath."
        } catch {
            Write-Output "Ошибка при копировании файлов в $commonProjectDbPath : ${_}"
            exit 1
        }

        # Возврат в исходный каталог
        Set-Location -Path $currentDirectory
        Write-Output "Возврат в исходный каталог: $currentDirectory."
    } else {
        Write-Output "Scaffold DbContext завершился с ошибками:"
        Write-Output $output
    }
} catch {
    Write-Output "Ошибка при выполнении scaffold DbContext: $_"
}
