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
('Syrup');
GO

-- Insert into Ingredients
INSERT INTO [Ingredients] (idIngredientType, title, description, fee) VALUES
(1, 'Whole Milk', 'Rich and creamy milk', 0.50),
(2, 'Brown Sugar', 'Unrefined sugar', 0.30),
(3, 'Vanilla Syrup', 'Sweet vanilla flavor', 0.70);
GO

-- Insert into Images
INSERT INTO [Images] (title, description, url) VALUES
('Espresso Image', 'Image of espresso', 'https://example.com/espresso.jpg'),
('Green Tea Image', 'Image of green tea', 'https://example.com/greentea.jpg'),
('Croissant Image', 'Image of croissant', 'https://example.com/croissant.jpg');
GO

-- Insert into Allowed ingredients
INSERT INTO [Allowed ingredients] (idIngredient, idProduct, allowedNumber) VALUES
(1, 1, 1), -- Whole Milk, Espresso
(2, 1, 2), -- Brown Sugar, Espresso
(3, 3, 1); -- Vanilla Syrup, Croissant
GO

-- Insert into OrderStatuses (добавление статусов для заказов)
INSERT INTO [OrderStatuses] (title) VALUES
('Pending'), -- заказ в ожидании
('In Progress'), -- заказ в процессе
('Completed'), -- заказ завершен
('Cancelled'); -- заказ отменен
GO

-- Insert into Orders
-- Добавляем статус заказа (например, статус "Pending")
INSERT INTO [Orders] (creationDate, idCustomer, idEmployee, idStatus) VALUES
(GETDATE(), 1, 2, 3), -- idCustomer: John Doe, idEmployee: Jane Smith, статус: Completed
(GETDATE(), 2, 3, 3); -- idCustomer: Jane Smith, idEmployee: Michael Brown, статус: Completed
GO

-- Insert into Order products (бывшая таблица OrderProducts)
-- Указываем, какие товары входят в заказ
INSERT INTO [Order products] (idProduct, total) VALUES
(1, 5.00), -- OrderProduct 1: Espresso (2шт)
(3, 3.00), -- OrderProduct 2: Croissant (1шт)
(2, 1.50); -- OrderProduct 3: Green Tea (1шт)
GO

-- Insert into Order items (бывшая таблица OrderItems)
-- Связываем заказ с элементами заказа
INSERT INTO [Order items] (idOrder, idOrderProduct) VALUES
(1, 1),
(1, 2),
(2, 3);
GO

-- Insert into Order item ingredients (бывшая таблица OrderItemIngredients)
-- Указываем, какие ингредиенты входят в состав товаров заказа
INSERT INTO [Order item ingredients] (idOrderProduct, idIngredient, amount) VALUES
(1, 1, 1), -- OrderProduct 1: Whole Milk
(1, 2, 2), -- OrderProduct 1: Brown Sugar
(3, 3, 1); -- OrderProduct 3: Vanilla Syrup
GO

-- Insert into Product images
INSERT INTO [Product images] (idProduct, idImage) VALUES
(1, 1), -- Espresso, Espresso Image
(2, 2), -- Green Tea, Green Tea Image
(3, 3); -- Croissant, Croissant Image
GO

-- Insert into Ingredient images
INSERT INTO [Ingredient images] (idIngredient, idImage) VALUES
(1, 1), -- Whole Milk, Espresso Image
(2, 2), -- Brown Sugar, Green Tea Image
(3, 3); -- Vanilla Syrup, Croissant Image
GO
