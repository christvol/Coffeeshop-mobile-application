using Mobile_application.Classes.API;
using System.Timers;
using API = Mobile_application.Classes.Common.API;
using Timer = System.Timers.Timer;
namespace Mobile_application.Pages;

public partial class PageSMSCodeVerification : ContentPage
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
            var apiClient = new ApiClient(API.entryPoint);
            var response = await apiClient.RegisterAsync(this.PhoneNumber);
            this.code = response.Code;

            if (response != null)
            {
                await this.DisplayAlert("Информация", $"Код подтверждения: {this.code}", "OK");
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось получить код: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Проверка введенного кода.
    /// </summary>
    private async Task ValidateCodeAsync(string enteredCode)
    {
        if (enteredCode == this.code)
        {
            this.StopCountdown();
            await this.DisplayAlert("Успех", "Код подтвержден!", "OK");
            var apiClient = new ApiClient(API.entryPoint);
            //TODO: добавить методы в контроллер
            //var user =
            //var userType = apiClient.GetUserTypeByUserIdAsync(1);
            //if (userType.Result.Title == "Admin")
            //{

            //}
            //else
            //{

            //}
            // Логика успешной проверки, например, переход на следующую страницу
            await this.Navigation.PushAsync(new PageMain());
        }
        else if (enteredCode.Length == 4) // Проверяем, только если введено 4 символа
        {
            await this.DisplayAlert("Ошибка", "Неверный код. Попробуйте еще раз.", "OK");
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
