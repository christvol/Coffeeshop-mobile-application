GO

CREATE VIEW IngredientsView AS
SELECT
    it.id AS IngredientTypeId,
    it.title AS IngredientTypeTitle,
    i.id AS IngredientId,
    i.idIngredientType AS IngredientTypeIdRef,
    i.title AS IngredientTitle,
    i.description AS IngredientDescription,
    i.fee AS IngredientFee,
    ai.id AS AllowedIngredientId,
    ai.idIngredient AS AllowedIngredientIdRef,
    ai.idProduct AS AllowedIngredientProductId,
    ai.allowedNumber AS AllowedIngredientNumber
FROM
    [Ingredient types] it
JOIN
    Ingredients i ON i.idIngredientType = it.id
JOIN
    [Allowed ingredients] ai ON ai.idIngredient = i.id;

GO

CREATE OR ALTER VIEW OrderDetailsView AS
SELECT 
    o.id AS OrderId,
    o.creationDate AS OrderDate,
    o.idCustomer AS CustomerId,
    u.firstName AS CustomerFirstName,
    u.lastName AS CustomerLastName,
    os.title AS OrderStatus,
    ps.title AS PaymentStatus,
    oi.id AS OrderItemId,
    op.id AS OrderProductId,
    op.idProduct AS ProductId,
    p.title AS ProductTitle,
    p.fee AS ProductPrice,
    oii.id AS OrderItemIngredientId,
    oii.idIngredient AS IngredientId,
    i.title AS IngredientTitle,
    i.idIngredientType AS IngredientTypeId,
    i.fee AS IngredientFee,
    oii.amount AS IngredientQuantity
FROM Orders o
JOIN OrderStatuses os ON o.idStatus = os.id
JOIN PaymentStatuses ps ON o.idStatusPayment = ps.id
JOIN [Users] u ON o.idCustomer = u.id
JOIN [Order items] oi ON o.id = oi.idOrder
JOIN [Order products] op ON oi.idOrderProduct = op.id
JOIN Products p ON op.idProduct = p.id
LEFT JOIN [Order item ingredients] oii ON op.id = oii.idOrderProduct
LEFT JOIN Ingredients i ON oii.idIngredient = i.id;
GO



