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

# Проверяем существование папки и создаем, если она отсутствует
if (-Not (Test-Path -Path $serverDbPath)) {
    Write-Output "Папка $serverDbPath не существует. Создаем папку..."
    New-Item -Path $serverDbPath -ItemType Directory | Out-Null
    if (Test-Path -Path $serverDbPath) {
        Write-Output "Папка $serverDbPath успешно создана."
    } else {
        Write-Output "Ошибка при создании папки $serverDbPath."
    }
} else {
    Write-Output "Папка $serverDbPath уже существует."
}

# Удаляем все файлы и папки в целевой директории
if (Test-Path -Path "$serverDbPath\*") {
    Write-Output "Удаляем содержимое папки $serverDbPath..."
    try {
        Remove-Item -Path "$serverDbPath\*" -Recurse -Force
        Write-Output "Содержимое папки $serverDbPath успешно удалено."
    } catch {
        Write-Output "Ошибка при удалении содержимого папки $serverDbPath : ${_}"
    }
} else {
    Write-Output "Содержимое папки $serverDbPath отсутствует или уже удалено."
}

# Scaffold DbContext
Write-Output "Выполняем scaffold DbContext..."
try {
    $output = dotnet ef dbcontext scaffold "Server=KRISTINAVIVO\SQLEXPRESS;Database=CoffeeShop;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer `
        --output-dir Classes\DB --no-pluralize --project "$projectFilePath" 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Output "Scaffold DbContext успешно выполнен."

        # Переход на каталог выше
        Set-Location -Path (Join-Path -Path $currentDirectory -ChildPath "..")
        $restApiServerDbPath = Join-Path -Path (Join-Path -Path $currentDirectory -ChildPath "REST API server") -ChildPath "Classes\DB"

        # Проверяем существование папки REST API server\Classes\DB
        if (-Not (Test-Path -Path $restApiServerDbPath)) {
            Write-Output "Папка $restApiServerDbPath не существует. Создаем папку..."
            New-Item -Path $restApiServerDbPath -ItemType Directory | Out-Null
            if (Test-Path -Path $restApiServerDbPath) {
                Write-Output "Папка $restApiServerDbPath успешно создана."
            } else {
                Write-Output "Ошибка при создании папки $restApiServerDbPath."
            }
        } else {
            Write-Output "Папка $restApiServerDbPath уже существует. Очищаем содержимое..."
            try {
                Remove-Item -Path "$restApiServerDbPath\*" -Recurse -Force
                Write-Output "Содержимое папки $restApiServerDbPath успешно удалено."
            } catch {
                Write-Output "Ошибка при удалении содержимого папки $restApiServerDbPath : ${_}"
            }
        }

        # Копируем сгенерированные файлы в REST API server\Classes\DB
        Write-Output "Копируем файлы из $serverDbPath в $restApiServerDbPath..."
        try {
            Copy-Item -Path "$serverDbPath\*" -Destination "$restApiServerDbPath" -Recurse -Force
            Write-Output "Файлы успешно скопированы."
        } catch {
            Write-Output "Ошибка при копировании файлов: ${_}"
        }
    } else {
        Write-Output "Scaffold DbContext завершился с ошибками:"
        Write-Output $output
    }
} catch {
    Write-Output "Ошибка при выполнении scaffold DbContext: $_"
}
