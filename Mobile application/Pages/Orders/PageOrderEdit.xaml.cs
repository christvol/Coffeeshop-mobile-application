using Common.Classes.DB;
using Common.Classes.DTO;
using Common.Classes.Session;

namespace Mobile_application.Pages
{
    public partial class PageOrderEdit : CustomContentPage
    {
        #region Поля
        private List<OrderStatusDTO> _orderStatuses = new();
        private List<OrderDetailsView> _orderDetails = new();
        private List<Users> _allUsers = new();

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

        private void SetupUserPickers()
        {
            //var customers = this._allUsers.Where(u => u.IdUserTypeNavigation?.Title == "Customer").ToList();
            //var employees = this._allUsers.Where(u => u.IdUserTypeNavigation?.Title == "Employee").ToList();
            List<Users> customers = this._allUsers;
            List<Users> employees = this._allUsers;

            Users? selectedCustomer = customers.FirstOrDefault(u => u.Id == this.Order?.IdCustomer);
            Users? selectedEmployee = employees.FirstOrDefault(u => u.Id == this.Order?.IdEmployee);

            this.pCustomer.ConfigurePicker<Users>(customers, "FullName", "Id", selectedCustomer);
            this.pEmployee.ConfigurePicker<Users>(employees, "FullName", "Id", selectedEmployee);
        }




        /// <summary>
        /// Заполняет поля формы.
        /// </summary>
        private async void FillFields()
        {
            this.EntryCreationDate.Text = this.Order?.CreationDate?.ToString("dd.MM.yyyy HH:mm") ?? "";

            // Статус заказа
            OrderStatusDTO? selectedStatus = this._orderStatuses.FirstOrDefault(s => s.Id == this.Order?.IdStatus);
            this.pStatus.ConfigurePicker<OrderStatusDTO>(this._orderStatuses, "Title", "Id", selectedStatus);

            // Загрузка деталей заказа
            await this.LoadOrderDetails();

            this.ccvOrderItems.SetDisplayedFields("ProductTitle", "Total");
            this.ccvOrderItems.SetItems(this._orderDetails);

            this.ccvOrderItems.SetEditCommand<OrderDetailsView>(this.OnEditOrderProduct);
            this.ccvOrderItems.SetDeleteCommand<OrderDetailsView>(this.OnDeleteOrderProduct);

        }

        /// <summary>
        /// Загружает детали заказа из `OrderDetailsView`.
        /// </summary>
        private async Task LoadOrderDetails()
        {
            if (this.Order == null)
            {
                return;
            }

            this._orderDetails = (await this.ApiClient.GetOrderDetailsByIdAsync(this.Order.Id))
                .GroupBy(d => d.OrderProductId)
                .Select(g => g.First()) // одна запись на продукт
                .ToList();

        }

        /// <summary>
        /// Устанавливает доступность полей.
        /// </summary>
        private void SetFieldAccessibility()
        {
            if (this.SessionData.Mode == WindowMode.Read)
            {
                this.pCustomer.IsEnabled = false;
                this.pEmployee.IsEnabled = false;
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
            this._allUsers = await this.ApiClient.GetAllUsersAsync();

            this.SetupUserPickers(); // Сначала настраиваем источники
            this.FillFields();       // Потом выбираем значения
        }

        private async void OnEditOrderProduct(OrderDetailsView selectedProduct)
        {
            if (selectedProduct == null || this.Order == null)
            {
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
                Mode = WindowMode.Update,
                CurrentUser = this.SessionData?.CurrentUser,
                Data = new { Order = this.Order, Product = product }
            };

            await this.Navigation.PushAsync(new PageProductCustomer(newSessionData));
        }

        private async void OnDeleteOrderProduct(OrderDetailsView selectedProduct)
        {
            if (this.Order == null || selectedProduct.OrderProductId == null)
            {
                return;
            }

            bool confirm = await this.DisplayAlert("Подтверждение", $"Удалить продукт \"{selectedProduct.ProductTitle}\" из заказа?", "Да", "Нет");
            if (!confirm)
            {
                return;
            }

            bool success = await this.ApiClient.DeleteProductFromOrderAsync(this.Order.Id, selectedProduct.OrderProductId);

            if (success)
            {
                await this.DisplayAlert("Успех", "Продукт удалён из заказа", "OK");
                await this.LoadOrderDetails();
                this.ccvOrderItems.SetItems(this._orderDetails);
            }
            else
            {
                await this.DisplayAlert("Ошибка", "Не удалось удалить продукт", "OK");
            }
        }




        /// <summary>
        /// Обработчик выбора элемента списка.
        /// </summary>
        private async void OnOrderItemSelected(OrderDetailsView selectedItem)
        {
            if (selectedItem == null || this.Order == null)
            {
                return;
            }

            Users? customer = await this.ApiClient.GetUserByIdAsync((int)this.Order.IdCustomer);
            OrderDTO? order = await this.ApiClient.GetOrderByIdAsync(this.Order.Id);

            ProductDTO? product = await this.ApiClient.GetProductByIdAsync((int)selectedItem.ProductId);
            var newSessionData = new SessionData
            {
                CurrentUser = customer, //this.SessionData?.CurrentUser,
                Data = new { Order = order, Product = product }
            };

            await this.Navigation.PushAsync(new PageProductCustomer(newSessionData));
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

                // Получаем Id выбранных пользователей
                this.Order.IdCustomer = (this.pCustomer.SelectedItem as Users)?.Id;
                this.Order.IdEmployee = (this.pEmployee.SelectedItem as Users)?.Id;

                // Статус
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
