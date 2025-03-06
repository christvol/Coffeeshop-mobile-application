using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageIngredientTypes : CustomContentPage
    {
        #region ��������

        public ObservableCollection<IngredientTypeDTO> IngredientTypes { get; set; } = new();

        #endregion

        #region ������������/�����������

        public PageIngredientTypes(SessionData? sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            // ������������� ����������� �������
            this.ccvItems.SetEditCommand<IngredientTypeDTO>(this.OnEditIngredientType);
            this.ccvItems.SetDeleteCommand<IngredientTypeDTO>(this.OnDeleteIngredientType);
        }

        #endregion

        #region ������

        /// <summary>
        /// ��������� ��������� ����� ������������.
        /// </summary>
        private async void UpdateItemsCollection()
        {
            this.IngredientTypes.UpdateObservableCollection(await this.ApiClient.GetAllIngredientTypesAsync());
        }

        #endregion

        #region ����������� �������

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.UpdateItemsCollection();

            // ����������� CollectionView
            this.ccvItems.SetDisplayedFields("Title");
            this.ccvItems.SetItems(this.IngredientTypes);
        }

        /// <summary>
        /// ���������� ������� ������ �������������� ���� �����������.
        /// </summary>
        private async void OnEditIngredientType(IngredientTypeDTO ingredientType)
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
                    Data = ingredientType
                };
                await this.Navigation.PushAsync(new PageIngredientTypeEdit(editSessionData));
            }
            catch (Exception ex)
            {
                _ = this.ShowError(ex);
            }
        }

        /// <summary>
        /// ���������� ������� ������ �������� ���� �����������.
        /// </summary>
        private async void OnDeleteIngredientType(IngredientTypeDTO ingredientType)
        {
            bool confirm = await this.DisplayAlert("�������������", $"������� ��� ����������� \"{ingredientType.Title}\"?", "��", "���");
            if (!confirm)
            {
                return;
            }

            try
            {
                await this.ApiClient.DeleteIngredientTypeAsync(ingredientType.Id);
                await this.DisplayAlert("�����", "��� ����������� �����.", "OK");
                this.UpdateItemsCollection();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("������", $"�� ������� ������� ��� �����������: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// ���������� ������� ������ ���������� ������ ���� �����������.
        /// </summary>
        private async void OnAddIngredientTypeClicked(object sender, EventArgs e)
        {
            try
            {
                if (this.SessionData == null || this.SessionData.CurrentUser == null)
                {
                    throw new InvalidOperationException(CommonLocal.Strings.ErrorMessages.SessionDataUserNotSet);
                }
                var newSessionData = new SessionData
                {
                    CurrentUser = this.SessionData.CurrentUser,
                    Mode = WindowMode.Create
                };
                await this.Navigation.PushAsync(new PageIngredientTypeEdit(newSessionData));
            }
            catch (Exception ex)
            {
                _ = this.ShowError(ex);
            }
        }


        #endregion
    }
}
