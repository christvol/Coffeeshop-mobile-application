using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages;

public partial class PageIngredients : CustomContentPage
{
    #region ��������

    public ObservableCollection<IngredientDTO> items { get; set; } = new();

    #endregion

    #region ������������/�����������

    public PageIngredients(SessionData? sessionData) : base(sessionData)
    {
        this.InitializeComponent();
        this.BindingContext = this;

        // ������������� ����������� �������
        this.ccvItems.SetEditCommand<IngredientDTO>(this.OnEditIngredient);
        this.ccvItems.SetDeleteCommand<IngredientDTO>(this.OnDeleteIngredient);
    }

    #endregion

    #region ������

    /// <summary>
    /// ��������� ��������� ������������.
    /// </summary>
    private async void UpdateItemsCollection()
    {
        this.items.UpdateObservableCollection(await this.ApiClient.GetAllIngredientsAsync());
    }

    #endregion

    #region ����������� �������

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.UpdateItemsCollection();

        // ����������� CollectionView
        this.ccvItems.SetDisplayedFields("Title", "Description");
        this.ccvItems.SetItems(this.items);
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
        _ = this.DisplayAlert("OnDeleteIngredient", "���������� ��������", "OK");
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

    #endregion
}
