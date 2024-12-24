namespace Мобильное_приложение.Classes.API
{
    using DB.Classes.DB;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Конструктор для инициализации клиента API.
        /// </summary>
        /// <param name="baseAddress">Базовый адрес API.</param>
        public ApiClient(string baseAddress = "http://localhost:5121/api/")
        {
            this._httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            this._httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// Получение списка стран.
        /// </summary>
        public async Task<List<CountryCodes>> GetCountryCodesAsync()
        {
            try
            {
                var response = await this._httpClient.GetAsync("CountryCodes");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<CountryCodes>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<CountryCodes>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении данных: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение данных о стране по ID.
        /// </summary>
        public async Task<CountryCodes?> GetCountryByIdAsync(int id)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"CountryCodes/id/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<CountryCodes>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении страны по ID: {ex.Message}");
            }
        }

        public async Task<RegistrationResponse?> RegisterAsync(string phoneNumber)
        {
            try
            {
                var response = await this._httpClient.PostAsJsonAsync("Auth/register", new
                {
                    PhoneNumber = phoneNumber
                });

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RegistrationResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при отправке кода: {ex.Message}");
            }
        }

    }

}
