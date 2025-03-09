using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageOrders : CustomContentPage
    {
        #region ��������

        public ObservableCollection<OrderDTO> Orders { get; set; } = new();

        #endregion

        #region ������������/�����������

        public PageOrders(SessionData? sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.SessionData = sessionData ?? new SessionData();

            // ������������� ����������� �������
            this.ccvOrders.SetEditCommand<OrderDTO>(this.OnEditOrderClicked);
            this.ccvOrders.SetDeleteCommand<OrderDTO>(this.OnDeleteOrderClicked);
        }

        #endregion

        #region ������

        /// <summary>
        /// ��������� ������ �������.
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
                await this.DisplayAlert("������", $"�� ������� ��������� ������: {ex.Message}", "OK");
            }
        }

        #endregion

        #region ����������� �������

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.LoadOrders();

            // ����������� CollectionView
            this.ccvOrders.SetDisplayedFields("Id", "IdEmployee", "IdCustomer", "CreationDate", "IdStatus");
            this.ccvOrders.SetItems(this.Orders);
        }

        /// <summary>
        /// ���������� ������ "��������".
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
        /// ���������� ������ "�������������".
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

                // ����� �������� �������� ��������� �����
                if (editSessionData.Mode == WindowMode.Update)
                {
                    _ = await this.ApiClient.UpdateOrderAsync(order.Id, order);
                    await this.DisplayAlert("�����", "����� ������� ��������.", "OK");
                    this.LoadOrders(); // ���������� ������ �������
                }
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("������", $"�� ������� ������� ��������������: {ex.Message}", "OK");
            }
        }


        /// <summary>
        /// ���������� ������ "�������".
        /// </summary>
        private async void OnDeleteOrderClicked(OrderDTO order)
        {
            bool confirm = await this.DisplayAlert("��������", $"�� �������, ��� ������ ������� ����� #{order.Id}?", "��", "���");
            if (!confirm)
            {
                return;
            }

            try
            {
                await this.ApiClient.DeleteOrderAsync(order.Id);
                await this.DisplayAlert("�����", "����� ������.", "OK");
                this.LoadOrders();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("������", $"�� ������� ������� �����: {ex.Message}", "OK");
            }
        }

        #endregion
    }
}
