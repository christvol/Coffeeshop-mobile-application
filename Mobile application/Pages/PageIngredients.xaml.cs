using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages;

public partial class PageIngredients : CustomContentPage
{
    #region Свойства

    public ObservableCollection<IngredientDTO> items { get; set; } = new();

    #endregion

    #region Конструкторы/Деструкторы

    public PageIngredients(SessionData? sessionData) : base(sessionData)
    {
        this.InitializeComponent();
        this.BindingContext = this;

        // Устанавливаем обработчики событий
        this.ccvItems.SetEditCommand<IngredientDTO>(this.OnEditIngredient);
        this.ccvItems.SetDeleteCommand<IngredientDTO>(this.OnDeleteIngredient);
    }

    #endregion

    #region Методы

    /// <summary>
    /// Обновляет коллекцию ингредиентов.
    /// </summary>
    private async void UpdateItemsCollection()
    {
        this.items.UpdateObservableCollection(await this.ApiClient.GetAllIngredientsAsync());
    }

    #endregion

    #region Обработчики событий

    protected override void OnAppearing()
    {
        base.OnAppearing();
        this.UpdateItemsCollection();

        // Настраиваем CollectionView
        this.ccvItems.SetDisplayedFields("Title", "Description");
        this.ccvItems.SetItems(this.items);
    }

    /// <summary>
    /// Обработчик нажатия кнопки редактирования ингредиента.
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
    /// Обработчик нажатия кнопки удаления ингредиента.
    /// </summary>
    private async void OnDeleteIngredient(IngredientDTO ingredient)
    {
        _ = this.DisplayAlert("OnDeleteIngredient", "Обработчик удаления", "OK");
        bool confirm = await this.DisplayAlert("Подтверждение", $"Удалить ингредиент \"{ingredient.Title}\"?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        try
        {
            await this.ApiClient.DeleteIngredientAsync(ingredient.Id);
            await this.DisplayAlert("Успех", "Ингредиент удалён.", "OK");
            this.UpdateItemsCollection();
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось удалить ингредиент: {ex.Message}", "OK");
        }
    }

    #endregion
}
