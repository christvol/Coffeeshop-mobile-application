using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes;
using System.Timers;
using Timer = System.Timers.Timer;
namespace Mobile_application.Pages;

public partial class PageSMSCodeVerification : CustomContentPage
{
    #region Поля    
    private readonly string phoneNumber;
    private string code;
    private readonly bool isCodeCorrect;
    private readonly int countdownDefault = 60;
    private int countdown = 60;
    private Timer countdownTimer;

    #endregion

    #region Свойства
    public string PhoneNumber => this.phoneNumber;
    #endregion

    #region Методы

    /// <summary>
    /// Запуск таймера обратного отсчета.
    /// </summary>
    private void StartCountdown()
    {
        this.countdownTimer = new Timer(1000); // 1 секунда
        this.countdownTimer.Elapsed += this.OnCountdownElapsed;
        this.countdownTimer.Start();
    }

    /// <summary>
    /// Остановка и удаление таймера.
    /// </summary>
    private void StopCountdown()
    {
        if (this.countdownTimer != null)
        {
            this.countdownTimer.Stop();
            this.countdownTimer.Elapsed -= this.OnCountdownElapsed;
            this.countdownTimer.Dispose();
            this.countdownTimer = null;
        }
    }

    /// <summary>
    /// Получение кода подтверждения через API.
    /// </summary>
    private async void GetCode()
    {
        try
        {

            var response = await this.ApiClient.GetCodeAsync(this.PhoneNumber);
            this.code = response.Code;

            if (response != null)
            {
                await this.DisplayAlert(CommonLocal.DialogTitles.Information,
                    string.Format(CommonLocal.Strings.SuccessMessages.CodeFetched, this.code), "OK");
                if (App.IsDebugMode)
                {
                    this.entrySMSCode.Text = this.code;
                }
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert(CommonLocal.DialogTitles.Error,
                string.Format(CommonLocal.Strings.ErrorMessages.CodeFetchFailed, ex.Message), "OK");
        }
    }


    /// <summary>
    /// Проверка введенного кода.
    /// </summary>Phone number must be exactly 10 digits
    private async Task ValidateCodeAsync(string enteredCode)
    {
        if (enteredCode.Length < this.code.Length)
            return;
        if (string.IsNullOrEmpty(enteredCode))
        {
            await this.DisplayAlert(CommonLocal.DialogTitles.Error, CommonLocal.Strings.ErrorMessages.EmptyCode, "OK");
            return;
        }

        if (enteredCode == this.code)
        {
            try
            {
                this.StopCountdown();
                await this.DisplayAlert(CommonLocal.DialogTitles.Success, CommonLocal.Strings.SuccessMessages.CodeVerified, "OK");



                // Получаем пользователя по номеру телефона
                var user = await this.ApiClient.GetUserByPhoneNumberAsync(this.phoneNumber.Trim());
                if (user == null)
                {
                    // Если пользователь не найден, создаем его
                    var newUser = new UserRequestDto
                    {
                        PhoneNumber = this.phoneNumber.Trim(),
                        IdUserType = CommonLocal.DefaultUserData.IdUserType,
                        FirstName = CommonLocal.DefaultUserData.FirstName,
                        LastName = CommonLocal.DefaultUserData.LastName,
                        BirthDate = DateTime.UtcNow.AddYears(CommonLocal.DefaultUserData.BirthYearOffset),
                        Email = CommonLocal.DefaultUserData.Email
                    };

                    user = await this.ApiClient.CreateUserAsync(newUser);

                    if (user == null)
                    {
                        await this.DisplayAlert(CommonLocal.DialogTitles.Error, CommonLocal.Strings.ErrorMessages.UserCreationFailed, "OK");
                        return;
                    }
                }

                // Получаем тип пользователя
                var userType = await this.ApiClient.GetUserTypeByUserIdAsync(user.Id);
                if (userType == null)
                {
                    await this.DisplayAlert(CommonLocal.DialogTitles.Error, CommonLocal.Strings.ErrorMessages.UserTypeNotFound, "OK");
                    return;
                }

                // Формируем SessionData
                this.SessionData = new SessionData
                {
                    CurrentUser = user,
                    Data = null, // Можно передать дополнительные данные, если нужно
                    Mode = WindowMode.Read
                };

                // Переход на соответствующую страницу в зависимости от типа пользователя
                switch (userType.Title)
                {
                    case CommonLocal.UserTypes.Customer:
                        await this.Navigation.PushAsync(new PageMainCustomer(this.SessionData));
                        break;
                    case CommonLocal.UserTypes.Employee:
                    case CommonLocal.UserTypes.Admin:
                        await this.Navigation.PushAsync(new PageMainEmployee(this.SessionData));
                        break;
                    default:
                        await this.DisplayAlert(CommonLocal.DialogTitles.Error, CommonLocal.Strings.ErrorMessages.UnknownUserType, "OK");
                        break;
                }
            }
            catch (Exception ex)
            {
                await this.DisplayAlert(CommonLocal.DialogTitles.Error,
                    string.Format(CommonLocal.Strings.ErrorMessages.DataProcessingError, ex.Message), "OK");
            }
        }
        else
        {
            await this.DisplayAlert(CommonLocal.DialogTitles.Error, CommonLocal.Strings.ErrorMessages.InvalidCode, "OK");
        }
    }





    #endregion

    #region Конструкторы/Деструкторы

    public PageSMSCodeVerification(string phoneNumber)
    {
        this.InitializeComponent();
        this.phoneNumber = phoneNumber;

        // Установить отображение номера телефона
        this.lblPhoneNumber.Text = this.phoneNumber;
        if (App.IsDebugMode)
        {
            this.countdownDefault = 3;
            this.countdown = this.countdownDefault;
        }
        // Запустить таймер и запросить код
        this.StartCountdown();
        this.GetCode();
    }

    #endregion

    #region Обработчики событий

    protected override void OnAppearing()
    {
        base.OnAppearing();

    }

    /// <summary>
    /// Обработка таймера обратного отсчета.
    /// </summary>
    private void OnCountdownElapsed(object sender, ElapsedEventArgs e)
    {
        this.countdown--;

        // Обновление UI из потока
        MainThread.BeginInvokeOnMainThread(() =>
        {
            this.lblTimer.Text = TimeSpan.FromSeconds(this.countdown).ToString(@"mm\:ss");

            if (this.countdown <= 0)
            {
                this.StopCountdown();
                this.btnSendCode.IsEnabled = true;
                //btnSendCode.BackgroundColor = Colors.Azure;
                this.lblTimer.Text = "00:00";
            }
        });
    }

    /// <summary>
    /// Обработка нажатия кнопки "Повторить отправку".
    /// </summary>
    private async void OnSendButtonClicked(object sender, EventArgs e)
    {
        try
        {
            this.btnSendCode.IsEnabled = false;
            this.countdown = this.countdownDefault;
            this.StartCountdown();
            this.GetCode();
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось отправить код: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Обработка изменения текста в поле ввода кода.
    /// </summary>
    private async void OnEntrySMSCodeTextChanged(object sender, TextChangedEventArgs e)
    {
        await this.ValidateCodeAsync(e.NewTextValue);
    }

    /// <summary>
    /// Завершение страницы: остановка таймера.
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        this.StopCountdown();
    }

    #endregion
}
