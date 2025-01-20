namespace REST_API_SERVER.Classes
{
    public static class StringHelper
    {
        /// <summary>
        /// Удаляет пробелы и знак '+' из строки.
        /// </summary>
        /// <param name="input">Строка с телефоном.</param>
        /// <returns>Строка без пробелов и знака '+'.</returns>
        public static string CleanPhoneNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return input.Replace(" ", "").Replace("+", "");
        }
    }

}
