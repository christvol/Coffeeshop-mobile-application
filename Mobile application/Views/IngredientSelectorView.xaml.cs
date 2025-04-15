using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes.API;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Views;

public partial class IngredientSelectorView : ContentView
{
    private readonly SessionData _sessionData;
    private readonly ApiClient _apiClient;

    private OrderDTO? _order;
    private ProductDTO? _product;
    private IngredientTypeDTO? _selectedType;

    private readonly ObservableCollection<IngredientTypeDTO> _types = new();
    private readonly ObservableCollection<IngredientDTO> _ingredients = new();

    public event Action? IngredientAdded;

    public IngredientSelectorView(SessionData sessionData, ApiClient apiClient)
    {
        this.InitializeComponent();
        this._sessionData = sessionData;
        this._apiClient = apiClient;

        if (sessionData.Data is { } data)
        {
            this._order = data.GetType().GetProperty("Order")?.GetValue(data) as OrderDTO;
            this._product = data.GetType().GetProperty("Product")?.GetValue(data) as ProductDTO;
        }

        this.cvTypes.ItemsSource = this._types;
        this.cvIngredients.ItemsSource = this._ingredients;

        this.LoadIngredientTypes();
    }

    private async void LoadIngredientTypes()
    {
        List<IngredientTypeDTO> list = await this._apiClient.GetAllIngredientTypesAsync();
        this._types.UpdateObservableCollection(list);
    }

    private async void cvTypes_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not IngredientTypeDTO selectedType)
        {
            return;
        }

        this._selectedType = selectedType;

        List<IngredientDTO> allIngredients = await this._apiClient.GetAllIngredientsAsync();
        var filtered = allIngredients
            .Where(i => i.IdIngredientType == selectedType.Id)
            .OrderBy(i => i.Title)
            .ToList();

        this._ingredients.UpdateObservableCollection(filtered);

        this.TypeSection.IsVisible = false;
        this.IngredientSection.IsVisible = true;
    }

    private async void cvIngredients_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not IngredientDTO selected)
        {
            return;
        }

        if (this._order == null || this._product == null)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Нет данных заказа", "OK");
            return;
        }

        var dto = new OrderItemIngredientDTO
        {
            IdOrderProduct = this._product.Id,
            IdIngredient = selected.Id,
            Amount = 1
        };

        bool success = await this._apiClient.AddIngredientToOrderAsync(this._order.Id, this._product.Id, dto);
        if (success)
        {
            await Application.Current.MainPage.DisplayAlert("Успех", "Ингредиент добавлен", "OK");
            IngredientAdded?.Invoke();

            // Вернуться к списку типов
            this.IngredientSection.IsVisible = false;
            this.TypeSection.IsVisible = true;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось добавить", "OK");
        }
    }
}
