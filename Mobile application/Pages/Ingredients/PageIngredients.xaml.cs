using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageIngredients : CustomContentPage
    {
        #region Свойства

        public ObservableCollection<IngredientDTO> Items { get; set; } = new();
        private OrderDTO? _currentOrder;
        private ProductDTO? _currentProduct;
        private IngredientTypeDTO? _selectedType;

        #endregion

        #region Конструкторы/Деструкторы

        public PageIngredients(SessionData? sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            // Устанавливаем обработчики событий
            this.ccvItems.SetEditCommand<IngredientDTO>(this.OnEditIngredient);
            this.ccvItems.SetDeleteCommand<IngredientDTO>(this.OnDeleteIngredient);
            this.ccvItems.SetItemSelectedCommand<IngredientDTO>(this.OnIngredientSelected);

            // Проверяем, получили ли массив данных с заказом и категорией
            if (this.SessionData?.Data is { } dataObject &&
                dataObject.GetType().GetProperty("Order") != null &&
                dataObject.GetType().GetProperty("Product") != null &&
                dataObject.GetType().GetProperty("SelectedType") != null)
            {
                this._currentOrder = dataObject.GetType().GetProperty("Order")?.GetValue(dataObject) as OrderDTO;
                this._currentProduct = dataObject.GetType().GetProperty("Product")?.GetValue(dataObject) as ProductDTO;
                this._selectedType = dataObject.GetType().GetProperty("SelectedType")?.GetValue(dataObject) as IngredientTypeDTO;
            }
        }

        #endregion

        #region Методы

        /// <summary>
        /// Обновляет коллекцию ингредиентов, фильтруя по категории.
        /// </summary>
        private async void UpdateItemsCollection()
        {
            // Загружаем все ингредиенты и фильтруем по выбранной категории
            List<IngredientDTO> allIngredients = await this.ApiClient.GetAllIngredientsAsync();
            foreach (IngredientDTO item in allIngredients)
            {
                item.IdIngredientType = item.IngredientType == null ? null : item.IngredientType.Id;
            }
            List<IngredientDTO> filteredIngredients = this._selectedType != null
                ? allIngredients.Where(i => i.IdIngredientType == this._selectedType.Id).OrderBy(i => i.Title).ToList()
                : allIngredients.OrderBy(i => i.Title).ToList();

            this.Items.UpdateObservableCollection(filteredIngredients);
        }

        #endregion

        #region Обработчики событий

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.UpdateItemsCollection();

            // Настраиваем CollectionView
            this.ccvItems.SetDisplayedFields("Title", "Description", "Fee");
            this.ccvItems.SetItems(this.Items);

            bool isAdmin = await this.IsUserAdminAsync(this.SessionData.CurrentUser.Id);
            this.ccvItems.IsListItemEditButtonsVisible = isAdmin;
            this.btnAdd.IsVisible = isAdmin;
        }

        /// <summary>
        /// Обработчик выбора ингредиента.
        /// </summary>
        private async void OnIngredientSelected(IngredientDTO selectedIngredient)
        {
            try
            {
                if (this._currentOrder == null || this._currentProduct == null)
                {
                    await this.DisplayAlert("Ошибка", "Заказ или продукт не найден", "OK");
                    return;
                }

                // Добавляем ингредиент в заказ
                var orderIngredient = new OrderItemIngredientDTO
                {
                    IdOrderProduct = this._currentProduct.Id,
                    IdIngredient = selectedIngredient.Id,
                    Amount = 1 // По умолчанию 1
                };

                bool success = await this.ApiClient.AddIngredientToOrderAsync(this._currentOrder.Id, this._currentProduct.Id, orderIngredient);
                if (!success)
                {
                    await this.DisplayAlert("Ошибка", "Не удалось добавить ингредиент", "OK");
                    return;
                }

                await this.DisplayAlert("Успех", "Ингредиент добавлен в заказ", "OK");

                // Возвращаемся в `PageProductCustomer`
                var newSessionData = new SessionData
                {
                    CurrentUser = this.SessionData?.CurrentUser,
                    Data = new { Order = this._currentOrder, Product = this._currentProduct }
                };

                await this.Navigation.PushAsync(new PageProductCustomer(newSessionData));
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось обработать ингредиент: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки редактирования ингредиента.
        /// </summary>
        private async void OnEditIngredient(IngredientDTO ingredient)
        {
            try
            {
                if (this.SessionData == null || this.SessionData.CurrentUser == null)
                {
                    throw new InvalidOperationException(CommonLocal.Strings.ErrorMessages.SessionDataUserNotSet);
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
                _ = this.ShowError(ex);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки удаления ингредиента.
        /// </summary>
        private async void OnDeleteIngredient(IngredientDTO ingredient)
        {
            bool confirm = await this.DisplayAlert("Подтверждение", $"Удалить ингредиент \"{ingredient.Title}\"?", "Да", "Нет");
            if (!confirm)
            {
                return;
            }

            try
            {
                await this.ApiClient.DeleteIngredientAsync(ingredient.Id);
                await this.DisplayAlert("Успех", "Ингредиент удалён.", "OK");
                this.UpdateItemsCollection();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось удалить ингредиент: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки добавления нового ингредиента.
        /// </summary>
        private async void OnAddIngredientClicked(object sender, EventArgs e)
        {
            try
            {
                if (this.SessionData == null || this.SessionData.CurrentUser == null)
                {
                    throw new InvalidOperationException(CommonLocal.Strings.ErrorMessages.SessionDataUserNotSet);
                }
                if (this._currentOrder != null)
                {
                    throw new InvalidOperationException("No order selected");
                }
                var newSessionData = new SessionData
                {
                    CurrentUser = this.SessionData.CurrentUser,
                    Mode = WindowMode.Create
                };
                await this.Navigation.PushAsync(new PageIngredientEdit(newSessionData));
            }
            catch (Exception ex)
            {
                _ = this.ShowError(ex);
            }
        }

        #endregion
    }
}
