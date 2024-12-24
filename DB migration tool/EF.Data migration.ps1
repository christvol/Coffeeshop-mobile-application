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

        # Переход на каталог решения
        $solutionDirectory = Join-Path -Path $currentDirectory -ChildPath ".."
        Set-Location -Path $solutionDirectory

        # Определяем пути к проектам
        $restApiServerDbPath = Join-Path -Path $solutionDirectory -ChildPath "REST API server\Classes\DB"
        $mobileAppDbPath = Join-Path -Path $solutionDirectory -ChildPath "Mobile application\Classes\DB"
        $migrationToolDbPath = Join-Path -Path $solutionDirectory -ChildPath "DB migration tool\Classes\DB"

        $targetPaths = @($restApiServerDbPath, $mobileAppDbPath, $migrationToolDbPath)

        foreach ($targetPath in $targetPaths) {
            # Проверяем существование папки и очищаем содержимое
            if (-Not (Test-Path -Path $targetPath)) {
                Write-Output "Папка $targetPath не существует. Создаем папку..."
                New-Item -Path $targetPath -ItemType Directory | Out-Null
                if (Test-Path -Path $targetPath) {
                    Write-Output "Папка $targetPath успешно создана."
                } else {
                    Write-Output "Ошибка при создании папки $targetPath."
                }
            } else {
                Write-Output "Папка $targetPath уже существует. Очищаем содержимое..."
                try {
                    Remove-Item -Path "$targetPath\*" -Recurse -Force
                    Write-Output "Содержимое папки $targetPath успешно удалено."
                } catch {
                    Write-Output "Ошибка при удалении содержимого папки $targetPath : ${_}"
                }
            }

            # Копируем сгенерированные файлы
            Write-Output "Копируем файлы из $serverDbPath в $targetPath..."
            try {
                Copy-Item -Path "$serverDbPath\*" -Destination "$targetPath" -Recurse -Force
                Write-Output "Файлы успешно скопированы в $targetPath."
            } catch {
                Write-Output "Ошибка при копировании файлов в $targetPath : ${_}"
            }
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
