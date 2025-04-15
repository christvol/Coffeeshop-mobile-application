CREATE TABLE [Users] (
  [id] int IDENTITY (1, 1),
  [idUserType] int NOT NULL,
  [firstName] nvarchar(255),
  [lastName] nvarchar(255),
  [creationDate] datetime,
  [birthDate] datetime NOT NULL,
  [email] nvarchar(255),
  [phoneNumber] nvarchar(255) NOT NULL,
  [idImage] int,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [User types] (
  [id] int IDENTITY (1, 1),
  [title] nvarchar(255) NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Orders] (
  [id] int IDENTITY (1, 1),
  [creationDate] datetime NOT NULL,
  [idCustomer] int,
  [idEmployee] int,
  [idStatus] int NOT NULL DEFAULT 1,         -- "Created"
  [idStatusPayment] int NOT NULL DEFAULT 1,  -- "Not Paid"
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO


CREATE TABLE [Products] (
  [id] int IDENTITY (1, 1),
  [title] nvarchar(255) NOT NULL,
  [description] nvarchar(255),
  [fee] real NOT NULL,
  [idProductType] int NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Ingredients] (
  [id] int IDENTITY (1, 1),
  [idIngredientType] int,
  [title] nvarchar(255) NOT NULL,
  [description] nvarchar(255),
  [fee] real NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Ingredient types] (
  [id] int IDENTITY (1, 1),
  [title] nvarchar(255) NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Images] (
  [id] int IDENTITY (1, 1),
  [title] nvarchar(255),
  [description] nvarchar(255),
  [url] nvarchar(1024) NOT NULL,
  [data] varbinary(MAX),
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Ingredient images] (
  [id] tinyint IDENTITY (1, 1),
  [idIngredient] int NOT NULL,
  [idImage] int NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Product images] (
  [id] int IDENTITY (1, 1),
  [idProduct] int NOT NULL,
  [idImage] int NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Allowed ingredients] (
  [id] int IDENTITY (1, 1),
  [idIngredient] int NOT NULL,
  [idProduct] int NOT NULL,
  [allowedNumber] tinyint NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Product types] (
  [id] int IDENTITY (1, 1),
  [title] nvarchar(255) NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Order items] (
  [id] int IDENTITY (1, 1),
  [idOrder] int NOT NULL,
  [idOrderProduct] int NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Order products] (
  [id] int IDENTITY (1, 1),
  [idProduct] int,
  [total] real NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [Order item ingredients] (
  [id] int IDENTITY (1, 1),
  [idOrderProduct] int NOT NULL,
  [idIngredient] int,
  [amount] int NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [OrderStatuses] (
  [id] int IDENTITY (1, 1),
  [title] varchar(255),
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

CREATE TABLE [PaymentStatuses] (
  [id] int IDENTITY (1, 1),
  [title] nvarchar(255) NOT NULL,
  PRIMARY KEY ([id])
) ON [PRIMARY]
GO

-- FOREIGN KEYS WITH CASCADE WHERE APPLICABLE

ALTER TABLE [Users] 
  ADD FOREIGN KEY (idUserType) REFERENCES [User types] ([id]);

ALTER TABLE [Users] 
  ADD FOREIGN KEY (idImage) REFERENCES [Images] ([id]);

ALTER TABLE [Orders] 
  ADD FOREIGN KEY (idCustomer) REFERENCES [Users] ([id]);

ALTER TABLE [Orders] 
  ADD FOREIGN KEY (idEmployee) REFERENCES [Users] ([id]);

ALTER TABLE [Orders] 
  ADD FOREIGN KEY (idStatus) REFERENCES [OrderStatuses] ([id]);

ALTER TABLE [Orders] 
  ADD FOREIGN KEY (idStatusPayment) REFERENCES [PaymentStatuses] ([id]);

ALTER TABLE [Products] 
  ADD FOREIGN KEY (idProductType) REFERENCES [Product types] ([id]) ON DELETE CASCADE;

ALTER TABLE [Ingredients] 
  ADD FOREIGN KEY (idIngredientType) REFERENCES [Ingredient types] ([id]) ON DELETE CASCADE;

ALTER TABLE [Ingredient images] 
  ADD FOREIGN KEY (idIngredient) REFERENCES [Ingredients] ([id]) ON DELETE CASCADE;

ALTER TABLE [Ingredient images] 
  ADD FOREIGN KEY (idImage) REFERENCES [Images] ([id]);

ALTER TABLE [Product images] 
  ADD FOREIGN KEY (idProduct) REFERENCES [Products] ([id]) ON DELETE CASCADE;

ALTER TABLE [Product images] 
  ADD FOREIGN KEY (idImage) REFERENCES [Images] ([id]) ON DELETE CASCADE;

ALTER TABLE [Allowed ingredients] 
  ADD FOREIGN KEY (idIngredient) REFERENCES [Ingredients] ([id]) ON DELETE NO ACTION;

ALTER TABLE [Allowed ingredients] 
  ADD FOREIGN KEY (idProduct) REFERENCES [Products] ([id]) ON DELETE NO ACTION;

ALTER TABLE [Order items] 
  ADD FOREIGN KEY (idOrder) REFERENCES [Orders] ([id]) ON DELETE CASCADE;

ALTER TABLE [Order items] 
  ADD FOREIGN KEY (idOrderProduct) REFERENCES [Order products] ([id]) ON DELETE CASCADE;

ALTER TABLE [Order products] 
  ADD FOREIGN KEY (idProduct) REFERENCES [Products] ([id]) ON DELETE CASCADE;

ALTER TABLE [Order item ingredients] 
  ADD FOREIGN KEY (idOrderProduct) REFERENCES [Order products] ([id]) ON DELETE CASCADE;

ALTER TABLE [Order item ingredients] 
  ADD FOREIGN KEY (idIngredient) REFERENCES [Allowed ingredients] ([id]) ON DELETE CASCADE;
