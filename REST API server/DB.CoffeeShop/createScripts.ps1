# Список имен файлов
$fileNames = @(
    "DB.SelectFromViews.sql",
    "DB.SelectFromTables.sql",
    "DB.FillTables.sql",
    "DB.UpdateTables.sql",
    "DB.Cleanup.sql",
    "DB.Deployment.sql",
    "DB.ColumnRegularExpressions.sql",
    "DB.CreateViews.sql",
    "DB.CreateTables.sql",
    "DB.Create.sql",
    "DB.Table.PhoneCodes.sql"
)

# Получение текущей директории
$currentDirectory = Get-Location

# Создание файлов
foreach ($fileName in $fileNames) {
    $filePath = Join-Path -Path $currentDirectory -ChildPath $fileName
    New-Item -ItemType File -Path $filePath -Force | Out-Null
    Write-Host "Created file: $filePath"
}

# Генерация содержимого для файла DB.Deployment.sql
$deploymentContent = @"
--#region Cleanup DB
:r "$($currentDirectory)\DB.Cleanup.sql"
--#endregion
GO

--#region Delete previous tables, relations, views
:r "$($currentDirectory)\DB.Cleanup.sql"
--#endregion
GO

--#region Create tables and relations
:r "$($currentDirectory)\DB.CreateTables.sql"
--#endregion
GO

--#region Update tables and relations
:r "$($currentDirectory)\DB.UpdateTables.sql"
--#endregion
GO

--#region Create table for phone codes
:r "$($currentDirectory)\DB.Table.PhoneCodes.sql"
--#endregion
GO

--#region Column regular expressions
:r "$($currentDirectory)\DB.ColumnRegularExpressions.sql"
--#endregion
GO

--#region Create views
:r "$($currentDirectory)\DB.CreateViews.sql"
--#endregion
GO

--#region Fill tables
:r "$($currentDirectory)\DB.FillTables.sql"
--#endregion
GO

--#region Select from tables
:r "$($currentDirectory)\DB.SelectFromTables.sql"
--#endregion
GO

--#region Select from views
:r "$($currentDirectory)\DB.SelectFromViews.sql"
--#endregion
GO
"@

# Запись содержимого в DB.Deployment.sql
$deploymentFilePath = Join-Path -Path $currentDirectory -ChildPath "DB.Deployment.sql"
Set-Content -Path $deploymentFilePath -Value $deploymentContent

Write-Host "Deployment script created: $deploymentFilePath"
Write-Host "All files have been created successfully."
