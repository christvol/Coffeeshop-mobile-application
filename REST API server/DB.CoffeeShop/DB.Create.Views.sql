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
