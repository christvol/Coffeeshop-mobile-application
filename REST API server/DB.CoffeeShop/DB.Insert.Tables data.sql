-- Устанавливает международный формат (YYYY-MM-DD)
SET DATEFORMAT YMD; 

-- Insert into User types
INSERT INTO [User types] (title) VALUES
('Admin'),
('Customer'),
('Employee');
GO

-- Insert into Users
INSERT INTO [Users] (idUserType, firstName, lastName, creationDate, birthDate, email, phoneNumber, idImage) VALUES
(1, 'John', 'Doe', GETDATE(), '1980-05-15', 'john.doe@example.com', '72345678930', NULL),
(2, 'Jane', 'Smith', GETDATE(), '1992-08-25', 'jane.smith@example.com', '70987654321', NULL),
(3, 'Michael', 'Brown', GETDATE(), '1985-01-20', 'michael.brown@example.com', '71122334455', NULL);
GO

-- Insert into Product types
INSERT INTO [Product types] (title) VALUES
('Coffee'),
('Tea'),
('Pastry');
GO

-- Insert into Products
INSERT INTO [Products] (title, description, fee, idProductType) VALUES
('Espresso', 'Strong coffee', 2.50, 1),
('Green Tea', 'Refreshing green tea', 1.50, 2),
('Croissant', 'Freshly baked croissant', 3.00, 3),
('Latte', 'Espresso with steamed milk', 3.50, 1),
('Cappuccino', 'Espresso with frothed milk', 3.80, 1),
('Americano', 'Espresso with hot water', 2.80, 1),
('Mocha', 'Espresso with chocolate and steamed milk', 4.00, 1),
('Black Tea', 'Strong black tea', 1.80, 2),
('Chamomile Tea', 'Herbal tea with chamomile', 2.20, 2),
('Mint Tea', 'Refreshing tea with mint', 2.50, 2),
('Jasmine Tea', 'Aromatic jasmine green tea', 2.70, 2),
('Muffin', 'Chocolate chip muffin', 2.50, 3),
('Bagel', 'Freshly baked bagel', 2.00, 3),
('Cheesecake', 'Rich and creamy cheesecake', 4.50, 3),
('Danish Pastry', 'Sweet pastry with fruit filling', 3.80, 3);
GO

-- Insert into Ingredient types
INSERT INTO [Ingredient types] (title) VALUES
('Milk'),
('Sugar'),
('Syrup'),
('Spice'),
('Topping'),
('Cream');
GO

-- Insert into Ingredients
INSERT INTO [Ingredients] (idIngredientType, title, description, fee) VALUES
(1, 'Whole Milk', 'Rich and creamy milk', 0.50),
(2, 'Brown Sugar', 'Unrefined sugar', 0.30),
(3, 'Vanilla Syrup', 'Sweet vanilla flavor', 0.70),
(1, 'Skim Milk', 'Low fat milk', 0.40),
(1, 'Soy Milk', 'Plant-based milk alternative', 0.60),
(2, 'White Sugar', 'Refined sugar', 0.25),
(3, 'Caramel Syrup', 'Sweet caramel flavor', 0.70),
(3, 'Hazelnut Syrup', 'Nutty and sweet', 0.75),
(4, 'Cinnamon', 'Spicy and aromatic', 0.20),
(5, 'Chocolate Chips', 'Sweet topping', 0.50),
(6, 'Whipped Cream', 'Fluffy cream topping', 0.60);
GO

-- Insert into Images
-- Вставка изображений в таблицу Images с данными из файлов
-- Каждый путь — локальный. Убедись, что SQL Server имеет доступ к этим файлам

-- Espresso
INSERT INTO Images (title, description, url, data)
SELECT 
    'Espresso Image',
    'Image of espresso',
    '/images/Espresso.png',
    BulkColumn
FROM OPENROWSET(BULK N'C:\Users\FossW\source\repos\Coffeeshop-mobile-application\REST API server\DB.CoffeeShop\images\Espresso.png', SINGLE_BLOB) AS img;

-- Green Tea
INSERT INTO Images (title, description, url, data)
SELECT 
    'Green Tea Image',
    'Image of green tea',
    '/images/Green Tea.png',
    BulkColumn
