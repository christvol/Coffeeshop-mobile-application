using Common.Classes.DTO;
using Common.Classes.Session;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages;

public partial class PageIngredients : CustomContentPage
{
    public ObservableCollection<object> observableCollection { get; set; } = new();
    public PageIngredients(SessionData sessionData)
    {
        this.InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        List<IngredientDTO> items = await this.ApiClient.GetAllIngredientsAsync();

        // «аполн€ем ObservableCollection, чтобы CollectionView автоматически обновилс€
        this.observableCollection.Clear();
        foreach (IngredientDTO item in items)
        {
            this.observableCollection.Add(item);
        }
        this.ccvItems.DisplayedFields = new List<string>() { "Id", "Title", "Description" };
        this.ccvItems.Items = this.observableCollection;
    }
}