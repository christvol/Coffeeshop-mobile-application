﻿namespace Mobile_application.Classes.API
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
        /// <param name="userDto">Объект <see cref="UserDTO"/>, содержащий данные для создания пользователя.</param>
        /// <returns>Объект <see cref="Users"/> с данными созданного пользователя или null, если запрос завершился неудачно.</returns>
        /// <exception cref="HttpRequestException">Возникает при ошибке запроса.</exception>
        public async Task<Users?> CreateUserAsync(UserDTO userDto)
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
        public async Task<bool> UpdateUserAsync(int id, UserDTO userDto)
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
        /// <returns>Объект <see cref="UserTypeDTO"/> или null, если не найден.</returns>
        /// <exception cref="HttpRequestException">Возникает при ошибке запроса.</exception>
        public async Task<UserTypeDTO?> GetUserTypeByUserIdAsync(int id)
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
                return JsonSerializer.Deserialize<UserTypeDTO>(json, new JsonSerializerOptions
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

        #region Ingredients

        /// <summary>
        /// Получение всех ингредиентов.
        /// </summary>
        public async Task<List<IngredientDTO>> GetIngredientsAsync()
        {
            try
            {
                var response = await this._httpClient.GetAsync("Ingredients");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<IngredientDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<IngredientDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении ингредиентов: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение ингредиента по ID.
        /// </summary>
        public async Task<IngredientDTO?> GetIngredientByIdAsync(int id)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"Ingredients/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IngredientDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении ингредиента по ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Создание нового ингредиента.
        /// </summary>
        public async Task<IngredientDTO?> CreateIngredientAsync(IngredientDTO ingredientDto)
        {
            try
            {
                var response = await this._httpClient.PostAsJsonAsync("Ingredients", ingredientDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IngredientDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании ингредиента: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление ингредиента по ID.
        /// </summary>
        public async Task<IngredientDTO?> UpdateIngredientAsync(int id, IngredientDTO ingredientDto)
        {
            try
            {
                var response = await this._httpClient.PutAsJsonAsync($"Ingredients/{id}", ingredientDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IngredientDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении ингредиента: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаление ингредиента по ID.
        /// </summary>
        public async Task DeleteIngredientAsync(int id)
        {
            try
            {
                var response = await this._httpClient.DeleteAsync($"Ingredients/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении ингредиента: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение типа ингредиента по ID.
        /// </summary>
        public async Task<IngredientTypeDTO?> GetIngredientTypeAsync(int id)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"Ingredients/{id}/IngredientType");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IngredientTypeDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении типа ингредиента: {ex.Message}");
            }
        }

        #endregion

        #region AllowedIngredients

        /// <summary>
        /// Получение всех разрешённых ингредиентов.
        /// </summary>
        public async Task<List<AllowedIngredientsDTO>> GetAllowedIngredientsAsync()
        {
            try
            {
                var response = await this._httpClient.GetAsync("AllowedIngredients");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<AllowedIngredientsDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<AllowedIngredientsDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении разрешённых ингредиентов: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение разрешённого ингредиента по ID.
        /// </summary>
        public async Task<AllowedIngredientsDTO?> GetAllowedIngredientByIdAsync(int id)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"AllowedIngredients/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AllowedIngredientsDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении разрешённого ингредиента по ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Создание нового разрешённого ингредиента.
        /// </summary>
        public async Task<AllowedIngredientsDTO?> CreateAllowedIngredientAsync(AllowedIngredientsDTO allowedIngredientsDto)
        {
            try
            {
                var response = await this._httpClient.PostAsJsonAsync("AllowedIngredients", allowedIngredientsDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AllowedIngredientsDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании разрешённого ингредиента: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление разрешённого ингредиента по ID.
        /// </summary>
        public async Task<AllowedIngredientsDTO?> UpdateAllowedIngredientAsync(int id, AllowedIngredientsDTO allowedIngredientsDto)
        {
            try
            {
                var response = await this._httpClient.PutAsJsonAsync($"AllowedIngredients/{id}", allowedIngredientsDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<AllowedIngredientsDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении разрешённого ингредиента: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаление разрешённого ингредиента по ID.
        /// </summary>
        public async Task DeleteAllowedIngredientAsync(int id)
        {
            try
            {
                var response = await this._httpClient.DeleteAsync($"AllowedIngredients/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении разрешённого ингредиента: {ex.Message}");
            }
        }

        #endregion

        #region IngredientsView

        /// <summary>
        /// Получение всех элементов из представления IngredientsView.
        /// </summary>
        public async Task<List<IngredientsView>> GetIngredientsViewAsync()
        {
            try
            {
                var response = await this._httpClient.GetAsync("IngredientsView");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<IngredientsView>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<IngredientsView>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении элементов из IngredientsView: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение элемента из представления IngredientsView по ID.
        /// </summary>
        public async Task<IngredientsView?> GetIngredientsViewByIdAsync(int id)
        {
            try
            {
                var response = await this._httpClient.GetAsync($"IngredientsView/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IngredientsView>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении элемента из IngredientsView по ID: {ex.Message}");
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
