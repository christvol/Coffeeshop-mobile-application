using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageIngredients : CustomContentPage
    {
        #region ����
        private List<AllowedIngredientsDTO> _allowedIngredients = new();

        #endregion

        #region ��������

        public ObservableCollection<IngredientDTO> Items { get; set; } = new();
        private OrderDTO? _currentOrder;
        private ProductDTO? _currentProduct;
        private IngredientTypeDTO? _selectedType;

        #endregion

        #region ������������/�����������

        public PageIngredients(SessionData? sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            // ������������� ����������� �������
            this.ccvItems.SetEditCommand<IngredientDTO>(this.OnEditIngredient);
            this.ccvItems.SetDeleteCommand<IngredientDTO>(this.OnDeleteIngredient);
            this.ccvItems.SetItemSelectedCommand<IngredientDTO>(this.OnIngredientSelected);

            // ���������, �������� �� ������ ������ � ������� � ����������
            if (this.SessionData?.Data is { } dataObject &&
                dataObject.GetType().GetProperty("Order") != null &&
                dataObject.GetType().GetProperty("Product") != null &&
                dataObject.GetType().GetProperty("SelectedType") != null)
            {
                this._currentOrder = dataObject.GetType().GetProperty("Order")?.GetValue(dataObject) as OrderDTO;
                this._currentProduct = dataObject.GetType().GetProperty("Product")?.GetValue(dataObject) as ProductDTO;
                this._selectedType = dataObject.GetType().GetProperty("SelectedType")?.GetValue(dataObject) as IngredientTypeDTO;
            }

            if (this.SessionData?.Data?.GetType().GetProperty("Ingredients") != null)
            {
                this.Items.UpdateObservableCollection(
                    this.SessionData.Data.GetType().GetProperty("Ingredients")?.GetValue(this.SessionData.Data) as List<IngredientDTO> ?? new());
            }

            if (this.SessionData?.Data?.GetType().GetProperty("Allowed") != null)
            {
                this._allowedIngredients = this.SessionData.Data.GetType().GetProperty("Allowed")?.GetValue(this.SessionData.Data) as List<AllowedIngredientsDTO> ?? new();
            }

        }

        #endregion

        #region ������

        /// <summary>
        /// ��������� ��������� ������������, �������� �� ���������.
        /// </summary>
        private async void UpdateItemsCollection()
        {
            // ��������� ��� ����������� � ��������� �� ��������� ���������
            List<IngredientDTO> allIngredients = await this.ApiClient.GetAllIngredientsAsync();
            foreach (IngredientDTO item in allIngredients)
            {
                item.IdIngredientType = item.IngredientType == null ? null : item.IngredientType.Id;
            }
            List<IngredientDTO> filteredIngredients = this._selectedType != null
                ? allIngredients
                    .Where(i =>
                        i.IdIngredientType == this._selectedType.Id &&
                        this._allowedIngredients.Any(a => a.IdIngredient == i.Id))
                    .OrderBy(i => i.Title)
                    .ToList()
                : allIngredients
                    .Where(i => this._allowedIngredients.Any(a => a.IdIngredient == i.Id))
                    .OrderBy(i => i.Title)
                    .ToList();


            this.Items.UpdateObservableCollection(filteredIngredients);
        }

        #endregion

        #region ����������� �������

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.UpdateItemsCollection();

            // ����������� CollectionView
            this.ccvItems.SetDisplayedFields("Title", "Description");
            this.ccvItems.SetItems(this.Items);

            bool isAdmin = await this.IsUserAdminAsync(this.SessionData.CurrentUser.Id);
            this.ccvItems.IsEditButtonVisible = isAdmin;
            this.ccvItems.IsDeleteButtonVisible = isAdmin;
            this.btnAdd.IsVisible = isAdmin;
        }

        /// <summary>
        /// ���������� ������ �����������.
        /// </summary>
        private async void OnIngredientSelected(IngredientDTO selectedIngredient)
        {
            try
            {
                if (this._currentOrder == null || this._currentProduct == null)
                {
                    await this.DisplayAlert("������", "����� ��� ������� �� ������", "OK");
                    return;
                }

                if (selectedIngredient.IdIngredientType == null)
                {
                    await this.DisplayAlert("������", "��� ���������� ����������� �� ��������", "OK");
                    return;
                }

                // ��������� ������ ������
                List<Common.Classes.DB.OrderDetailsView> orderDetails =
                    await this.ApiClient.GetOrderDetailsByIdAsync(this._currentOrder.Id);

                // ���������, �� ��� �� ��� �������� ���������� ������ ����
                bool sameTypeExists = orderDetails.Any(i =>
                    i.ProductId == this._currentProduct.Id &&
                    i.IngredientTypeId == selectedIngredient.IdIngredientType);

                if (sameTypeExists)
                {
                    string typeTitle = selectedIngredient.IngredientType?.Title ?? "����� ����";
                    await this.DisplayAlert("������", $"�� ��� �������� ���������� ���� \"{typeTitle}\"", "OK");
                    return;
                }

                // ��������� ����������
                var orderIngredient = new OrderItemIngredientDTO
                {
                    IdOrderProduct = this._currentProduct.Id,
                    IdIngredient = selectedIngredient.Id,
                    Amount = 1
                };

                bool success = await this.ApiClient.AddIngredientToOrderAsync(
                    this._currentOrder.Id,
                    this._currentProduct.Id,
                    orderIngredient);

                if (!success)
                {
                    await this.DisplayAlert("������", "�� ������� �������� ����������", "OK");
                    return;
                }

                await this.DisplayAlert("�����", "���������� �������� � �����", "OK");

                var newSessionData = new SessionData
                {
                    CurrentUser = this.SessionData?.CurrentUser,
                    Data = new { Order = this._currentOrder, Product = this._currentProduct }
                };

                await this.Navigation.PushAsync(new PageProductCustomer(newSessionData));
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("������", $"�� ������� ���������� ����������: {ex.Message}", "OK");
            }
        }


        /// <summary>
        /// ���������� ������� ������ �������������� �����������.
        /// </summary>
        private async void OnEditIngredient(IngredientDTO ingredient)
        {
            try
            {
                if (this.SessionData == null || this.SessionData.CurrentUser == null)
                {
                    throw new InvalidOperationException(CommonLocal.Strings.ErrorMessages.SessionDataUserNotSet);
                }
                var editSessionData = new SessionData
                {
                    CurrentUser = this.SessionData.CurrentUser,
                    Mode = WindowMode.Update,
                    Data = ingredient
                };
                await this.Navigation.PushAsync(new PageIngredientEdit(editSessionData));
            }
            catch (Exception ex)
            {
                _ = this.ShowError(ex);
            }
        }

        /// <summary>
        /// ���������� ������� ������ �������� �����������.
        /// </summary>
        private async void OnDeleteIngredient(IngredientDTO ingredient)
        {
            bool confirm = await this.DisplayAlert("�������������", $"������� ���������� \"{ingredient.Title}\"?", "��", "���");
            if (!confirm)
            {
                return;
            }

            try
            {
                await this.ApiClient.DeleteIngredientAsync(ingredient.Id);
                await this.DisplayAlert("�����", "���������� �����.", "OK");
                this.UpdateItemsCollection();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("������", $"�� ������� ������� ����������: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// ���������� ������� ������ ���������� ������ �����������.
        /// </summary>
        private async void OnAddIngredientClicked(object sender, EventArgs e)
        {
            try
            {
                if (this.SessionData == null || this.SessionData.CurrentUser == null)
                {
                    throw new InvalidOperationException(CommonLocal.Strings.ErrorMessages.SessionDataUserNotSet);
                }
                if (this._currentOrder != null)
                {
                    throw new InvalidOperationException("No order selected");
                }
                var newSessionData = new SessionData
                {
                    CurrentUser = this.SessionData.CurrentUser,
                    Mode = WindowMode.Create
                };
                await this.Navigation.PushAsync(new PageIngredientEdit(newSessionData));
            }
            catch (Exception ex)
            {
                _ = this.ShowError(ex);
            }
        }

        #endregion
    }
}
