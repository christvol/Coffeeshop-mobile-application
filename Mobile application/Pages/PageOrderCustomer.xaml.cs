using Common.Classes.DB;
using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages;

public partial class PageOrderCustomer : CustomContentPage
{
    #region Поля
    private OrderDTO? _currentOrder;
    public ObservableCollection<OrderDetailsView> OrderProducts { get; set; } = new();
    #endregion

    #region Конструкторы/Деструкторы
    public PageOrderCustomer(SessionData sessionData) : base(sessionData)
    {
        this.InitializeComponent();
        this.InitializeData();
    }
    #endregion

    #region Методы

    /// <summary>
    /// Инициализация данных страницы.
    /// </summary>
    private async void InitializeData()
    {
        if (this.SessionData?.Data is OrderDTO order)
        {
            this._currentOrder = order;
        }
        else if (this.SessionData?.Data is { } dataObject &&
                 dataObject.GetType().GetProperty("Order") != null)
        {
            this._currentOrder = dataObject.GetType().GetProperty("Order")?.GetValue(dataObject) as OrderDTO;
        }

        if (this._currentOrder == null)
        {
            await this.DisplayAlert("Ошибка", "Заказ не найден", "OK");
            return;
        }

        this.BindingContext = this;

        // Устанавливаем обработчик удаления продукта
        this.ccvProducts.SetDeleteCommand<OrderDetailsView>(this.OnDeleteProduct);

        await this.LoadOrderProducts();
    }


    /// <summary>
    /// Загружает уникальные продукты текущего заказа.
    /// </summary>
    private async Task LoadOrderProducts()
    {
        if (this._currentOrder == null)
        {
            return;
        }

        // Получаем детали заказа
        List<OrderDetailsView> orderDetails = await this.ApiClient.GetOrderDetailsByIdAsync(this._currentOrder.Id);

        // Оставляем только уникальные продукты по ProductId
        var productList = orderDetails
            .GroupBy(d => d.ProductId)
            .Select(g => g.First()) // Берем первый элемент каждой группы
            .ToList();

        // Обновляем коллекцию
        this.OrderProducts.UpdateObservableCollection(productList);

        // Обновляем CustomCollectionView
        this.ccvProducts.SetDisplayedFields("ProductTitle");
        this.ccvProducts.SetItems(this.OrderProducts);
    }

    /// <summary>
    /// Обработчик удаления продукта из заказа.
    /// </summary>
    private async void OnDeleteProduct(OrderDetailsView selectedProduct)
    {
        if (this._currentOrder == null || selectedProduct.ProductId == null)
        {
            return;
        }

        bool confirm = await this.DisplayAlert("Подтверждение", $"Удалить продукт \"{selectedProduct.ProductTitle}\" из заказа?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        bool success = await this.ApiClient.DeleteProductFromOrderAsync(this._currentOrder.Id, selectedProduct.ProductId.Value);

        if (success)
        {
            await this.DisplayAlert("Успех", "Продукт удалён из заказа", "OK");
            await this.LoadOrderProducts(); // Обновляем список продуктов
        }
        else
        {
            await this.DisplayAlert("Ошибка", "Не удалось удалить продукт", "OK");
        }
    }

    #endregion

    #region Обработчики событий

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

        await this.Navigation.PushAsync(new PageProductTypes(newSessionData));
    }

    private void btnPay_Clicked(object sender, EventArgs e)
    {
        // Логика оплаты
    }

    #endregion
}
