using Common.Classes.DB;
using Mobile_application.Classes.API;
using System.Text.RegularExpressions;
using static Mobile_application.Classes.CommonLocal;

namespace Mobile_application.Pages;
/// <summary>
/// Страница авторизации
/// </summary>
public partial class PageLogin : ContentPage
{
    #region Поля
    /// <summary>
    /// Коды стран
    /// </summary>
    private List<CountryCodes> _countries = new();
    #endregion

    #region Методы

    #region PickerPhoneCodes - выпадающий список для кодов стран

    /// <summary>
    /// Устанавливает выбранный элемент в выпадающем списке (Picker) для кодов стран 
    /// на основе переданного названия страны.
    /// </summary>
    /// <param name="countryName">Название страны, которое нужно выбрать в списке.</param>
    /// <remarks>
    /// Если список стран (_countries) пуст или равен null, метод завершает выполнение.
    /// Поиск выполняется без учета регистра символов. Если страна найдена, она устанавливается
    /// как выбранный элемент (SelectedItem) для <see cref="pckrPhoneCode"/>.
    /// </remarks>
    /// <example>
    /// Пример использования:
    /// <code>
    /// PickerPhoneCodesSelectCountryByName("Russia");
    /// </code>
    /// Если страна с названием "Russia" найдена, она будет выбрана в Picker.
    /// </example>
    private void PickerPhoneCodesSelectCountryByName(string countryName)
    {
        if (this._countries == null || this._countries.Count == 0)
        {
            return;
        }

        var selectedCountry = this._countries.FirstOrDefault(c =>
            c.CountryName.Equals(countryName, StringComparison.OrdinalIgnoreCase));

        if (selectedCountry != null)
        {
            this.pckrPhoneCode.SelectedItem = selectedCountry;
        }
    }

    /// <summary>
    /// Загружает список стран с API.
    /// </summary>
    private async Task PickerPhoneCodesUpdateAsync()
    {
        try
        {
            var apiClient = new ApiClient(API.entryPoint);
            this._countries = await apiClient.GetCountryCodesAsync();

            if (this._countries == null || this._countries.Count == 0)
            {
                await this.DisplayAlert("Информация", "Список стран пуст или не получен.", "OK");
                return;
            }

            this.pckrPhoneCode.ItemsSource = this._countries;
            this.pckrPhoneCode.ItemDisplayBinding = new Binding("CountryName");
            if (App.IsDebugMode)
            {
                this.PickerPhoneCodesSelectCountryByName("Russia");
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось загрузить список стран: {ex.Message}", "OK");
        }
    }

    #endregion

    #region EntryPhone - текстовое  поле для номера телефона
    /// <summary>
    /// Валидирует номер телефона.
    /// Формат: начинается с "+" и может содержать дефисы, пробелы и цифры, минимальная длина 11, максимальная 20.
    /// </summary>
    private bool ValidatePhone(string phone)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return false;
        }

