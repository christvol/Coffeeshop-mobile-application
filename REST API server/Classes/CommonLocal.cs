namespace REST_API_SERVER.Classes
{
    public static class CommonLocal
    {
        public static class Strings
        {
            #region CountryCodesController
            public static class CountryCodesController
            {
                public const string CountryNotFoundById = "Country with the specified ID not found.";
                public const string CountryNotFoundByTicker = "Country with the specified ticker not found.";
            }
            #endregion

            #region ProductTypesController
            public static class ProductTypesController
            {
                public const string ProductTypeNotFound = "Product type not found.";
                public const string ProductTypeNameRequired = "Product type name is required.";
                public const string ProductTypeIdMismatch = "Product type ID does not match.";
            }
            #endregion


            #region UsersController
            public static class UsersController
            {
                public const string UserNotFound = "User not found.";
                public const string UserPhoneNotFound = "User with the specified phone number not found.";
                public const string UserIdMismatch = "User ID mismatch.";
                public const string ConcurrencyConflict = "Concurrency conflict occurred while updating the user.";
            }
            #endregion

            #region AuthController
            public static class AuthController
            {
                public const string PhoneNumberRequired = "Phone number is required.";
                public const string InvalidPhoneNumberFormat = "Invalid phone number format. Expected format: + followed by 11 to 20 digits.";
                public const string VerificationCodeExpired = "The verification code has expired. Please request a new one.";
                public const string VerificationCodeNotFound = "Verification code not found. Please request a new one.";
                public const string InvalidVerificationCode = "Invalid verification code.";
                public const string ServerError = "Internal server error. Please try again later.";
                public const string VerificationCodeSent = "Verification code sent.";
            }
            #endregion


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
