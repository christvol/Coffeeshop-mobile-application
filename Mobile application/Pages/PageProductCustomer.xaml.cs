using Common.Classes.DTO;
using Common.Classes.Session;

namespace Mobile_application.Pages;

public partial class PageProductCustomer : CustomContentPage
{
    #region Поля
    private ProductDTO? _product;
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
    /// Инициализация данных страницы
    /// </summary>
    private async void InitializeData()
    {
        if (this.SessionData?.Data is ProductDTO product)
        {
            this._product = product;
            this.BindingContext = this;
            await this.HandleOrder();
        }
    }

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
        OrderDTO? existingOrder = orders.FirstOrDefault(o => o.IdStatus == pendingStatus.Id);

        if (existingOrder == null)
        {
            await this.CreateNewOrder(userId, pendingStatus.Id);
        }
        else
        {
            await this.AddProductToExistingOrder(existingOrder.Id);
        }
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
                new() {
                    IdProduct = this._product.Id,
                    Total = this._product.Fee,
                    Ingredients = new List<OrderItemIngredientDTO>()
                }
            }
        };

        OrderDTO? createdOrder = await this.ApiClient.CreateOrderAsync(newOrder);

        if (createdOrder != null)
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

    #endregion

    #region Свойства

    public string ProductTitle => this._product?.Title ?? "Без названия";
    public string ProductDescription => this._product?.Description ?? "Описание отсутствует";
    public string ProductImage => "coffedefaultpreview.png";
    //this._product?.ProductImages.FirstOrDefault() ?? "coffedefaultpreview.png";
    public string ProductFee => this._product != null ? $"{this._product.Fee:C}" : "Цена не указана";

    public List<string> ProductIngredients => this._product != null
        ? new List<string> { $"Тип продукта: {this._product.IdProductType}" }
        : new List<string> { "Нет данных" };

    #endregion
}
