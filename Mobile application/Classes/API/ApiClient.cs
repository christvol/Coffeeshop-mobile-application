namespace Mobile_application.Classes.API
{
    using global::Common.Classes.DB;
    using global::Common.Classes.DTO;
    using REST_API_SERVER.DTOs;
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

                _ = response.EnsureSuccessStatusCode();

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

                _ = response.EnsureSuccessStatusCode();

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

        #region Users

        /// <summary>
        /// Создание нового пользователя.
        /// </summary>
        /// <param name="userDto">Объект <see cref="UserRequestDto"/>, содержащий данные для создания пользователя.</param>
        /// <returns>Объект <see cref="Users"/> с данными созданного пользователя или null, если запрос завершился неудачно.</returns>
        /// <exception cref="HttpRequestException">Возникает при ошибке запроса.</exception>
        public async Task<Users?> CreateUserAsync(UserRequestDto userDto)
        {
            try
            {
                var response = await this._httpClient.PostAsJsonAsync("Users", userDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Users>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании пользователя: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение пользователя по ID.
        /// </summary>
        /// <param name="id">ID пользователя.</param>
        /// <returns>Объект <see cref="Users"/> или null, если не найден.</returns>
        /// <exception cref="HttpRequestException">Возникает при ошибке запроса.</exception>
        public async Task<Users?> GetUserByIdAsync(int id)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"Users/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Users>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении пользователя по ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение пользователя по номеру телефона.
        /// </summary>
        /// <param name="phoneNumber">Номер телефона пользователя.</param>
        /// <returns>Объект <see cref="Users"/> или null, если не найден.</returns>
        /// <exception cref="HttpRequestException">Возникает при ошибке запроса.</exception>
        public async Task<Users?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"Users/phone/{phoneNumber}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Users>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении пользователя по номеру телефона: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление данных пользователя.
        /// </summary>
        /// <param name="id">ID пользователя.</param>
        /// <param name="userDto">Данные пользователя для обновления.</param>
        /// <returns>True, если обновление прошло успешно; иначе false.</returns>
        public async Task<bool> UpdateUserAsync(int id, UserRequestDto userDto)
        {
            try
            {
                var response = await this._httpClient.PutAsJsonAsync($"Users/{id}", userDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении пользователя: {ex.Message}");
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

                _ = response.EnsureSuccessStatusCode();

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

        #region Products

        /// <summary>
        /// Получение списка всех продуктов.
        /// </summary>
        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            try
            {
                var response = await this._httpClient.GetAsync("Products");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProductDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<ProductDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении списка продуктов: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение продуктов по ID типа.
        /// </summary>
        public async Task<List<ProductDTO>> GetProductsByTypeAsync(int typeId)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"Products/ByType/{typeId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ProductDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<ProductDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении продуктов по типу: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение продукта по ID.
        /// </summary>
        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"Products/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении продукта по ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Создание нового продукта.
        /// </summary>
        public async Task<ProductDTO?> CreateProductAsync(ProductDTO productDto)
        {
            try
            {
                var response = await this._httpClient.PostAsJsonAsync("Products", productDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ProductDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании продукта: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление продукта.
        /// </summary>
        public async Task<bool> UpdateProductAsync(int id, ProductDTO productDto)
        {
            try
            {
                var response = await this._httpClient.PutAsJsonAsync($"Products/{id}", productDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении продукта: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаление продукта.
        /// </summary>
        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var response = await this._httpClient.DeleteAsync($"Products/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении продукта: {ex.Message}");
            }
        }

        #endregion

        public async Task<RegistrationResponse?> GetCodeAsync(string phoneNumber)
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
