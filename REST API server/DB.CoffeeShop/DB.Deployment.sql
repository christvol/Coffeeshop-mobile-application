--#region Create DB
:r "C:\Users\kmeen\Documents\SQL Server Management Studio\DB.CoffeeShop\DB.CoffeeShop\DB.Create.sql"
--#endregion
GO

--#region Delete previous tables, relations, views
:r "C:\Users\kmeen\Documents\SQL Server Management Studio\DB.CoffeeShop\DB.CoffeeShop\DB.Cleanup.sql"
--#endregion
GO

--#region Create tables and relations
:r "C:\Users\kmeen\Documents\SQL Server Management Studio\DB.CoffeeShop\DB.CoffeeShop\DB.Create.Tables.sql"
--#endregion
GO

--#region Create table for phone codes
:r "C:\Users\kmeen\Documents\SQL Server Management Studio\DB.CoffeeShop\DB.CoffeeShop\DB.Create.Table.PhoneCodes.sql"
--#endregion
GO

--#region Update tables and relations
:r "C:\Users\kmeen\Documents\SQL Server Management Studio\DB.CoffeeShop\DB.CoffeeShop\DB.Update.Tables.sql"
--#endregion
GO

--#region Create triggers
:r "C:\Users\kmeen\Documents\SQL Server Management Studio\DB.CoffeeShop\DB.CoffeeShop\DB.Create.Triggers.sql"
--#endregion
GO

--#region Create views
:r "C:\Users\kmeen\Documents\SQL Server Management Studio\DB.CoffeeShop\DB.CoffeeShop\DB.Create.Views.sql"
--#endregion
GO

--#region Fill tables
:r "C:\Users\kmeen\Documents\SQL Server Management Studio\DB.CoffeeShop\DB.CoffeeShop\DB.Insert.Tables data.sql"
--#endregion
GO

--#region Select from tables
:r "C:\Users\kmeen\Documents\SQL Server Management Studio\DB.CoffeeShop\DB.CoffeeShop\DB.Select.Tables.sql"
--#endregion
GO

--#region Select from views
:r "C:\Users\kmeen\Documents\SQL Server Management Studio\DB.CoffeeShop\DB.CoffeeShop\DB.Select.Views.sql"
--#endregion
GO
