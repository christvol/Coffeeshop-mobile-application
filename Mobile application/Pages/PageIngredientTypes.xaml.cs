using Common.Classes.Session;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages;

public partial class PageIngredientTypes : CustomContentPage
{
    public ObservableCollection<object> IngredientTypes { get; set; } = new();

    public PageIngredientTypes(SessionData sessionData)
    {
        this.InitializeComponent();
        this.BindingContext = this; // ”станавливаем контекст данных
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        List<Common.Classes.DB.ProductTypes> items = await this.ApiClient.GetAllProductTypesAsync();

        // «аполн€ем ObservableCollection, чтобы CollectionView автоматически обновилс€
        this.IngredientTypes.Clear();
        foreach (Common.Classes.DB.ProductTypes item in items)
        {
            this.IngredientTypes.Add(item);
        }

        this.ccvItems.Items = this.IngredientTypes;
    }
}
