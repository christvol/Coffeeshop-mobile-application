using Common.Classes.DTO;
using Common.Classes.Session;

namespace Mobile_application.Pages
{
    public partial class PageIngredientTypeEdit : CustomContentPage
    {
        #region Свойства
        public IngredientTypeDTO IngredientType
        {
            get; set;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageIngredientTypeEdit(SessionData? sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.IngredientType = this.SessionData.Data as IngredientTypeDTO ?? new IngredientTypeDTO();
            this.FillFields();
            this.SetFieldAccessibility();
        }
        #endregion

        #region Методы
        private void FillFields()
        {
            this.EntryTitle.Text = this.IngredientType?.Title ?? string.Empty;
        }

        private void SetFieldAccessibility()
        {
            if (this.SessionData.Mode == WindowMode.Read)
            {
                this.EntryTitle.IsEnabled = false;
            }
        }
        #endregion

        #region Обработчики событий
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (this.IngredientType == null)
                {
                    await this.DisplayAlert("Ошибка", "Данные типа ингредиента отсутствуют.", "OK");
                    return;
                }

                this.IngredientType.Title = this.EntryTitle.Text ?? string.Empty;

                if (this.SessionData.Mode == WindowMode.Create)
                {
                    _ = await this.ApiClient.CreateIngredientTypeAsync(this.IngredientType);
                    await this.DisplayAlert("Успех", "Тип ингредиента успешно добавлен.", "OK");
                }
                else if (this.SessionData.Mode == WindowMode.Update)
                {
                    _ = await this.ApiClient.UpdateIngredientTypeAsync(this.IngredientType.Id, this.IngredientType);
                    await this.DisplayAlert("Успех", "Тип ингредиента успешно обновлен.", "OK");
                }

                _ = await this.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось сохранить тип ингредиента: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            _ = await this.Navigation.PopAsync();
        }
        #endregion
    }
}
