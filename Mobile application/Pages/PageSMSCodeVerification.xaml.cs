using System.Timers;
using Mobile_application.Classes;
using Mobile_application.Classes.API;
using Timer = System.Timers.Timer;

namespace Mobile_application.Pages;

public partial class PageSMSCodeVerification : ContentPage
{
    #region Поля    
    private readonly string phoneNumber;
    private string code;
    private bool isCodeCorrect;
    private int countdownDefault = 60;
    private int countdown = 60;
    private Timer countdownTimer;

    #endregion

    #region Свойства
    public string PhoneNumber => phoneNumber;
    #endregion

    #region Методы

    /// <summary>
    /// Запуск таймера обратного отсчета.
    /// </summary>
    private void StartCountdown()
    {
        countdownTimer = new Timer(1000); // 1 секунда
        countdownTimer.Elapsed += OnCountdownElapsed;
        countdownTimer.Start();
    }

    /// <summary>
    /// Остановка и удаление таймера.
    /// </summary>
    private void StopCountdown()
    {
        if (countdownTimer != null)
        {
            countdownTimer.Stop();
            countdownTimer.Elapsed -= OnCountdownElapsed;
            countdownTimer.Dispose();
            countdownTimer = null;
        }
    }

    /// <summary>
    /// Получение кода подтверждения через API.
    /// </summary>
    private async void GetCode()
    {
        try
        {
            var apiClient = new ApiClient(Common.API.entryPoint);
            var response = await apiClient.RegisterAsync(PhoneNumber);
            code = response.Code;

            if (response != null)
            {
                await DisplayAlert("Информация", $"Код подтверждения: {code}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось получить код: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Проверка введенного кода.
    /// </summary>
    private async Task ValidateCodeAsync(string enteredCode)
    {
        if (enteredCode == code)
        {
            StopCountdown();
            await DisplayAlert("Успех", "Код подтвержден!", "OK");
            // Логика успешной проверки, например, переход на следующую страницу
            await Navigation.PushAsync(new PageMain());
        }
        else if (enteredCode.Length == 4) // Проверяем, только если введено 4 символа
        {
            await DisplayAlert("Ошибка", "Неверный код. Попробуйте еще раз.", "OK");
        }
    }

    #endregion

    #region Конструкторы/Деструкторы

    public PageSMSCodeVerification(string phoneNumber)
    {
        InitializeComponent();
        this.phoneNumber = phoneNumber;

        // Установить отображение номера телефона
        lblPhoneNumber.Text = this.phoneNumber;
        if (App.IsDebugMode)
        {
            countdownDefault = 3;
            countdown = countdownDefault;
        }
        // Запустить таймер и запросить код
        StartCountdown();
        GetCode();
    }

    #endregion

    #region Обработчики событий

    /// <summary>
    /// Обработка таймера обратного отсчета.
    /// </summary>
    private void OnCountdownElapsed(object sender, ElapsedEventArgs e)
    {
        countdown--;

        // Обновление UI из потока
        MainThread.BeginInvokeOnMainThread(() =>
        {
            lblTimer.Text = TimeSpan.FromSeconds(countdown).ToString(@"mm\:ss");

            if (countdown <= 0)
            {
                StopCountdown();
                btnSendCode.IsEnabled = true;
                btnSendCode.BackgroundColor = Colors.Azure;
                lblTimer.Text = "00:00";
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
            btnSendCode.IsEnabled = false;
            countdown = countdownDefault;
            StartCountdown();
            GetCode();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", $"Не удалось отправить код: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Обработка изменения текста в поле ввода кода.
    /// </summary>
    private async void OnEntrySMSCodeTextChanged(object sender, TextChangedEventArgs e)
    {
        await ValidateCodeAsync(e.NewTextValue);
    }

    /// <summary>
    /// Завершение страницы: остановка таймера.
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        StopCountdown();
    }

    #endregion
}
