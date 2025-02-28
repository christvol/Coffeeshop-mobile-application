using Common.Classes.DB;
using Common.Classes.Session;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages;

public partial class PageProductTypes : CustomContentPage
{

    #region Поля

    #endregion

    #region Свойства
    public ObservableCollection<ProductTypes> Categories
    {
        get; set;
    }
    #endregion

    #region Методы
    private void LoadUserData()
    {
        if (this.SessionData != null && this.SessionData.CurrentUser != null)
        {
            this.Title = $"Dashboard: {this.SessionData.CurrentUser.FirstName}";
        }
    }

    private void BindSessionDataToHeader()
    {
        if (this.FindByName("PageHeader") is Controls.PageHeader header && this.SessionData != null)
        {
            header.SessionData = this.SessionData;
        }
    }

    private async void LoadCategoriesAsync()
    {
        try
        {
            List<ProductTypes> categories = await this.ApiClient.GetAllProductTypesAsync();
            this.Categories.Clear();
            foreach (ProductTypes category in categories)
            {
                this.Categories.Add(category);
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось загрузить категории: {ex.Message}", "OK");
        }
    }
    #endregion

    #region Конструкторы/Деструкторы
    public PageProductTypes(SessionData sessionData) : base(sessionData)
    {
        this.InitializeComponent();
        this.Categories = new ObservableCollection<ProductTypes>();
        this.BindingContext = this;
        this.LoadUserData();
        this.BindSessionDataToHeader();
        this.LoadCategoriesAsync();
    }
    #endregion

    #region Операторы

    #endregion

    #region Обработчики событий
    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.LoadCategoriesAsync();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        var newSessionData = new SessionData
        {
            CurrentUser = this.SessionData?.CurrentUser,
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
                CurrentUser = this.SessionData?.CurrentUser,
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
                CurrentUser = this.SessionData?.CurrentUser,
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
            bool confirm = await this.DisplayAlert("Delete", $"Вы уверены, что хотите удалить категорию: {category.Title}?", "Yes", "No");
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
    #endregion

}
