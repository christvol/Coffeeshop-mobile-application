using Common.Classes.DB;
using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageOrders : CustomContentPage
    {
        #region Свойства

        public ObservableCollection<OrderDetailsView> Orders { get; set; } = new();



        #endregion

        #region Конструкторы/Деструкторы

        public PageOrders(SessionData? sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.SessionData = sessionData ?? new SessionData();

            // Устанавливаем обработчики событий
            this.ccvOrders.SetEditCommand<OrderDetailsView>(this.OnEditOrderClicked);
            this.ccvOrders.SetDeleteCommand<OrderDetailsView>(this.OnDeleteOrderClicked);

        }

        #endregion

        #region Методы

        /// <summary>
        /// Загружает список заказов.
        /// </summary>
        private async void LoadOrders()
        {
            try
            {
                List<Common.Classes.DB.OrderDetailsView> details = await this.ApiClient.GetAllOrderDetailsAsync();
                var grouped = details
                    .GroupBy(d => d.OrderId)
                    .Select(g => g.First())
                    .ToList();

                this.Orders.UpdateObservableCollection(grouped);

                this.ccvOrders.SetDisplayedFields("OrderId", "OrderDate", "CustomerFirstName", "CustomerLastName", "OrderStatus", "PaymentStatus");
                this.ccvOrders.SetItems(this.Orders);

            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось загрузить заказы: {ex.Message}", "OK");
            }
        }


        #endregion

        #region Обработчики событий

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.LoadOrders();

            // Настраиваем CollectionView
            this.ccvOrders.SetDisplayedFields("Id", "IdEmployee", "IdCustomer", "CreationDate", "IdStatus");
            this.ccvOrders.SetItems(this.Orders);
        }

        /// <summary>
        /// Обработчик кнопки "Добавить".
        /// </summary>
        private async void OnAddOrderClicked(object sender, EventArgs e)
        {
            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData?.CurrentUser,
                Mode = WindowMode.Create,
                Data = new OrderDTO()
            };

            await this.Navigation.PushAsync(new PageOrderEdit(newSessionData));
        }

        /// <summary>
        /// Обработчик кнопки "Редактировать".
        /// </summary>
        private async void OnEditOrderClicked(OrderDetailsView orderView)
        {
            try
            {
                if (this.SessionData == null || this.SessionData.CurrentUser == null)
                {
                    throw new InvalidOperationException("SessionData or CurrentUser is not set.");
                }

                // 🔽 Загружаем реальный OrderDTO по ID
                OrderDTO? order = await this.ApiClient.GetOrderByIdAsync(orderView.OrderId);
                if (order == null)
                {
                    await this.DisplayAlert("Ошибка", "Заказ не найден", "OK");
                    return;
                }

                var editSessionData = new SessionData
                {
                    CurrentUser = this.SessionData.CurrentUser,
                    Mode = WindowMode.Update,
                    Data = order
                };

                await this.Navigation.PushAsync(new PageOrderEdit(editSessionData));

                this.LoadOrders(); // Обновляем список после возврата
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось открыть редактирование: {ex.Message}", "OK");
            }
        }



        /// <summary>
        /// Обработчик кнопки "Удалить".
        /// </summary>
        private async void OnDeleteOrderClicked(OrderDetailsView orderView)

        {
            bool confirm = await this.DisplayAlert("Удаление", $"Вы уверены, что хотите удалить заказ #{orderView.OrderId}?", "Да", "Нет");
            if (!confirm)
            {
                return;
            }

            try
            {
                await this.ApiClient.DeleteOrderAsync(orderView.OrderId);
                await this.DisplayAlert("Успех", "Заказ удален.", "OK");
                this.LoadOrders();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось удалить заказ: {ex.Message}", "OK");
            }
        }

        #endregion
    }
}
