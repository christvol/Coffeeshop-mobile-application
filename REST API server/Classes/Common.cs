namespace REST_API_SERVER.Classes
{
    public static class Common
    {
        public static class Strings
        {
            public static class ErrorMessages
            {
                public const string PhoneNumberRequired = "Номер телефона обязателен.";
                public const string InvalidPhoneNumberFormat = "Неверный формат номера телефона. Ожидается формат: + и от 11 до 20 цифр.";
                public const string VerificationCodeExpired = "Код истек. Пожалуйста, запросите новый.";
                public const string VerificationCodeNotFound = "Код не найден. Пожалуйста, запросите новый.";
                public const string InvalidVerificationCode = "Неверный код.";
                public const string ServerError = "Внутренняя ошибка сервера. Пожалуйста, попробуйте позже.";
                public const string CountryNotFoundById = "Страна с указанным ID не найдена.";
                public const string CountryNotFoundByTicker = "Страна с указанным тикером не найдена.";
            }

            public static class SuccessMessages
            {
                public const string VerificationCodeSent = "Код подтверждения отправлен.";
            }

            public static class ValidationPatterns
            {
                public const string PhoneNumber = @"^\+\d{11,20}$";
            }
        }
    }
}
