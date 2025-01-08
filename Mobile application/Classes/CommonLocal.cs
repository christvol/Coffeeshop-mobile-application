using System.Reflection;

namespace Mobile_application.Classes
{
    public class CommonLocal
    {
        public static class ResourceFileLoader
        {
            /// <summary>
            /// Loads a specified resource file from the project and copies it to the application's local directory.
            /// </summary>
            /// <param name="resourceFileName">The name of the resource file including its relative path in the project (e.g., "Resources.Images.dotnet_bot.png").</param>
            /// <param name="outputFileName">The name to save the file as in the application's local directory.</param>
            /// <returns>A task representing the asynchronous operation.</returns>
            public static async Task LoadResourceFileToLocalAsync(string resourceFileName, string outputFileName)
            {
                try
                {
                    // Get the current assembly
                    var assembly = Assembly.GetExecutingAssembly();

                    // Get the resource stream
                    using var stream = assembly.GetManifestResourceStream(resourceFileName);

                    if (stream == null)
                    {
                        Console.WriteLine($"Resource file '{resourceFileName}' not found.");
                        return;
                    }

                    // Define the destination path in the local directory
                    string destinationPath = Path.Combine(FileSystem.AppDataDirectory, outputFileName);

                    // Create the file and copy the contents from the resource stream
                    using var fileStream = File.Create(destinationPath);
                    await stream.CopyToAsync(fileStream);

                    Console.WriteLine($"File '{outputFileName}' has been saved to '{destinationPath}'.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while loading the resource file: {ex.Message}");
                }
            }
        }
        public static class PhoneCodes
        {
            #region Поля
            /// <summary>
            /// Имя папки, где хранятся флаги стран.
            /// </summary>
            public static readonly string FlagsDirName = "countries flags";
            /// <summary>
            /// Имя текстового файла, содержащего список кодов стран.
            /// </summary>
            public static readonly string CountryCodesFileName = "countries list with codes.txt";
            #endregion

            #region Свойства
            /// <summary>
            /// Возвращает полный путь к папке, где хранятся флаги стран.
            /// </summary>
            public static string FlagsDirectoryPath =>
                Path.Combine(FileSystem.AppDataDirectory, FlagsDirName);
            /// <summary>
            /// Возвращает полный путь к текстовому файлу, содержащему список кодов стран.
            /// </summary>
            public static string CountryCodesFilePath =>
                Path.Combine(FileSystem.AppDataDirectory, CountryCodesFileName);
            #endregion

            /// <summary>
            /// Имя папки, где хранятся флаги стран.
            /// </summary>
            public static readonly string flagsDirName = "countries flags";

            /// <summary>
            /// HttpClient для выполнения сетевых запросов.
            /// </summary>
            private static readonly HttpClient httpClient = new HttpClient();

            /// <summary>
            /// Загружает флаг страны по коду и сохраняет в локальную директорию.
            /// </summary>
            /// <param name="countryTicker">Код страны.</param>
            /// <returns>Задача выполнения асинхронной операции.</returns>
            public static async Task GetFlagAsync(string countryTicker, bool isForce = false)
            {
                try
                {
                    // Формируем путь для сохранения
                    string directoryPath = Path.Combine(FileSystem.AppDataDirectory, flagsDirName);
                    Directory.CreateDirectory(directoryPath); // Создаем директорию, если ее нет
                    string filePath = Path.Combine(directoryPath, $"{countryTicker}.png");

                    // Проверяем наличие файла и поведение в зависимости от флага isForce
                    if (File.Exists(filePath) && !isForce)
                    {
                        Console.WriteLine($"Файл для {countryTicker} уже существует: {filePath}. Пропуск загрузки.");
                        return;
                    }

                    // Формируем URL
                    string url = $"https://flagsapi.com/{countryTicker}/flat/64.png";

                    // Выполняем GET-запрос
                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    // Проверяем успешность запроса
                    response.EnsureSuccessStatusCode();

                    // Считываем содержимое ответа как массив байтов
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                    // Сохраняем файл
                    await File.WriteAllBytesAsync(filePath, imageBytes);

                    Console.WriteLine($"Флаг для {countryTicker} сохранен по пути: {filePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка при получении флага: {ex.Message}");
                }
            }

        }
        public static class API
        {
            public static readonly string entryPoint = "http://localhost:5121/api/";
        }
        public static class Strings
        {
            public static class ErrorMessages
            {
                public const string EmptyCode = "Код не может быть пустым.";
                public const string InvalidCode = "Неверный код. Попробуйте еще раз.";
                public const string UserCreationFailed = "Не удалось создать пользователя.";
                public const string UserTypeNotFound = "Тип пользователя не найден.";
                public const string UnknownUserType = "Неизвестный тип пользователя.";
                public const string DataProcessingError = "Произошла ошибка при обработке данных: {0}";
                public const string CodeFetchFailed = "Не удалось получить код: {0}";
            }

            public static class SuccessMessages
            {
                public const string CodeVerified = "Код подтвержден!";
                public const string CodeFetched = "Код подтверждения: {0}";
            }
        }

        public static class DialogTitles
        {
            public const string Information = "Информация";
            public const string Error = "Ошибка";
            public const string Success = "Успех";
        }

        public static class DefaultUserData
        {
            public const int IdUserType = 2; // Тип пользователя: Customer
            public const string FirstName = "Новый";
            public const string LastName = "Пользователь";
            public const string Email = "your@mail.com";
            public const int BirthYearOffset = -18; // Возраст по умолчанию: 18 лет назад
        }

        public static class UserTypes
        {
            public const string Customer = "Customer";
            public const string Employee = "Employee";
            public const string Admin = "Admin";
        }
    }
}