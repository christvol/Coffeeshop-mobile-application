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
        private readonly HttpClient httpClient;

        /// <summary>
        /// Конструктор для инициализации клиента API.
        /// </summary>
        /// <param name="baseAddress">Базовый адрес API.</param>
        public ApiClient(string baseAddress = "http://localhost:5121/api/")
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            this.httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        #region CountryCodes

        /// <summary>
        /// Получение списка стран.
        /// </summary>
        public async Task<List<CountryCodes>> GetCountryCodesAsync()
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync("CountryCodes");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();

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
                HttpResponseMessage response = await this.httpClient.GetAsync($"CountryCodes/id/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

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
                HttpResponseMessage response = await this.httpClient.GetAsync("ProductTypes");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.GetAsync($"ProductTypes/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("ProductTypes", productType);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.PutAsJsonAsync($"ProductTypes/{id}", productType);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.DeleteAsync($"ProductTypes/{id}");

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

        public async Task<List<Users>> GetAllUsersAsync()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync("Users");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Ошибка при получении пользователей: {response.StatusCode}");
            }

            return await response.Content.ReadFromJsonAsync<List<Users>>() ?? new List<Users>();
        }


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
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("Users", userDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.GetAsync($"Users/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.GetAsync($"Users/phone/{phoneNumber}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.PutAsJsonAsync($"Users/{id}", userDto);

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
        /// Получает список всех типов пользователей.
        /// </summary>
        public async Task<List<Common.Classes.DB.UserTypes>> GetAllUserTypesAsync()
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync("UserTypes");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                return await response.Content.ReadFromJsonAsync<List<Common.Classes.DB.UserTypes>>() ?? new List<Common.Classes.DB.UserTypes>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении списка типов пользователей: {ex.Message}");
            }
        }


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
                HttpResponseMessage response = await this.httpClient.GetAsync($"Users/{id}/UserType");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.GetAsync("Products");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.GetAsync($"Products/ByType/{typeId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.GetAsync($"Products/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("Products", productDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.PutAsJsonAsync($"Products/{id}", productDto);

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
                HttpResponseMessage response = await this.httpClient.DeleteAsync($"Products/{id}");

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
        public async Task<List<IngredientDTO>> GetAllIngredientsAsync()
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync("Ingredients");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.GetAsync($"Ingredients/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("Ingredients", ingredientDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.PutAsJsonAsync($"Ingredients/{id}", ingredientDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.DeleteAsync($"Ingredients/{id}");

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
                HttpResponseMessage response = await this.httpClient.GetAsync($"Ingredients/{id}/IngredientType");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
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

        #region IngredientTypes
        public async Task<List<IngredientTypeDTO>> GetAllIngredientTypesAsync()
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync("IngredientTypes");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<IngredientTypeDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<IngredientTypeDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении типов ингредиентов: {ex.Message}");
            }
        }
        public async Task<IngredientTypeDTO?> GetIngredientTypeByIdAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync($"IngredientTypes/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IngredientTypeDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении типа ингредиента по ID: {ex.Message}");
            }
        }
        public async Task<IngredientTypeDTO?> CreateIngredientTypeAsync(IngredientTypeDTO ingredientTypeDto)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("IngredientTypes", ingredientTypeDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IngredientTypeDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании типа ингредиента: {ex.Message}");
            }
        }
        public async Task<IngredientTypeDTO?> UpdateIngredientTypeAsync(int id, IngredientTypeDTO ingredientTypeDto)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.PutAsJsonAsync($"IngredientTypes/{id}", ingredientTypeDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IngredientTypeDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении типа ингредиента: {ex.Message}");
            }
        }
        public async Task DeleteIngredientTypeAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.DeleteAsync($"IngredientTypes/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении типа ингредиента: {ex.Message}");
            }
        }

        #endregion

        #region AllowedIngredients

        /// <summary>
        /// Получение всех разрешённых ингредиентов для конкретного продукта.
        /// </summary>
        public async Task<List<AllowedIngredientsDTO>> GetAllowedIngredientsByProductIdAsync(int productId)
        {
            try
            {
                List<AllowedIngredientsDTO> all = await this.GetAllowedIngredientsAsync();
                return all.Where(ai => ai.IdProduct == productId).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении разрешённых ингредиентов по ProductId {productId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение всех разрешённых ингредиентов.
        /// </summary>
        public async Task<List<AllowedIngredientsDTO>> GetAllowedIngredientsAsync()
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync("AllowedIngredients");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.GetAsync($"AllowedIngredients/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("AllowedIngredients", allowedIngredientsDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.PutAsJsonAsync($"AllowedIngredients/{id}", allowedIngredientsDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.DeleteAsync($"AllowedIngredients/{id}");

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
                HttpResponseMessage response = await this.httpClient.GetAsync("IngredientsView");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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
                HttpResponseMessage response = await this.httpClient.GetAsync($"IngredientsView/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
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

        #region OrderDetails

        /// <summary>
        /// Получение всех заказов с деталями.
        /// </summary>
        public async Task<List<OrderDetailsView>> GetAllOrderDetailsAsync()
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync("Orders/details");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                return await response.Content.ReadFromJsonAsync<List<OrderDetailsView>>() ?? new List<OrderDetailsView>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении всех деталей заказов: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение деталей конкретного заказа.
        /// </summary>
        public async Task<List<OrderDetailsView>> GetOrderDetailsByIdAsync(int orderId)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync($"Orders/details/{orderId}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new List<OrderDetailsView>();
                }

                _ = response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<OrderDetailsView>>() ?? new List<OrderDetailsView>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении деталей заказа ID {orderId}: {ex.Message}");
            }
        }

        #endregion

        #region Orders

        /// <summary>
        /// Добавляет ингредиент в продукт заказа.
        /// </summary>
        public async Task<bool> AddIngredientToOrderAsync(int orderId, int productId, OrderItemIngredientDTO ingredientDto)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync(
                    $"Orders/{orderId}/product/{productId}/add-ingredient",
                    ingredientDto
                );

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при добавлении ингредиента в заказ ID {orderId}, продукт ID {productId}: {ex.Message}");
            }
        }


        /// <summary>
        /// Получение списка всех заказов.
        /// </summary>
        public async Task<List<OrderDTO>> GetOrdersAsync()
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync("Orders");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<OrderDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении списка заказов: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение заказа по ID.
        /// </summary>
        public async Task<OrderDTO?> GetOrderByIdAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync($"Orders/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении заказа по ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Создание нового заказа.
        /// </summary>
        public async Task<OrderDTO?> CreateOrderAsync(OrderDTO orderDto)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("Orders", orderDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании заказа: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление статуса заказа.
        /// </summary>
        public async Task<bool> UpdateOrderStatusAsync(int id, int newStatus)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.PutAsJsonAsync($"Orders/{id}/status", newStatus);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении статуса заказа: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление заказа целиком.
        /// </summary>
        public async Task<bool> UpdateOrderAsync(int id, OrderDTO orderDto)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.PutAsJsonAsync($"Orders/{id}", orderDto);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении заказа: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаление заказа по ID.
        /// </summary>
        public async Task DeleteOrderAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.DeleteAsync($"Orders/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении заказа: {ex.Message}");
            }
        }

        /// <summary>
        /// Получает список заказов клиента по его ID.
        /// </summary>
        public async Task<List<OrderDTO>> GetOrdersByCustomerIdAsync(int customerId)
        {
            try
            {
                List<OrderDTO> orders = await this.httpClient.GetFromJsonAsync<List<OrderDTO>>("Orders");

                return orders?.Where(o => o.IdCustomer == customerId).ToList() ?? new List<OrderDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении заказов клиента ID {customerId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Добавляет продукт в существующий заказ.
        /// </summary>
        public async Task<bool> AddProductToOrderAsync(int orderId, OrderProductDTO orderProductDto)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync($"Orders/{orderId}/add-product", orderProductDto);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при добавлении продукта в заказ ID {orderId}: {ex.Message}");
            }
        }

        #endregion

        #region OrderStatuses

        /// <summary>
        /// Получение списка всех статусов заказов.
        /// </summary>
        public async Task<List<OrderStatusDTO>> GetAllOrderStatusesAsync()
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync("OrderStatuses");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderStatusDTO>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<OrderStatusDTO>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении списка статусов заказов: {ex.Message}");
            }
        }

        /// <summary>
        /// Получение статуса заказа по ID.
        /// </summary>
        public async Task<OrderStatusDTO?> GetOrderStatusByIdAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.GetAsync($"OrderStatuses/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                _ = response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderStatusDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении статуса заказа по ID: {ex.Message}");
            }
        }

        /// <summary>
        /// Получает статус заказа по названию.
        /// </summary>
        public async Task<OrderStatusDTO?> GetOrderStatusByTitleAsync(string title)
        {
            try
            {
                List<OrderStatusDTO> statuses = await this.httpClient.GetFromJsonAsync<List<OrderStatusDTO>>("OrderStatuses");

                return statuses?.FirstOrDefault(s => s.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении статуса заказа по названию '{title}': {ex.Message}");
            }
        }

        #endregion

        #region Images
        public async Task<byte[]?> GetImageBytesAsync(int imageId)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"Images/{imageId}/data");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }

        #endregion

        public async Task<RegistrationResponse?> GetCodeAsync(string phoneNumber)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("Auth/register", new
                {
                    PhoneNumber = phoneNumber
                });

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Ошибка: {response.StatusCode}, {response.ReasonPhrase}");
                }

                string json = await response.Content.ReadAsStringAsync();
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

        /// <summary>
        /// Удаляет ингредиент из продукта в заказе.
        /// </summary>
        public async Task<bool> RemoveIngredientFromOrderAsync(int orderId, int productId, int ingredientId)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.DeleteAsync(
                    $"Orders/{orderId}/product/{productId}/ingredient/{ingredientId}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении ингредиента ID {ingredientId} из заказа ID {orderId}, продукт ID {productId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Удаляет продукт из заказа.
        /// </summary>
        public async Task<bool> DeleteProductFromOrderAsync(int orderId, int productId)
        {
            try
            {
                HttpResponseMessage response = await this.httpClient.DeleteAsync($"Orders/{orderId}/product/{productId}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при удалении продукта ID {productId} из заказа {orderId}: {ex.Message}");
            }
        }

        public async Task<bool> SetOrderPaymentStatusAsync(int orderId, int paymentStatusId)
        {
            HttpResponseMessage response = await this.httpClient.PutAsJsonAsync($"Orders/{orderId}/payment-status", paymentStatusId);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UploadProductImageAsync(int productId, Stream imageStream, string fileName)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(imageStream), "file", fileName);

            HttpResponseMessage response = await this.httpClient.PostAsync($"Products/{productId}/upload-image", content);
            return response.IsSuccessStatusCode;
        }




    }
}
