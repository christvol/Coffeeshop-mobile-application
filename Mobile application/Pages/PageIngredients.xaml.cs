using Common.Classes.DTO;
using Common.Classes.Session;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Mobile_application.Pages;

public partial class PageIngredients : CustomContentPage
{
    public ICommand EditIngredientCommand
    {
        get;
    }
    public ICommand DeleteIngredientCommand
    {
        get;
    }
    public ObservableCollection<object> observableCollection { get; set; } = new();
    public PageIngredients(SessionData sessionData)
    {
        this.InitializeComponent();
        this.ccvItems.EditCommand = new Command<IngredientDTO>(this.OnEditIngredient);
        this.ccvItems.DeleteCommand = new Command<IngredientDTO>(this.OnDeleteIngredient);
        this.BindingContext = this;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        List<IngredientDTO> items = await this.ApiClient.GetAllIngredientsAsync();

        // Заполняем ObservableCollection, чтобы CollectionView автоматически обновился
        this.observableCollection.Clear();
        foreach (IngredientDTO item in items)
        {
            this.observableCollection.Add(item);
        }
        this.ccvItems.DisplayedFields = new List<string>() { "Id", "Title", "Description" };
        this.ccvItems.Items = this.observableCollection;
    }

    private async void OnEditIngredient(IngredientDTO ingredient)
    {
        //var editSessionData = new SessionData
        //{
        //    CurrentUser = this.SessionData.CurrentUser,
        //    Mode = WindowMode.Update,
        //    Data = ingredient
        //};

        //await this.Navigation.PushAsync(new PageIngredientEdit(editSessionData));
    }

    private async void OnDeleteIngredient(IngredientDTO ingredient)
    {
        //var confirm = await this.DisplayAlert("Подтверждение", $"Удалить ингредиент \"{ingredient.Title}\"?", "Да", "Нет");
        //if (!confirm)
        //    return;

        //try
        //{
        //    await this.ApiClient.DeleteIngredientAsync(ingredient.Id);
        //    this.Product.Ingredients.Remove(ingredient);
        //    await this.DisplayAlert("Успех", "Ингредиент удалён.", "OK");
        //}
        //catch (Exception ex)
        //{
        //    await this.DisplayAlert("Ошибка", $"Не удалось удалить ингредиент: {ex.Message}", "OK");
        //}
    }
}