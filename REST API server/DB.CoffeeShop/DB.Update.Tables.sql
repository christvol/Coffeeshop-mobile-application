-- Set default value for the "creationDate" column in the "Users" table
ALTER TABLE [Users]
ADD CONSTRAINT DF_Users_CreationDate DEFAULT GETDATE() FOR [creationDate];

-- Set default value for the "creationDate" column in the "Orders" table
ALTER TABLE [Orders]
ADD CONSTRAINT DF_Orders_CreationDate DEFAULT GETDATE() FOR [creationDate];
