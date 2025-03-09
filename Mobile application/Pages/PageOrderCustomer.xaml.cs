using Common.Classes.DB;
using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages;

public partial class PageOrderCustomer : CustomContentPage
{
    #region ����
    private OrderDTO? _currentOrder;
    public ObservableCollection<OrderDetailsView> OrderProducts { get; set; } = new();
    #endregion

    #region ������������/�����������
    public PageOrderCustomer(SessionData sessionData) : base(sessionData)
    {
        this.InitializeComponent();
        this.InitializeData();
    }
    #endregion

    #region ������

    /// <summary>
    /// ������������� ������ ��������.
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
            await this.DisplayAlert("������", "����� �� ������", "OK");
            return;
        }

        this.BindingContext = this;

        // ������������� ���������� �������� ��������
        this.ccvProducts.SetDeleteCommand<OrderDetailsView>(this.OnDeleteProduct);

        await this.LoadOrderProducts();
    }


    /// <summary>
    /// ��������� ���������� �������� �������� ������.
    /// </summary>
    private async Task LoadOrderProducts()
    {
        if (this._currentOrder == null)
        {
            return;
        }

        // �������� ������ ������
        List<OrderDetailsView> orderDetails = await this.ApiClient.GetOrderDetailsByIdAsync(this._currentOrder.Id);

        // ��������� ������ ���������� �������� �� ProductId
        var productList = orderDetails
            .GroupBy(d => d.ProductId)
            .Select(g => g.First()) // ����� ������ ������� ������ ������
            .ToList();

        // ��������� ���������
        this.OrderProducts.UpdateObservableCollection(productList);

        // ��������� CustomCollectionView
        this.ccvProducts.SetDisplayedFields("ProductTitle");
        this.ccvProducts.SetItems(this.OrderProducts);
    }

    /// <summary>
    /// ���������� �������� �������� �� ������.
    /// </summary>
    private async void OnDeleteProduct(OrderDetailsView selectedProduct)
    {
        if (this._currentOrder == null || selectedProduct.ProductId == null)
        {
            return;
        }

        bool confirm = await this.DisplayAlert("�������������", $"������� ������� \"{selectedProduct.ProductTitle}\" �� ������?", "��", "���");
        if (!confirm)
        {
            return;
        }

        bool success = await this.ApiClient.DeleteProductFromOrderAsync(this._currentOrder.Id, selectedProduct.ProductId.Value);

        if (success)
        {
            await this.DisplayAlert("�����", "������� ����� �� ������", "OK");
            await this.LoadOrderProducts(); // ��������� ������ ���������
        }
        else
        {
            await this.DisplayAlert("������", "�� ������� ������� �������", "OK");
        }
    }

    #endregion

    #region ����������� �������

    /// <summary>
    /// ���������� ������� ������ "�������� � �������"
    /// </summary>
    private async void btnAddToCart_Clicked(object sender, EventArgs e)
    {
        if (this._currentOrder == null)
        {
            await this.DisplayAlert("������", "����� �� ������", "OK");
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
        // ������ ������
    }

    #endregion
}
