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
        this.ccvProducts.SetEditCommand<OrderDetailsView>(this.OnEditProduct);



        await this.LoadOrderProducts();
    }


    /// <summary>
    /// Загружает уникальные продукты текущего заказа.
    /// </summary>
    private async Task LoadOrderProducts()
    {
        if (this._currentOrder == null || this.SessionData?.CurrentUser == null)
        {
            return;
        }

        try
        {
            int userId = this.SessionData.CurrentUser.Id;

            // Загружаем все позиции заказа
            List<OrderDetailsView> orderDetails = await this.ApiClient.GetOrderDetailsByIdAsync(this._currentOrder.Id);

            // Оставляем только те, что принадлежат текущему пользователю
            var filtered = orderDetails
                .Where(d => d.CustomerId == userId)
                .ToList();

            // ❗ Группируем по OrderProductId, чтобы избежать дублирования одного и того же продукта с разными ингредиентами
            var grouped = filtered
                .GroupBy(d => d.OrderProductId)
                .Select(g => g.First()) // берём одну строку на продукт
                .ToList();

            // Обновляем коллекцию
            this.OrderProducts.UpdateObservableCollection(grouped);

            // Обновляем CollectionView
            this.ccvProducts.SetDisplayedFields("ProductTitle", "ProductPrice");
            this.ccvProducts.SetItems(this.OrderProducts);
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось загрузить продукты заказа: {ex.Message}", "OK");
        }
    }




    /// <summary>
    /// Обработчик удаления продукта из заказа.
    /// </summary>
    private async void OnDeleteProduct(OrderDetailsView selectedProduct)
    {
        if (this._currentOrder == null)
        {
            return;
        }

        if (selectedProduct.ProductId == null || selectedProduct.OrderProductId == null)
        {
            await this.DisplayAlert("Ошибка", "Нет информации о позиции заказа", "OK");
            return;
        }

        bool confirm = await this.DisplayAlert("Подтверждение", $"Удалить продукт \"{selectedProduct.ProductTitle}\" из заказа?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        bool success = await this.ApiClient.DeleteProductFromOrderAsync(this._currentOrder.Id, selectedProduct.OrderProductId);

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


    private async void OnEditProduct(OrderDetailsView selectedProduct)
    {
        if (this._currentOrder == null)
        {
            await this.DisplayAlert("Ошибка", "Заказ не найден", "OK");
            return;
        }

        ProductDTO? product = await this.ApiClient.GetProductByIdAsync(selectedProduct.ProductId ?? 0);

        if (product == null)
        {
            await this.DisplayAlert("Ошибка", "Продукт не найден", "OK");
            return;
        }

        var newSessionData = new SessionData
        {
            CurrentUser = this.SessionData?.CurrentUser,
            Data = new { Order = this._currentOrder, Product = product }
        };

        await this.Navigation.PushAsync(new PageProductCustomer(newSessionData));
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
            await this.DisplayAlert("Ошибка", "Заказ не найден. Выберите категорию продукта", "OK");

            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData?.CurrentUser
            };

            await this.Navigation.PushAsync(new PageProductTypes(newSessionData));
            return;
        }

        var session = new SessionData
        {
            CurrentUser = this.SessionData?.CurrentUser,
            Data = this._currentOrder
        };

        await this.Navigation.PushAsync(new PageProductTypes(session));
    }


    private async void btnPay_Clicked(object sender, EventArgs e)
    {
        if (this._currentOrder == null)
        {
            await this.DisplayAlert("Ошибка", "Заказ не найден", "OK");
            return;
        }

        const int PaidStatusId = 2;         // "Paid"
        const int InProgressStatusId = 3;   // "In Progress"

        bool paymentSuccess = await this.ApiClient.SetOrderPaymentStatusAsync(this._currentOrder.Id, PaidStatusId);
        bool statusSuccess = await this.ApiClient.UpdateOrderStatusAsync(this._currentOrder.Id, InProgressStatusId);

        if (paymentSuccess && statusSuccess)
        {
            await this.DisplayAlert("Успех", "Оплата успешно проведена", "OK");

            // Сбрасываем текущий заказ
            this._currentOrder = null;

            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData?.CurrentUser,
                Data = null
            };

            await this.Navigation.PushAsync(new PageProductTypes(newSessionData));
        }
        else
        {
            await this.DisplayAlert("Ошибка", "Не удалось обновить заказ или статус оплаты", "OK");
        }
    }




    #endregion
}
