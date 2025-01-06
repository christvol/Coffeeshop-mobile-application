namespace REST_API_SERVER.Classes
{
    public static class CommonLocal
    {
        public static class Strings
        {
            public static class ErrorMessages
            {
                public const string PhoneNumberRequired = "Phone number is required.";
                public const string InvalidPhoneNumberFormat = "Invalid phone number format. Expected format: + followed by 11 to 20 digits.";
                public const string VerificationCodeExpired = "The verification code has expired. Please request a new one.";
                public const string VerificationCodeNotFound = "Verification code not found. Please request a new one.";
                public const string InvalidVerificationCode = "Invalid verification code.";
                public const string ServerError = "Internal server error. Please try again later.";
                public const string CountryNotFoundById = "Country with the specified ID not found.";
                public const string CountryNotFoundByTicker = "Country with the specified ticker not found.";
            }

            public static class SuccessMessages
            {
                public const string VerificationCodeSent = "Verification code sent.";
            }

            public static class ValidationPatterns
            {
                public const string PhoneNumber = @"^\+\d{11,20}$";
            }
        }
    }
}
