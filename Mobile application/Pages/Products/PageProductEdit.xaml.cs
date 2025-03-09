using Common.Classes.DB;
using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageProductEdit : CustomContentPage
    {
        #region Свойства

        public ProductDTO Product
        {
            get; set;
        }
        public ObservableCollection<IngredientDTO> Ingredients { get; private set; } = new();
        public ObservableCollection<ProductTypes> ProductTypes { get; private set; } = new();
        public List<ProductTypes> ProductTypesList { get; private set; } = new();
        public ProductTypes SelectedProductType
        {
            get; set;
        }
        public WindowMode Mode
        {
            get;
        }

        #endregion

        #region Конструкторы/Деструкторы

        public PageProductEdit(SessionData sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.Product = sessionData.Data as ProductDTO ?? new ProductDTO();
            this.Mode = sessionData.Mode;

            this.BindingContext = this;

            // Настраиваем список ингредиентов
            this.ccvItems.SetEditCommand<IngredientDTO>(this.OnEditIngredientClicked);
            this.ccvItems.SetDeleteCommand<IngredientDTO>(this.OnDeleteIngredientClicked);

            // Загружаем данные
            _ = this.LoadProductTypes();
            this.LoadIngredients();
        }

        #endregion

        #region Методы

        /// <summary>
        /// Загружает список типов продуктов.
        /// </summary>
        private async Task LoadProductTypes()
        {
            try
            {
                this.ProductTypesList = await this.ApiClient.GetAllProductTypesAsync();
                this.ProductTypes.UpdateObservableCollection(this.ProductTypesList);
                this.SelectedProductType = this.ProductTypes.FirstOrDefault(pt => pt.Id == this.Product.IdProductType);

            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось загрузить типы продуктов: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Загружает список ингредиентов, основываясь на `AllowedIngredients`.
        /// </summary>
        private async void LoadIngredients()
        {
            try
            {
                List<AllowedIngredientsDTO> allowedIngredients = await this.ApiClient.GetAllowedIngredientsAsync();
                List<IngredientDTO> ingredients = await this.ApiClient.GetAllIngredientsAsync();

                var filteredIngredients = allowedIngredients
                    .Where(ai => ai.IdProduct == this.Product.Id)
                    .Select(ai => ingredients.FirstOrDefault(i => i.Id == ai.IdIngredient))
                    .Where(i => i != null)
                    .ToList();

                this.Ingredients.UpdateObservableCollection(filteredIngredients);
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось загрузить ингредиенты: {ex.Message}", "OK");
            }
        }

        #endregion

        #region Обработчики событий

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await this.LoadProductTypes();
            this.LoadIngredients();

            // Заполняем Picker данными из загруженного списка типов продуктов
            this.pProductType.ConfigurePicker<ProductTypes>(this.ProductTypesList, "Title", "Id", this.SelectedProductType);

            // Настраиваем CollectionView
            this.ccvItems.SetDisplayedFields("Title", "Description");
            this.ccvItems.SetItems(this.Ingredients);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                string feeText = this.EntryFee.Text?.Replace("₽", "").Trim().Replace(",", ".") ?? "0";

                if (!decimal.TryParse(feeText, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out decimal fee))
                {
                    await this.DisplayAlert("Ошибка", "Некорректный формат цены.", "OK");
                    return;
                }

                this.Product.Title = this.EntryTitle.Text;
                this.Product.Description = this.EditorDescription.Text;
                this.Product.Fee = (float)fee;
                this.Product.IdProductType = this.SelectedProductType?.Id ?? this.Product.IdProductType;

                if (this.Mode == WindowMode.Create)
                {
                    _ = await this.ApiClient.CreateProductAsync(this.Product);
                    await this.DisplayAlert("Успех", "Продукт создан.", "OK");
                }
                else if (this.Mode == WindowMode.Update)
                {
                    _ = await this.ApiClient.UpdateProductAsync(this.Product.Id, this.Product);
                    await this.DisplayAlert("Успех", "Продукт обновлен.", "OK");
                }

                _ = await this.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось сохранить продукт: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            _ = await this.Navigation.PopAsync();
        }

        /// <summary>
        /// Добавляет новый совместимый ингредиент, исключая уже добавленные.
        /// </summary>
        private async void OnAddIngredientClicked(object sender, EventArgs e)
        {
            try
            {
                // Загружаем все доступные ингредиенты
                List<IngredientDTO> availableIngredients = await this.ApiClient.GetAllIngredientsAsync();

                // Загружаем список уже добавленных ингредиентов
                List<AllowedIngredientsDTO> existingAllowedIngredients = await this.ApiClient.GetAllowedIngredientsAsync();

                // Отбираем ингредиенты, которых еще нет в текущем продукте
                var filteredIngredients = availableIngredients
                    .Where(i => !existingAllowedIngredients.Any(ai => ai.IdProduct == this.Product.Id && ai.IdIngredient == i.Id))
                    .ToList();

                if (!filteredIngredients.Any())
                {
                    await this.DisplayAlert("Ошибка", "Все возможные ингредиенты уже добавлены.", "OK");
                    return;
                }

                string selectedIngredient = await this.DisplayActionSheet("Выберите ингредиент", "Отмена", null,
                    filteredIngredients.Select(i => i.Title).ToArray());

                if (selectedIngredient == "Отмена" || selectedIngredient == null)
                {
                    return;
                }

                IngredientDTO ingredient = filteredIngredients.First(i => i.Title == selectedIngredient);

                var allowedIngredient = new AllowedIngredientsDTO
                {
                    IdProduct = this.Product.Id,
                    IdIngredient = ingredient.Id,
                    AllowedNumber = 1
                };

                _ = await this.ApiClient.CreateAllowedIngredientAsync(allowedIngredient);
                this.LoadIngredients();

                await this.DisplayAlert("Успех", "Ингредиент добавлен.", "OK");
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось добавить ингредиент: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Обработчик редактирования ингредиента.
        /// </summary>
        /// <summary>
        /// Обработчик редактирования ингредиента.
        /// </summary>
        private async void OnEditIngredientClicked(IngredientDTO ingredient)
        {
            try
            {
                if (this.SessionData == null || this.SessionData.CurrentUser == null)
                {
                    throw new InvalidOperationException("SessionData or CurrentUser is not set.");
                }

                var editSessionData = new SessionData
                {
                    CurrentUser = this.SessionData.CurrentUser,
                    Mode = WindowMode.Update,
                    Data = ingredient
                };

                await this.Navigation.PushAsync(new PageIngredientEdit(editSessionData));
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось открыть редактирование: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Обработчик удаления ингредиента.
        /// </summary>
        private async void OnDeleteIngredientClicked(IngredientDTO ingredient)
        {
            try
            {
                bool confirm = await this.DisplayAlert("Удаление", $"Вы уверены, что хотите удалить \"{ingredient.Title}\"?", "Да", "Нет");
                if (!confirm)
                {
                    return;
                }

                List<AllowedIngredientsDTO> allowedIngredients = await this.ApiClient.GetAllowedIngredientsAsync();
                AllowedIngredientsDTO? toDelete = allowedIngredients.FirstOrDefault(ai => ai.IdProduct == this.Product.Id && ai.IdIngredient == ingredient.Id);

                if (toDelete != null)
                {
                    await this.ApiClient.DeleteAllowedIngredientAsync(toDelete.Id);
                    this.LoadIngredients();
                    await this.DisplayAlert("Успех", "Ингредиент удалён.", "OK");
                }
                else
                {
                    await this.DisplayAlert("Ошибка", "Ингредиент не найден в допустимых.", "OK");
                }
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось удалить ингредиент: {ex.Message}", "OK");
            }
        }

        #endregion
    }
}
