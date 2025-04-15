using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Mobile_application.Pages;

public partial class PageProductCustomer : CustomContentPage, INotifyPropertyChanged
{
    #region Поля
    private ProductDTO? _currentProduct;
    private OrderDTO? _currentOrder;
    public ObservableCollection<Common.Classes.DB.OrderDetailsView> ProductIngredients { get; set; } = new();

    private ObservableCollection<IngredientTypeDTO> IngredientTypes { get; set; } = new();

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
        if (this._currentProduct == null)
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
                    IdProduct = this._currentProduct.Id,
                    Total = this._currentProduct.Fee,
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
        if (this._currentProduct == null)
        {
            await this.DisplayAlert("Ошибка", "Выбранный продукт не найден", "OK");
            return;
        }

        var newProduct = new OrderProductDTO
        {
            IdProduct = this._currentProduct.Id,
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
            this._currentProduct = dataObject.GetType().GetProperty("Product")?.GetValue(dataObject) as ProductDTO;
        }

        if (this._currentProduct == null)
        {
            await this.DisplayAlert("Ошибка", "Продукт не найден", "OK");
            return;
        }

        this.BindingContext = this;

        await this.LoadIngredientTypes();


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
        if (this._currentOrder == null || this._currentProduct == null)
        {
            return;
        }

        // Получаем детали заказа
        List<Common.Classes.DB.OrderDetailsView> orderDetails = await this.ApiClient.GetOrderDetailsByIdAsync(this._currentOrder.Id);

        // Фильтруем только ингредиенты, относящиеся к текущему продукту
        var productIngredients = orderDetails
            .Where(d => d.ProductId == this._currentProduct.Id && d.IngredientId != null)
            .ToList();

        this.ProductIngredients.UpdateObservableCollection(productIngredients);
        // ⬇️ Уведомляем UI, что сумма изменилась
        this.OnPropertyChanged(nameof(this.OrderTotal));

        // Настраиваем CustomCollectionView
        this.ccvProductIngredients.SetDisplayedFields("IngredientTitle", "IngredientFee");
        this.ccvProductIngredients.SetItems(this.ProductIngredients);
        this.ccvProductIngredients.SetDeleteCommand<Common.Classes.DB.OrderDetailsView>(this.OnIngredientDelete);