        // Регулярное выражение: +цифры (с пробелами/дефисами), длина от 11 до 20 символов
        var regex = new Regex(@"^\+\d{1,}(\s)?(\d{1,4}([-\s]?\d{1,4}){1,4})$");
        return regex.IsMatch(phone) && phone.Length >= 11 && phone.Length <= 20;
    }

    /// <summary>
    /// Извлекает код страны из текста.
    /// </summary>
    private string ExtractCountryCode(string input)
    {
        return input.StartsWith("+") ? input.Split(' ').FirstOrDefault() ?? string.Empty : string.Empty;
    }

    /// <summary>
    /// Форматирует номер телефона, удаляя лишние символы.
    /// </summary>
    private string ExtractPhoneNumber(string input)
    {
        if (input.StartsWith("+"))
        {
            int spaceIndex = input.IndexOf(' ');
            return spaceIndex > 0 ? input.Substring(spaceIndex + 1) : string.Empty;
        }
        return input;
    }

    /// <summary>
    /// Форматирует номер телефона, добавляя дефисы и пробелы.
    /// </summary>
    private string FormatPhoneNumber(string input)
    {
        // Удаляем все символы, кроме цифр
        input = new string(input.Where(char.IsDigit).ToArray());

        if (input.Length <= 3)
        {
            return input;
        }

        if (input.Length <= 6)
        {
            return $"{input.Substring(0, 3)}-{input.Substring(3)}";
        }

        if (input.Length <= 10)
        {
            return $"{input.Substring(0, 3)}-{input.Substring(3, 3)}-{input.Substring(6)}";
        }

        // Формат с кодом страны
        if (input.Length > 10)
        {
            return $"{input.Substring(0, 3)}-{input.Substring(3, 3)}-{input.Substring(6, 4)}";
        }
        return input;
    }

    /// <summary>
    /// Обновляет текст в поле номера телефона.
    /// </summary>
    private void EntryPhoneUpdateText(string newText)
    {
        try
        {
            string countryCode = this.ExtractCountryCode(newText);
            string phoneNumber = this.ExtractPhoneNumber(newText);

            // Форматируем номер телефона
            // string formattedPhoneNumber = FormatPhoneNumber(phoneNumber);
            string formattedPhoneNumber = phoneNumber;
            // Объединяем код страны и номер
            string finalText = string.IsNullOrEmpty(countryCode)
                ? formattedPhoneNumber
                : $"{countryCode} {formattedPhoneNumber}";
            // Замена дефисов на пробелы (опционально)
            finalText = finalText.Replace('-', ' ');
            if (finalText.Length > 20)
            {
                finalText = finalText.Substring(0, 20);
            }

            if (this.entryPhone.Text != finalText)
            {
                this.entryPhone.TextChanged -= this.OnEntryPhoneTextChanged;
                this.entryPhone.Text = finalText;
                this.entryPhone.TextChanged += this.OnEntryPhoneTextChanged;
            }
        }
        catch (Exception ex)
        {
            this.DisplayAlert("Ошибка", $"Ошибка при обработке номера телефона: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Обновляет текст номера телефона при выборе страны.
    /// </summary>
    private void EntryPhoneUpdateText(CountryCodes selectedCountry)
    {
        try
        {
            // Получаем текущий текст из Entry
            string currentText = this.entryPhone.Text ?? string.Empty;

            // Извлекаем номер телефона без кода страны
            string phoneNumber = this.ExtractPhoneNumber(currentText);

            // Обновляем текст
            this.entryPhone.Text = $"{selectedCountry.CountryCode} {phoneNumber}";
        }
        catch (Exception ex)
        {
            this.DisplayAlert("Ошибка", $"Не удалось обновить номер телефона: {ex.Message}", "OK");
        }
    }

    #endregion

    #endregion

    #region Конструкторы/Деструкторы

    public PageLogin()
    {
        this.InitializeComponent();
    }

    #endregion

    #region Обработчики событий

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Загружаем данные только при первом открытии
        if (this._countries == null || this._countries.Count == 0)
        {
            _ = this.PickerPhoneCodesUpdateAsync();
            if (App.IsDebugMode)
            {
                this.entryPhone.Text = "9111789930";
            }
            this.btnLogin.IsEnabled = this.ValidatePhone(this.entryPhone.Text);
        }

    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.pckrPhoneCode.SelectedItem is CountryCodes selectedCountry)
        {
            this.EntryPhoneUpdateText(selectedCountry);
        }
    }

    private void OnEntryPhoneTextChanged(object sender, TextChangedEventArgs e)
    {
        this.EntryPhoneUpdateText(e.NewTextValue);
        this.btnLogin.IsEnabled = this.ValidatePhone(this.entryPhone.Text);
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        try
        {
            string phoneNumber = this.entryPhone.Text; // Получаем текст из поля ввода

            if (string.IsNullOrEmpty(phoneNumber))
            {
                this.DisplayAlert("Ошибка", "Пожалуйста, введите номер телефона.", "OK");
                return;
            }

            // Передаем номер телефона на следующую страницу
            await this.Navigation.PushAsync(new PageSMSCodeVerification(this.entryPhone.Text));
        }
        catch (Exception ex)
        {
            _ = this.DisplayAlert("Ошибка", $"Не удалось открыть экран: {ex.Message}", "OK");
        }
    }

    #endregion


}
