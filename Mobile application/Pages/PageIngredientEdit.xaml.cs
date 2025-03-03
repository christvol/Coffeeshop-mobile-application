using Common.Classes.DTO;
using Common.Classes.Session;

namespace Mobile_application.Pages
{
    public partial class PageIngredientEdit : CustomContentPage
    {
        public IngredientDTO Ingredient
        {
            get; set;
        }

        public PageIngredientEdit(SessionData sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.Ingredient = sessionData.Data as IngredientDTO ?? new IngredientDTO();

            // Блокируем поля в режиме Read
            if (sessionData.Mode == WindowMode.Read)
            {
                this.EntryTitle.IsEnabled = false;
                this.EditorDescription.IsEnabled = false;
                this.EntryFee.IsEnabled = false;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                // Обновляем ингредиент
                this.Ingredient.Title = this.EntryTitle.Text;
                this.Ingredient.Description = this.EditorDescription.Text;
                this.Ingredient.Fee = float.TryParse(this.EntryFee.Text, out float fee) ? fee : 0;
                if (this.SessionData.Mode == WindowMode.Create)
                {
                    // Добавляем новый ингредиент
                    _ = await this.ApiClient.CreateIngredientAsync(this.Ingredient);
                    await this.DisplayAlert("Успех", "Ингредиент успешно добавлен.", "OK");
                }
                else if (this.SessionData.Mode == WindowMode.Update)
                {
                    // Обновляем ингредиент
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
    }
}