        this.OnPropertyChanged(nameof(this.OrderTotal)); // ⬅️ здесь пересчитываем!
    }

    private async Task LoadIngredientTypes()
    {
        List<IngredientTypeDTO> types = await this.ApiClient.GetAllIngredientTypesAsync();
        this.IngredientTypes.UpdateObservableCollection(types);
        this.cvIngredientTypes.SetItems(this.IngredientTypes);
        this.cvIngredientTypes.SetDisplayedFields("Title");
        this.cvIngredientTypes.SetItemSelectedCommand<IngredientTypeDTO>(this.OnIngredientTypeSelected);
    }


    /// <summary>
    /// Удаляет ингредиент из заказа
    /// </summary>
    private async void OnIngredientDelete(Common.Classes.DB.OrderDetailsView ingredient)
    {
        if (this._currentOrder == null || this._currentProduct == null)
        {
            await this.DisplayAlert("Ошибка", "Нет данных для удаления", "OK");
            return;
        }

        bool confirm = await this.DisplayAlert("Подтверждение", $"Удалить ингредиент \"{ingredient.IngredientTitle}\" из заказа?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        bool success = await this.ApiClient.RemoveIngredientFromOrderAsync(this._currentOrder.Id, this._currentProduct.Id, ingredient.IngredientId.Value);

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

        if (this._currentProduct == null)
        {
            await this.DisplayAlert("Ошибка", "Продукт не найден", "OK");
            return;
        }

        var newSessionData = new SessionData
        {
            CurrentUser = this.SessionData?.CurrentUser,
            Data = new { Order = this._currentOrder, Product = this._currentProduct }
        };

        await this.Navigation.PushAsync(new PageIngredientTypes(newSessionData));
    }

    /// <summary>
    /// Обработчик нажатия кнопки "Добавить в корзину"
    /// </summary>
    private async void btnAddToCart_Clicked(object sender, EventArgs e)
    {
        if (this.SessionData?.CurrentUser == null)
        {
            await this.DisplayAlert("Ошибка", "Не удалось определить пользователя", "OK");
            return;
        }

        int userId = this.SessionData.CurrentUser.Id;
        OrderStatusDTO? createdStatus = await this.ApiClient.GetOrderStatusByTitleAsync("Created");

        if (createdStatus == null)
        {
            await this.DisplayAlert("Ошибка", "Не удалось получить статус 'Created'", "OK");
            return;
        }

        // Пытаемся найти уже созданный черновик-заказ
        List<OrderDTO> orders = await this.ApiClient.GetOrdersByCustomerIdAsync(userId);
        this._currentOrder = orders.FirstOrDefault(o => o.IdStatus == createdStatus.Id);

        if (this._currentOrder == null)
        {
            // Если продукта нет — ошибка
            if (this._currentProduct == null)
            {
                await this.DisplayAlert("Ошибка", "Продукт не выбран", "OK");
                return;
            }

            // Создаём черновик-заказ
            var newOrder = new OrderDTO
            {
                IdCustomer = userId,
                IdStatus = createdStatus.Id,
                IdStatusPayment = 1, // Not Paid
                CreationDate = DateTime.UtcNow,
                OrderItems = new List<OrderItemsDTO>
            {
                new()
                {
                    IdProduct = this._currentProduct.Id,
                    Total = this._currentProduct.Fee,
                    Ingredients = new List<OrderItemIngredientDTO>()
                }
            }
            };

            this._currentOrder = await this.ApiClient.CreateOrderAsync(newOrder);

            if (this._currentOrder == null)
            {
                await this.DisplayAlert("Ошибка", "Не удалось создать заказ", "OK");
                return;
            }
        }
        else
        {
            // Добавляем товар в уже существующий черновик-заказ
            //var newProduct = new OrderProductDTO
            //{
            //    IdProduct = this._currentProduct?.Id ?? 0,
            //    Quantity = 1
            //};

            //bool success = await this.ApiClient.AddProductToOrderAsync(this._currentOrder.Id, newProduct);

            //if (!success)
            //{
            //    await this.DisplayAlert("Ошибка", "Не удалось добавить продукт в заказ", "OK");
            //    return;
            //}
        }

        var newSessionData = new SessionData
        {
            CurrentUser = this.SessionData.CurrentUser,
            Data = this._currentOrder
        };

        await this.Navigation.PushAsync(new PageOrderCustomer(newSessionData));
    }

    private async void OnIngredientTypeSelected(IngredientTypeDTO selectedType)
    {
        if (this._currentOrder == null)
        {
            await this.DisplayAlert("Ошибка", "Заказ не найден", "OK");
            return;
        }

        List<IngredientDTO> allIngredients = await this.ApiClient.GetAllIngredientsAsync();
        var filtered = allIngredients
            .Where(i => i.IdIngredientType == selectedType.Id)
            .OrderBy(i => i.Title)
            .ToList();

        var newSessionData = new SessionData
        {
            CurrentUser = this.SessionData?.CurrentUser,
            Data = new
            {
                Order = this._currentOrder,
                Product = this._currentProduct,
                Ingredients = filtered,
                SelectedType = selectedType
            }
        };

        await this.Navigation.PushAsync(new PageIngredients(newSessionData));
    }

    #endregion

    #region Свойства
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public string ProductTitle => this._currentProduct?.Title ?? "Без названия";
    public string ProductDescription => this._currentProduct?.Description ?? "Описание отсутствует";
    public string ProductImage => "coffedefaultpreview.png";
    public string OrderTotal
    {
        get
        {
            if (this._currentOrder == null)
            {
                return "Сумма не определена";
            }

            float total = 0;

            // Группируем продукты — один раз учитываем каждый
            IEnumerable<int> productIds = this.ProductIngredients
                .Where(i => i.ProductId != null)
                .Select(i => i.ProductId.Value)
                .Distinct();

            foreach (int productId in productIds)
            {
                Common.Classes.DB.OrderDetailsView? product = this.ProductIngredients.FirstOrDefault(i => i.ProductId == productId);
                if (product != null)
                {
                    total += product.ProductPrice;
                }
            }

            // Добавляем стоимость ингредиентов
            foreach (Common.Classes.DB.OrderDetailsView ing in this.ProductIngredients)
            {
                if (ing.IngredientFee != null && ing.IngredientQuantity != null)
                {
                    total += ing.IngredientFee.Value * ing.IngredientQuantity.Value;
                }
            }

            return $"{total:C}";
        }
    }



    #endregion
}
