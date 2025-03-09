using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages;

public partial class PageProductCustomer : CustomContentPage
{
    #region Поля
    private ProductDTO? _product;
    private OrderDTO? _currentOrder;
    public ObservableCollection<Common.Classes.DB.OrderDetailsView> ProductIngredients { get; set; } = new();
    #endregion

    #region Конструкторы/Деструкторы
    public PageProductCustomer(SessionData sessionData) : base(sessionData)
    {
        this.InitializeComponent();
        this.InitializeData();
    }
    #endregion

    #region Методы

    /// <summary>
    /// Проверка существующего заказа и добавление продукта
    /// </summary>
    private async Task HandleOrder()
    {
        if (this.SessionData?.CurrentUser == null)
        {
            await this.DisplayAlert("Ошибка", "Не удалось определить пользователя", "OK");
            return;
        }

        int userId = this.SessionData.CurrentUser.Id;
        OrderStatusDTO? pendingStatus = await this.ApiClient.GetOrderStatusByTitleAsync("Pending");

        if (pendingStatus == null)
        {
            await this.DisplayAlert("Ошибка", "Не удалось получить статус 'Pending'", "OK");
            return;
        }

        List<OrderDTO> orders = await this.ApiClient.GetOrdersByCustomerIdAsync(userId);
        this._currentOrder = orders.FirstOrDefault(o => o.IdStatus == pendingStatus.Id);

        if (this._currentOrder == null)
        {
            await this.CreateNewOrder(userId, pendingStatus.Id);
        }
        else
        {
            await this.AddProductToExistingOrder(this._currentOrder.Id);
        }

        await this.LoadProductIngredients();
    }

    /// <summary>
    /// Создание нового заказа с продуктом
    /// </summary>
    private async Task CreateNewOrder(int userId, int statusId)
    {
        if (this._product == null)
        {
            await this.DisplayAlert("Ошибка", "Выбранный продукт не найден", "OK");
            return;
        }

        var newOrder = new OrderDTO
        {
            IdCustomer = userId,
            IdStatus = statusId,
            CreationDate = DateTime.UtcNow,
            OrderItems = new List<OrderItemsDTO>
            {
                new()
                {
                    IdProduct = this._product.Id,
                    Total = this._product.Fee,
                    Ingredients = new List<OrderItemIngredientDTO>()
                }
            }
        };

        this._currentOrder = await this.ApiClient.CreateOrderAsync(newOrder);

        if (this._currentOrder != null)
        {
            await this.DisplayAlert("Успех", "Заказ успешно создан и продукт добавлен", "OK");
        }
        else
        {
            await this.DisplayAlert("Ошибка", "Не удалось создать заказ", "OK");
        }
    }

    /// <summary>
    /// Добавление продукта в существующий заказ
    /// </summary>
    private async Task AddProductToExistingOrder(int orderId)
    {
        if (this._product == null)
        {
            await this.DisplayAlert("Ошибка", "Выбранный продукт не найден", "OK");
            return;
        }

        var newProduct = new OrderProductDTO
        {
            IdProduct = this._product.Id,
            Quantity = 1
        };

        bool success = await this.ApiClient.AddProductToOrderAsync(orderId, newProduct);

        if (success)
        {
            await this.DisplayAlert("Успех", "Продукт добавлен в заказ", "OK");
        }
        else
        {
            await this.DisplayAlert("Ошибка", "Не удалось добавить продукт в заказ", "OK");
        }
    }

    /// <summary>
    /// Инициализация данных страницы
    /// </summary>
    private async void InitializeData()
    {
        if (this.SessionData?.Data is { } dataObject &&
            dataObject.GetType().GetProperty("Order") != null &&
            dataObject.GetType().GetProperty("Product") != null)
        {
            this._currentOrder = dataObject.GetType().GetProperty("Order")?.GetValue(dataObject) as OrderDTO;
            this._product = dataObject.GetType().GetProperty("Product")?.GetValue(dataObject) as ProductDTO;
        }

        if (this._product == null)
        {
            await this.DisplayAlert("Ошибка", "Продукт не найден", "OK");
            return;
        }

        this.BindingContext = this;

        if (this._currentOrder == null)
        {
            await this.HandleOrder();
        }

        await this.LoadProductIngredients();
    }

