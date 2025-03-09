using Common.Classes.DTO;
using Common.Classes.Session;

namespace Mobile_application.Pages
{
    public partial class PageOrderEdit : CustomContentPage
    {
        #region Поля
        private List<OrderStatusDTO> _orderStatuses = new();
        #endregion

        #region Свойства
        public OrderDTO Order
        {
            get; set;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public PageOrderEdit(SessionData sessionData)
            : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.Order = this.SessionData.Data as OrderDTO ?? new OrderDTO();

            // Если создается новый заказ, устанавливаем текущую дату
            if (this.SessionData.Mode == WindowMode.Create && this.Order.CreationDate == null)
            {
                this.Order.CreationDate = DateTime.UtcNow;
            }

            this.FillFields();
            this.SetFieldAccessibility();
        }
        #endregion

        #region Методы
        private void FillFields()
        {
            this.EntryCreationDate.Text = this.Order?.CreationDate?.ToString("dd.MM.yyyy HH:mm") ?? "";
            this.EntryCustomer.Text = this.Order?.IdCustomer?.ToString() ?? "";
            this.EntryEmployee.Text = this.Order?.IdEmployee?.ToString() ?? "";

            this.pStatus.ConfigurePicker<OrderStatusDTO>(this._orderStatuses, "Title", "Id", this.Order?.IdStatus);

            this.ccvOrderItems.SetDisplayedFields("IdProduct", "Total");
            this.ccvOrderItems.SetItems(this.Order.OrderItems);
        }

        private void SetFieldAccessibility()
        {
            if (this.SessionData.Mode == WindowMode.Read)
            {
                this.EntryCustomer.IsEnabled = false;
                this.EntryEmployee.IsEnabled = false;
                this.pStatus.IsEnabled = false;
            }

            // Дата создания всегда только для просмотра
            this.EntryCreationDate.IsEnabled = false;
        }
        #endregion

        #region Обработчики событий
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this._orderStatuses = await this.ApiClient.GetAllOrderStatusesAsync();
            this.pStatus.ConfigurePicker<OrderStatusDTO>(this._orderStatuses, "Title", "Id", this.Order?.IdStatus);
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (this.Order == null)
                {
                    await this.DisplayAlert("Ошибка", "Данные заказа отсутствуют.", "OK");
                    return;
                }

                this.Order.IdCustomer = int.TryParse(this.EntryCustomer.Text, out int customerId) ? customerId : null;
                this.Order.IdEmployee = int.TryParse(this.EntryEmployee.Text, out int employeeId) ? employeeId : null;
                this.Order.IdStatus = (this.pStatus.SelectedItem as OrderStatusDTO)?.Id ?? this.Order.IdStatus;

                if (this.SessionData.Mode == WindowMode.Create)
                {
                    _ = await this.ApiClient.CreateOrderAsync(this.Order);
                    await this.DisplayAlert("Успех", "Заказ успешно добавлен.", "OK");
                }
                else if (this.SessionData.Mode == WindowMode.Update)
                {
                    _ = await this.ApiClient.UpdateOrderAsync(this.Order.Id, this.Order);
                    await this.DisplayAlert("Успех", "Заказ успешно обновлен.", "OK");
                }

                _ = await this.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось сохранить заказ: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            _ = await this.Navigation.PopAsync();
        }
        #endregion
    }
}
