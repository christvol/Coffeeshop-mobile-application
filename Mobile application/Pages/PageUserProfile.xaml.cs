using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes;

namespace Mobile_application.Pages;

public partial class PageUserProfile : CustomContentPage
{

    #region Поля

    #endregion

    #region Свойства

    #endregion

    #region Методы
    private async Task LoadUserDataAsync()
    {
        try
        {
            //this.CheckCurrentUser();
            // Асинхронный запрос на сервер для получения данных пользователя
            Common.Classes.DB.Users? user = await this.ApiClient.GetUserByIdAsync(this.SessionData.CurrentUser.Id);
            if (user != null)
            {
                this.SessionData.CurrentUser = user;



                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Заполнение полей формы данными пользователя
                    this.entryUserFirstName.Text = $"{user.FirstName}";
                    this.entryPhone.Text = user.PhoneNumber;
                    this.entryEmail.Text = user.Email;
                });


                // Дополнительная логика настройки переключателей и других элементов
            }

            // Настройка режима окна (например, отключение/включение полей)
            if (this.SessionData.Mode == WindowMode.Read)
            {
                this.entryUserFirstName.IsEnabled = false;
                this.entryPhone.IsEnabled = false;
                this.entryEmail.IsEnabled = false;
                this.btnSave.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось загрузить данные пользователя: {ex.Message}", "OK");
        }
    }

    #endregion

    #region Конструкторы/Деструкторы
    public PageUserProfile(SessionData sessionData)
    {
        this.InitializeComponent();
        this.SessionData = sessionData;

        // Асинхронная загрузка данных пользователя
        _ = Task.Run(async () => await this.LoadUserDataAsync());
    }

    public PageUserProfile()
    {
        this.InitializeComponent();
    }
    #endregion

    #region Операторы

    #endregion

    #region Обработчики событий
    private void btnLogout_Clicked(object sender, EventArgs e)
    {
        _ = NavigateToPage(new PageLogin());
    }
    private async void btnSave_Clicked(object sender, EventArgs e)
    {
        try
        {
            //this.CheckCurrentUser();
            // Проверка на наличие текущего пользователя
            if (this.SessionData.CurrentUser == null)
            {
                await this.DisplayAlert("Ошибка", "Данные пользователя не найдены.", "OK");
                return;
            }

            // Обновление данных пользователя из полей ввода
            var updatedUser = new UserDTO
            {
                Id = this.SessionData.CurrentUser.Id,
                FirstName = this.entryUserFirstName.Text,
                LastName = this.SessionData.CurrentUser.LastName,
                PhoneNumber = this.entryPhone.Text,
                Email = this.entryEmail.Text,
                BirthDate = this.SessionData.CurrentUser.BirthDate,
                IdUserType = this.SessionData.CurrentUser.IdUserType
            };



            // Отправка обновленных данных на сервер
            bool isUpdated = await this.ApiClient.UpdateUserAsync(updatedUser.Id, updatedUser);

            if (isUpdated)
            {
                // Обновляем данные в сессии
                this.SessionData.CurrentUser.FirstName = updatedUser.FirstName;
                this.SessionData.CurrentUser.LastName = updatedUser.LastName;
                this.SessionData.CurrentUser.PhoneNumber = updatedUser.PhoneNumber;
                this.SessionData.CurrentUser.Email = updatedUser.Email;

                await this.DisplayAlert("Успех", "Данные успешно сохранены.", "OK");

                // Получаем тип пользователя
                UserTypeDTO? userType = await this.ApiClient.GetUserTypeByUserIdAsync(this.SessionData.CurrentUser.Id);
                if (userType == null)
                {
                    await this.DisplayAlert(CommonLocal.DialogTitles.Error, CommonLocal.Strings.ErrorMessages.UserTypeNotFound, "OK");
                    return;
                }
                // Переход на соответствующую страницу в зависимости от типа пользователя
                await this.Navigation.PushAsync(new PageProductTypes(this.SessionData));
            }
            else
            {
                await this.DisplayAlert("Ошибка", "Не удалось обновить данные пользователя.", "OK");
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Произошла ошибка при сохранении данных: {ex.Message}", "OK");
        }
    }
    private void btnAboutApp_Clicked(object sender, EventArgs e)
    {
        _ = NavigateToPage(new PageAbout(this.SessionData));
    }
    private void btnFeedback_Clicked(object sender, EventArgs e)
    {
        _ = NavigateToPage(new PageContacts(this.SessionData));
    }
    #endregion

}