    /// <summary>
    /// Загружает ингредиенты для текущего продукта в заказе.
    /// </summary>
    private async Task LoadProductIngredients()
    {
        if (this._currentOrder == null || this._product == null)
        {
            return;
        }

        // Получаем детали заказа
        List<Common.Classes.DB.OrderDetailsView> orderDetails = await this.ApiClient.GetOrderDetailsByIdAsync(this._currentOrder.Id);

        // Фильтруем только ингредиенты, относящиеся к текущему продукту
        var productIngredients = orderDetails
            .Where(d => d.ProductId == this._product.Id && d.IngredientId != null)
            .ToList();

        this.ProductIngredients.UpdateObservableCollection(productIngredients);

        // Настраиваем CustomCollectionView
        this.ccvProductIngredients.SetDisplayedFields("IngredientTitle", "IngredientQuantity");
        this.ccvProductIngredients.SetItems(this.ProductIngredients);
        this.ccvProductIngredients.SetDeleteCommand<Common.Classes.DB.OrderDetailsView>(this.OnIngredientDelete);
    }

    /// <summary>
    /// Удаляет ингредиент из заказа
    /// </summary>
    private async void OnIngredientDelete(Common.Classes.DB.OrderDetailsView ingredient)
    {
        if (this._currentOrder == null || this._product == null)
        {
            await this.DisplayAlert("Ошибка", "Нет данных для удаления", "OK");
            return;
        }

        bool confirm = await this.DisplayAlert("Подтверждение", $"Удалить ингредиент \"{ingredient.IngredientTitle}\" из заказа?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        bool success = await this.ApiClient.RemoveIngredientFromOrderAsync(this._currentOrder.Id, this._product.Id, ingredient.IngredientId.Value);

        if (success)
        {
            await this.DisplayAlert("Успех", "Ингредиент удален из заказа", "OK");
            await this.LoadProductIngredients();
        }
        else
        {
            await this.DisplayAlert("Ошибка", "Не удалось удалить ингредиент", "OK");
        }
    }

    #endregion

    #region Обработчики событий

    /// <summary>
    /// Обработчик нажатия кнопки "Выбрать ингредиент"
    /// </summary>
    private async void btnSelectIngredient_Clicked(object sender, EventArgs e)
    {
        if (this._currentOrder == null)
        {
            await this.DisplayAlert("Ошибка", "Заказ не найден", "OK");
            return;
        }

        if (this._product == null)
        {
            await this.DisplayAlert("Ошибка", "Продукт не найден", "OK");
            return;
        }

        var newSessionData = new SessionData
        {
            CurrentUser = this.SessionData?.CurrentUser,
            Data = new { Order = this._currentOrder, Product = this._product }
        };

        await this.Navigation.PushAsync(new PageIngredientTypes(newSessionData));
    }

    /// <summary>
    /// Обработчик нажатия кнопки "Добавить в корзину"
    /// </summary>
    private async void btnAddToCart_Clicked(object sender, EventArgs e)
    {
        if (this._currentOrder == null)
        {
            await this.DisplayAlert("Ошибка", "Заказ не найден", "OK");
            return;
        }

        var newSessionData = new SessionData
        {
            CurrentUser = this.SessionData?.CurrentUser,
            Data = this._currentOrder
        };

        await this.Navigation.PushAsync(new PageOrderCustomer(newSessionData));
    }


    #endregion

    #region Свойства

    public string ProductTitle => this._product?.Title ?? "Без названия";
    public string ProductDescription => this._product?.Description ?? "Описание отсутствует";
    public string ProductImage => "coffedefaultpreview.png";
    public string ProductFee => this._product != null ? $"{this._product.Fee:C}" : "Цена не указана";

    #endregion
}
