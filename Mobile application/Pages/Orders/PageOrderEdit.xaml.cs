using Common.Classes.DB;
using Common.Classes.DTO;
using Common.Classes.Session;

namespace Mobile_application.Pages
{
    public partial class PageOrderEdit : CustomContentPage
    {
        #region ����
        private List<OrderStatusDTO> _orderStatuses = new();
        private List<OrderDetailsView> _orderDetails = new();
        #endregion

        #region ��������
        public OrderDTO Order
        {
            get; set;
        }
        #endregion

        #region ������������/�����������
        public PageOrderEdit(SessionData sessionData)
            : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.Order = this.SessionData.Data as OrderDTO ?? new OrderDTO();

            // ���� ��������� ����� �����, ������������� ������� ����
            if (this.SessionData.Mode == WindowMode.Create && this.Order.CreationDate == null)
            {
                this.Order.CreationDate = DateTime.UtcNow;
            }

            this.FillFields();
            this.SetFieldAccessibility();
        }
        #endregion

        #region ������

        /// <summary>
        /// ��������� ���� �����.
        /// </summary>
        private async void FillFields()
        {
            this.EntryCreationDate.Text = this.Order?.CreationDate?.ToString("dd.MM.yyyy HH:mm") ?? "";
            this.EntryCustomer.Text = this.Order?.IdCustomer?.ToString() ?? "";
            this.EntryEmployee.Text = this.Order?.IdEmployee?.ToString() ?? "";

            this.pStatus.ConfigurePicker<OrderStatusDTO>(this._orderStatuses, "Title", "Id", this.Order?.IdStatus);

            await this.LoadOrderDetails();

            this.ccvOrderItems.SetDisplayedFields("ProductTitle", "Total");
            this.ccvOrderItems.SetItems(this._orderDetails);

            // ������������� ���������� ������ ��������
            this.ccvOrderItems.SetItemSelectedCommand<OrderDetailsView>(this.OnOrderItemSelected);
        }

        /// <summary>
        /// ��������� ������ ������ �� `OrderDetailsView`.
        /// </summary>
        private async Task LoadOrderDetails()
        {
            if (this.Order == null)
            {
                return;
            }

            this._orderDetails = await this.ApiClient.GetOrderDetailsByIdAsync(this.Order.Id);
        }

        /// <summary>
        /// ������������� ����������� �����.
        /// </summary>
        private void SetFieldAccessibility()
        {
            if (this.SessionData.Mode == WindowMode.Read)
            {
                this.EntryCustomer.IsEnabled = false;
                this.EntryEmployee.IsEnabled = false;
                this.pStatus.IsEnabled = false;
            }

            // ���� �������� ������ ������ ��� ���������
            this.EntryCreationDate.IsEnabled = false;
        }
        #endregion

        #region ����������� �������

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this._orderStatuses = await this.ApiClient.GetAllOrderStatusesAsync();
            this.pStatus.ConfigurePicker<OrderStatusDTO>(this._orderStatuses, "Title", "Id", this.Order?.IdStatus);
        }

        /// <summary>
        /// ���������� ������ �������� ������.
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
                    await this.DisplayAlert("������", "������ ������ �����������.", "OK");
                    return;
                }

                this.Order.IdCustomer = int.TryParse(this.EntryCustomer.Text, out int customerId) ? customerId : null;
                this.Order.IdEmployee = int.TryParse(this.EntryEmployee.Text, out int employeeId) ? employeeId : null;
                this.Order.IdStatus = (this.pStatus.SelectedItem as OrderStatusDTO)?.Id ?? this.Order.IdStatus;

                if (this.SessionData.Mode == WindowMode.Create)
                {
                    _ = await this.ApiClient.CreateOrderAsync(this.Order);
                    await this.DisplayAlert("�����", "����� ������� ��������.", "OK");
                }
                else if (this.SessionData.Mode == WindowMode.Update)
                {
                    _ = await this.ApiClient.UpdateOrderAsync(this.Order.Id, this.Order);
                    await this.DisplayAlert("�����", "����� ������� ��������.", "OK");
                }

                _ = await this.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("������", $"�� ������� ��������� �����: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            _ = await this.Navigation.PopAsync();
        }

        #endregion
    }
}
