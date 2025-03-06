using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageIngredientTypes : CustomContentPage
    {
        #region Свойства

        public ObservableCollection<IngredientTypeDTO> IngredientTypes { get; set; } = new();

        #endregion

        #region Конструкторы/Деструкторы

        public PageIngredientTypes(SessionData? sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            // Устанавливаем обработчики событий
            this.ccvItems.SetEditCommand<IngredientTypeDTO>(this.OnEditIngredientType);
            this.ccvItems.SetDeleteCommand<IngredientTypeDTO>(this.OnDeleteIngredientType);
        }

        #endregion

        #region Методы

        /// <summary>
        /// Обновляет коллекцию типов ингредиентов.
        /// </summary>
        private async void UpdateItemsCollection()
        {
            this.IngredientTypes.UpdateObservableCollection(await this.ApiClient.GetAllIngredientTypesAsync());
        }

        #endregion

        #region Обработчики событий

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.UpdateItemsCollection();

            // Настраиваем CollectionView
            this.ccvItems.SetDisplayedFields("Title");
            this.ccvItems.SetItems(this.IngredientTypes);
        }

        /// <summary>
        /// Обработчик нажатия кнопки редактирования типа ингредиента.
        /// </summary>
        private async void OnEditIngredientType(IngredientTypeDTO ingredientType)
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
                    Data = ingredientType
                };
                await this.Navigation.PushAsync(new PageIngredientTypeEdit(editSessionData));
            }
            catch (Exception ex)
            {
                _ = this.ShowError(ex);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки удаления типа ингредиента.
        /// </summary>
        private async void OnDeleteIngredientType(IngredientTypeDTO ingredientType)
        {
            bool confirm = await this.DisplayAlert("Подтверждение", $"Удалить тип ингредиента \"{ingredientType.Title}\"?", "Да", "Нет");
            if (!confirm)
            {
                return;
            }

            try
            {
                await this.ApiClient.DeleteIngredientTypeAsync(ingredientType.Id);
                await this.DisplayAlert("Успех", "Тип ингредиента удалён.", "OK");
                this.UpdateItemsCollection();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось удалить тип ингредиента: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки добавления нового типа ингредиента.
        /// </summary>
        private async void OnAddIngredientTypeClicked(object sender, EventArgs e)
        {
            try
            {
                if (this.SessionData == null || this.SessionData.CurrentUser == null)
                {
                    throw new InvalidOperationException(CommonLocal.Strings.ErrorMessages.SessionDataUserNotSet);
                }
                var newSessionData = new SessionData
                {
                    CurrentUser = this.SessionData.CurrentUser,
                    Mode = WindowMode.Create
                };
                await this.Navigation.PushAsync(new PageIngredientTypeEdit(newSessionData));
            }
            catch (Exception ex)
            {
                _ = this.ShowError(ex);
            }
        }


        #endregion
    }
}
