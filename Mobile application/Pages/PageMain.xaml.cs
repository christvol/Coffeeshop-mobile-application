using Common.Classes.DB;
using Mobile_application.Classes.API;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages;

public partial class PageMain : CustomContentPage
{
    public ObservableCollection<ProductTypes> ProductCategories { get; private set; } = new();

    public Command<ProductTypes> NavigateToCategoryCommand
    {
        get;
    }

    public PageMain()
    {
        this.InitializeComponent();
        NavigationPage.SetHasBackButton(this, false);

        this.NavigateToCategoryCommand = new Command<ProductTypes>(this.OnCategorySelected);
        this.BindingContext = this;

        this.LoadCategoriesAsync();
    }

    private async Task LoadCategoriesAsync()
    {
        try
        {
            var categories = await ApiClient.GetAllProductTypesAsync();

            this.ProductCategories.Clear();
            foreach (var category in categories)
            {
                this.ProductCategories.Add(category);
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Error", $"Failed to load product categories: {ex.Message}", "OK");
        }
    }

    private async void OnCategorySelected(ProductTypes category)
    {
        //if (category == null)
        //    return;
        //await this.Navigation.PushAsync(new PageCategoryDetails(category));
    }
}
