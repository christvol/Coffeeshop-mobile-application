GO
CREATE TRIGGER trg_ValidatePhoneNumber
ON Users
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Обновляем данные в таблице Users, удаляя "+" и пробелы из номера телефона
    UPDATE Users
    SET phoneNumber = REPLACE(REPLACE(inserted.phoneNumber, '+', ''), ' ', '')
    FROM Users
    INNER JOIN inserted ON Users.id = inserted.id;

    -- Проверяем, что телефонный номер состоит из 10-15 цифр
    IF EXISTS (
        SELECT 1
        FROM inserted
        WHERE NOT (
            LEN(REPLACE(REPLACE(inserted.phoneNumber, '+', ''), ' ', '')) BETWEEN 10 AND 15
            AND REPLACE(REPLACE(inserted.phoneNumber, '+', ''), ' ', '') LIKE REPLICATE('[0-9]', LEN(REPLACE(REPLACE(inserted.phoneNumber, '+', ''), ' ', '')))
        )
    )
    BEGIN
        -- Вызываем ошибку, если номер не соответствует формату
        RAISERROR ('Phone number must be between 10 and 15 digits.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO
