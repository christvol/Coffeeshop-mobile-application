using Common.Classes.DTO;
using Common.Classes.Session;

namespace Mobile_application.Pages
{
    public partial class PageIngredientEdit : CustomContentPage
    {
        #region Поля
        private List<IngredientTypeDTO> _ingredientTypes = new();
        #endregion

        #region Свойства
        public IngredientDTO Ingredient
        {
            get; set;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageIngredientEdit(SessionData sessionData)
            : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.Ingredient = this.SessionData.Data as IngredientDTO ?? new IngredientDTO();

            this.FillFields();
            this.SetFieldAccessibility();
        }
        #endregion

        #region Методы
        private void FillFields()
        {
            this.EntryTitle.Text = this.Ingredient?.Title ?? string.Empty;
            this.EditorDescription.Text = this.Ingredient?.Description ?? string.Empty;
            this.EntryFee.Text = this.Ingredient?.Fee.ToString() ?? "0";
        }

        private void SetFieldAccessibility()
        {
            if (this.SessionData.Mode == WindowMode.Read)
            {
                this.EntryTitle.IsEnabled = false;
                this.EditorDescription.IsEnabled = false;
                this.EntryFee.IsEnabled = false;
                this.pIngredientType.IsEnabled = false;
            }
        }
        #endregion

        #region Обработчики событий
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this._ingredientTypes = await this.ApiClient.GetAllIngredientTypesAsync();
            this.pIngredientType.ConfigurePicker<IngredientTypeDTO>(this._ingredientTypes, "Title", "Id", this.Ingredient?.IngredientType);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (this.Ingredient == null)
                {
                    await this.DisplayAlert("Ошибка", "Данные ингредиента отсутствуют.", "OK");
                    return;
                }

                this.Ingredient.Title = this.EntryTitle.Text ?? string.Empty;
                this.Ingredient.Description = this.EditorDescription.Text ?? string.Empty;
                this.Ingredient.Fee = float.TryParse(this.EntryFee.Text, out float fee) ? fee : 0;
                this.Ingredient.IngredientType = this.pIngredientType.SelectedItem as IngredientTypeDTO ?? this.Ingredient.IngredientType;
                this.Ingredient.IdIngredientType = this.Ingredient.IngredientType == null ? null : this.Ingredient.IngredientType.Id;

                if (this.SessionData.Mode == WindowMode.Create)
                {
                    _ = await this.ApiClient.CreateIngredientAsync(this.Ingredient);
                    await this.DisplayAlert("Успех", "Ингредиент успешно добавлен.", "OK");
                }
                else if (this.SessionData.Mode == WindowMode.Update)
                {
                    _ = await this.ApiClient.UpdateIngredientAsync(this.Ingredient.Id, this.Ingredient);
                    await this.DisplayAlert("Успех", "Ингредиент успешно обновлен.", "OK");
                }

                _ = await this.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось сохранить ингредиент: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            _ = await this.Navigation.PopAsync();
        }
        #endregion
    }
}
