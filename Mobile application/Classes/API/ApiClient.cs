namespace Mobile_application.Classes.API
{
    using global::Common.Classes.DB;
    using global::Common.Classes.DTO;
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

        #region CountryCodes

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

        #endregion

        #region ProductTypes

        /// <summary>
        /// Получение всех типов продуктов.
        /// </summary>
        public async Task<List<ProductTypes>> GetAllProductTypesAsync()
        {
            try
            {
                var response = await this._httpClient.GetAsync("ProductTypes");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProductTypes>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<ProductTypes>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении типов продуктов: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение типа продукта по ID.
        /// </summary>
        public async Task<ProductTypes?> GetProductTypeByIdAsync(int id)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"ProductTypes/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductTypes>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении типа продукта по ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Создание нового типа продукта.
        /// </summary>
        public async Task<ProductTypes?> CreateProductTypeAsync(ProductTypes productType)
        {
            try
            {
                var response = await this._httpClient.PostAsJsonAsync("ProductTypes", productType);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductTypes>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании типа продукта: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление типа продукта по ID.
        /// </summary>
        public async Task<ProductTypes?> UpdateProductTypeAsync(int id, ProductTypes productType)
        {
            try
            {
                var response = await this._httpClient.PutAsJsonAsync($"ProductTypes/{id}", productType);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductTypes>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении типа продукта: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаление типа продукта по ID.
        /// </summary>
        public async Task DeleteProductTypeAsync(int id)
        {
            try
            {
                var response = await this._httpClient.DeleteAsync($"ProductTypes/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении типа продукта: {ex.Message}");
            }
        }

        #endregion

        #region UserTypes

        /// <summary>
        /// Получение типа пользователя по ID.
        /// </summary>
        /// <param name="id">ID пользователя.</param>
        /// <returns>Объект <see cref="UserTypeDto"/> или null, если не найден.</returns>
        /// <exception cref="HttpRequestException">Возникает при ошибке запроса.</exception>
        public async Task<UserTypeDto?> GetUserTypeByUserIdAsync(int id)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"Users/{id}/UserType");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserTypeDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении типа пользователя по ID: {ex.Message}");
            }
        }

        #endregion


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
