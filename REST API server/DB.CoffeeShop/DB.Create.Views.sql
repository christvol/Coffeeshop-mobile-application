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
    oii.amount AS IngredientQuantity,
    ROUND(totalOrder.TotalSum, 2) AS TotalSum,

    -- ?? Округлённая стоимость позиции: продукт + ингредиенты
    ROUND(
        p.fee +
        ISNULL((
            SELECT SUM(ISNULL(i2.fee, 0) * ISNULL(oii2.amount, 0))
            FROM [Order item ingredients] oii2
            LEFT JOIN Ingredients i2 ON oii2.idIngredient = i2.id
            WHERE oii2.idOrderProduct = op.id
        ), 0), 2
    ) AS OrderItemTotal

FROM Orders o
JOIN OrderStatuses os ON o.idStatus = os.id
JOIN PaymentStatuses ps ON o.idStatusPayment = ps.id
JOIN [Users] u ON o.idCustomer = u.id
JOIN [Order items] oi ON o.id = oi.idOrder
JOIN [Order products] op ON oi.idOrderProduct = op.id
JOIN Products p ON op.idProduct = p.id
LEFT JOIN [Order item ingredients] oii ON op.id = oii.idOrderProduct
LEFT JOIN Ingredients i ON oii.idIngredient = i.id
LEFT JOIN (
    SELECT 
        o.id AS OrderId,
        ROUND(
            (
                SELECT SUM(p.fee)
                FROM [Order items] oi2
                JOIN [Order products] op2 ON oi2.idOrderProduct = op2.id
                JOIN Products p ON op2.idProduct = p.id
                WHERE oi2.idOrder = o.id
            ) +
            (
                SELECT SUM(ISNULL(i.fee, 0) * ISNULL(oii2.amount, 0))
                FROM [Order items] oi2
                JOIN [Order products] op2 ON oi2.idOrderProduct = op2.id
                LEFT JOIN [Order item ingredients] oii2 ON op2.id = oii2.idOrderProduct
                LEFT JOIN Ingredients i ON oii2.idIngredient = i.id
                WHERE oi2.idOrder = o.id
            ), 2
        ) AS TotalSum
    FROM Orders o
) AS totalOrder ON o.id = totalOrder.OrderId
GO





