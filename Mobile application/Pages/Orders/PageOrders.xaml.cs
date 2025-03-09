using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageOrders : CustomContentPage
    {
        #region Свойства

        public ObservableCollection<OrderDTO> Orders { get; set; } = new();

        #endregion

        #region Конструкторы/Деструкторы

        public PageOrders(SessionData? sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.SessionData = sessionData ?? new SessionData();

            // Устанавливаем обработчики событий
            this.ccvOrders.SetEditCommand<OrderDTO>(this.OnEditOrderClicked);
            this.ccvOrders.SetDeleteCommand<OrderDTO>(this.OnDeleteOrderClicked);
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
                List<OrderDTO> orders = await this.ApiClient.GetOrdersAsync();
                this.Orders.UpdateObservableCollection(orders);
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
        private async void OnEditOrderClicked(OrderDTO order)
        {
            try
            {
                if (this.SessionData == null || this.SessionData.CurrentUser == null)
                {
                    throw new InvalidOperationException("SessionData or CurrentUser is not set.");
                }

                var editSessionData = new SessionData
                {
                    CurrentUser = this.SessionData.CurrentUser,
                    Mode = WindowMode.Update,
                    Data = order
                };

                await this.Navigation.PushAsync(new PageOrderEdit(editSessionData));

                // После закрытия страницы обновляем заказ
                if (editSessionData.Mode == WindowMode.Update)
                {
                    _ = await this.ApiClient.UpdateOrderAsync(order.Id, order);
                    await this.DisplayAlert("Успех", "Заказ успешно обновлен.", "OK");
                    this.LoadOrders(); // Обновление списка заказов
                }
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось открыть редактирование: {ex.Message}", "OK");
            }
        }


        /// <summary>
        /// Обработчик кнопки "Удалить".
        /// </summary>
        private async void OnDeleteOrderClicked(OrderDTO order)
        {
            bool confirm = await this.DisplayAlert("Удаление", $"Вы уверены, что хотите удалить заказ #{order.Id}?", "Да", "Нет");
            if (!confirm)
            {
                return;
            }

            try
            {
                await this.ApiClient.DeleteOrderAsync(order.Id);
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
