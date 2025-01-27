using Common.Classes.DB;
using Common.Classes.Session;
using Mobile_application.Classes.API;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages;

public partial class PageMainEmployee : CustomContentPage
{
    private readonly ApiClient _apiClient;
    public ObservableCollection<ProductTypes> Categories { get; set; } = new();

    public PageMainEmployee(SessionData sessionData)
    {
        this.InitializeComponent();
        this.SessionData = sessionData;
        this._apiClient = new ApiClient();
        this.BindingContext = this;

        NavigationPage.SetHasBackButton(this, false);
        this.LoadEmployeeData();
        this.BindSessionDataToHeader();
        this.LoadCategoriesAsync();
    }

    private void LoadEmployeeData()
    {
        if (this.SessionData.CurrentUser != null)
        {
            this.Title = $"Dashboard: {this.SessionData.CurrentUser.FirstName}";
        }
    }

    private void BindSessionDataToHeader()
    {
        if (this.FindByName("PageHeader") is Controls.PageHeader header)
        {
            header.SessionData = this.SessionData;
        }
    }

    private async void LoadCategoriesAsync()
    {
        try
        {
            var categories = await this._apiClient.GetAllProductTypesAsync();
            this.Categories.Clear();
            foreach (var category in categories)
            {
                this.Categories.Add(category);
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось загрузить категории: {ex.Message}", "OK");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.LoadCategoriesAsync();
    }


    private async void btnAddCategory_Clicked(object sender, EventArgs e)
    {
        var newSessionData = new SessionData
        {
            CurrentUser = this.SessionData.CurrentUser,
            Mode = WindowMode.Create, // Устанавливаем режим Create
            Data = null // Для новой категории данные не нужны
        };

        // Переход на страницу PageProductTypeEdit
        await this.Navigation.PushAsync(new PageProductTypeEdit(newSessionData));
    }


    private async void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is ProductTypes selectedCategory)
        {
            // Создание нового SessionData с текущим пользователем и категорией
            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData.CurrentUser,
                Data = selectedCategory
            };

            // Переход на страницу PageProducts
            await this.Navigation.PushAsync(new PageProducts(newSessionData));

            // Сбрасываем выбор
            ((CollectionView)sender).SelectedItem = null;
        }
    }


    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ProductTypes category)
        {
            var editSessionData = new SessionData
            {
                CurrentUser = this.SessionData.CurrentUser,
                Mode = WindowMode.Update, // Устанавливаем режим Update
                Data = category
            };

            // Переход на страницу PageProductTypeEdit
            await this.Navigation.PushAsync(new PageProductTypeEdit(editSessionData));
        }
    }


    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ProductTypes category)
        {
            var confirm = await this.DisplayAlert("Delete", $"Вы уверены, что хотите удалить категорию: {category.Title}?", "Yes", "No");
            if (!confirm)
            {
                return;
            }

            try
            {
                // Вызов метода API для удаления
                await this.ApiClient.DeleteProductTypeAsync(category.Id);

                // Удаление категории из локального списка
                _ = this.Categories.Remove(category);

                await this.DisplayAlert("Success", $"Категория {category.Title} успешно удалена.", "OK");
            }
            catch (Exception ex)
            {
                // Обработка ошибок при удалении
                await this.DisplayAlert("Error", $"Не удалось удалить категорию: {ex.Message}", "OK");
            }
        }
    }


}