FROM OPENROWSET(BULK N'C:\Users\FossW\source\repos\Coffeeshop-mobile-application\REST API server\DB.CoffeeShop\images\Green Tea.png', SINGLE_BLOB) AS img;

-- Croissant
INSERT INTO Images (title, description, url, data)
SELECT 
    'Croissant Image',
    'Image of croissant',
    '/images/Croissant.png',
    BulkColumn
FROM OPENROWSET(BULK N'C:\Users\FossW\source\repos\Coffeeshop-mobile-application\REST API server\DB.CoffeeShop\images\Croissant.png', SINGLE_BLOB) AS img;

-- NoImageAvailable (заглушка)
INSERT INTO Images (title, description, url, data)
SELECT 
    'NoImageAvailable',
    'Placeholder for missing images',
    '/images/NoImageAvailable.png',
    BulkColumn
FROM OPENROWSET(BULK N'C:\Users\FossW\source\repos\Coffeeshop-mobile-application\REST API server\DB.CoffeeShop\images\NoImageAvailable.png', SINGLE_BLOB) AS img;
GO

-- Insert into Allowed ingredients (каждому продукту — минимум 1-2 ингредиента)
INSERT INTO [Allowed ingredients] (idIngredient, idProduct, allowedNumber) VALUES
(1, 1, 2), (4, 1, 1),        -- Espresso
(2, 2, 2), (9, 2, 1),        -- Green Tea
(3, 3, 1), (10, 3, 1),       -- Croissant
(1, 4, 2), (11, 4, 1),       -- Latte
(1, 5, 2), (9, 5, 1),        -- Cappuccino
(2, 6, 2), (7, 6, 1),        -- Americano
(7, 7, 1), (11, 7, 1),       -- Mocha
(3, 8, 1), (9, 8, 1),        -- Black Tea
(3, 9, 1), (6, 9, 1),        -- Chamomile Tea
(2, 10, 1), (10, 10, 1),     -- Mint Tea
(2, 11, 1), (9, 11, 1),      -- Jasmine Tea
(3, 12, 1), (10, 12, 1),     -- Muffin
(3, 13, 1), (10, 13, 1),     -- Bagel
(3, 14, 1), (8, 14, 1),      -- Cheesecake
(3, 15, 1), (7, 15, 1);      -- Danish Pastry
GO

-- Insert into OrderStatuses
INSERT INTO [OrderStatuses] (title) VALUES
('Created'),
('Pending'),
('In Progress'),
('Completed'),
('Cancelled');
GO

-- Insert into PaymentStatuses
INSERT INTO [PaymentStatuses] (title) VALUES
('Not Paid'),
('Paid'),
('Refunded');
GO

-- Insert into Orders — теперь обязательно указывать idStatusPayment
INSERT INTO [Orders] (creationDate, idCustomer, idEmployee, idStatus, idStatusPayment) VALUES
(GETDATE(), 1, 2, 3, 2), -- Completed, Paid
(GETDATE(), 2, 3, 3, 2); -- Completed, Paid
GO

-- Insert into Order products
INSERT INTO [Order products] (idProduct, total) VALUES
(1, 5.00), -- 2 × Espresso
(3, 3.00), -- 1 × Croissant
(2, 1.50); -- 1 × Green Tea
GO

-- Insert into Order items
INSERT INTO [Order items] (idOrder, idOrderProduct) VALUES
(1, 1),
(1, 2),
(2, 3);
GO

-- Insert into Order item ingredients
INSERT INTO [Order item ingredients] (idOrderProduct, idIngredient, amount) VALUES
(1, 1, 1), -- Whole Milk
(1, 2, 2), -- Brown Sugar
(3, 3, 1); -- Vanilla Syrup
GO

-- Insert into Product images
-- Привязка товаров к изображениям
-- Предположим, что id изображений: 1 = Espresso, 2 = Green Tea, 3 = Croissant, 4 = Заглушка

INSERT INTO [Product images] (idProduct, idImage)
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 4),
(6, 4),
(7, 4),
(8, 4),
(9, 4),
(10, 4),
(11, 4),
(12, 4),
(13, 4),
(14, 4),
(15, 4);
GO

-- Insert into Ingredient images
INSERT INTO [Ingredient images] (idIngredient, idImage) VALUES
(1, 1),
(2, 2),
(3, 3);
GO
