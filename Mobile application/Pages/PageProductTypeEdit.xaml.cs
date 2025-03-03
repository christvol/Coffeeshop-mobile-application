using Common.Classes.DB;
using Common.Classes.Session;

namespace Mobile_application.Pages;

public partial class PageProductTypeEdit : CustomContentPage
{
    public ProductTypes Category
    {
        get; private set;
    }
    public WindowMode Mode => this.SessionData.Mode; // Режим окна из SessionData

    public PageProductTypeEdit(SessionData sessionData)
    {
        this.InitializeComponent();
        this.SessionData = sessionData;

        // Проверка корректности данных
        //this.CheckSessionData();

        if (this.Mode == WindowMode.Create)
        {
            // Создаём новую категорию в режиме Create
            this.Category = new ProductTypes
            {
                Title = string.Empty
            };
        }
        else if (this.Mode == WindowMode.Update && sessionData.Data is ProductTypes category)
        {
            // Используем существующую категорию в режиме Update
            this.Category = category;
        }
        else
        {
            throw new InvalidOperationException("Invalid mode or session data for PageProductTypeEdit.");
        }

        // Установка контекста данных для привязки
        this.BindingContext = this;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            if (this.Mode == WindowMode.Create)
            {
                // Вызов API для добавления категории
                ProductTypes? createdCategory = await this.ApiClient.CreateProductTypeAsync(this.Category);
                if (createdCategory != null)
                {
                    await this.DisplayAlert("Success", "Category added successfully!", "OK");
                }
            }
            else if (this.Mode == WindowMode.Update)
            {
                // Вызов API для обновления категории
                _ = await this.ApiClient.UpdateProductTypeAsync(this.Category.Id, this.Category);
                await this.DisplayAlert("Success", "Category updated successfully!", "OK");
            }

            _ = await this.Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Error", $"Failed to save category: {ex.Message}", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        _ = await this.Navigation.PopAsync();
    }
}
