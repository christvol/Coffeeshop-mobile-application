using Common.Classes.Session;

namespace Mobile_application.Pages;

public partial class PageAbout : CustomContentPage
{
    private const string StorageFileName = "adminText.txt";

    public PageAbout(SessionData sessionData) : base(sessionData)
    {
        this.InitializeComponent();
        _ = this.CheckAdminAccessAndShowEditorAsync(); // ⬅️ запускаем проверку
    }

    private async Task CheckAdminAccessAndShowEditorAsync()
    {
        if (this.SessionData?.CurrentUser == null)
        {
            return;
        }

        bool isAdmin = await this.IsUserAdminAsync(this.SessionData.CurrentUser.Id);
        this.adminPanel.IsVisible = isAdmin;
        this.txtAdminEditor.IsEnabled = isAdmin;
    }

    private const string AboutFilePath = "Pages/about.txt";

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = this.CheckAdminAccessAndShowEditorAsync();
        _ = this.LoadAboutTextAsync(); // Загружаем текст при появлении
    }

    private async Task LoadAboutTextAsync()
    {
        try
        {
            string path = Path.Combine(AppContext.BaseDirectory, AboutFilePath);
            if (File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                this.txtAdminEditor.Text = content;
            }
            else
            {
                this.txtAdminEditor.Text = "";
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось загрузить текст: {ex.Message}", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        _ = await this.Navigation.PopAsync(); // ⬅️ Возврат на предыдущую страницу
    }

    private async void OnLoadTextClicked(object sender, EventArgs e)
    {
        try
        {
            string path = Path.Combine(AppContext.BaseDirectory, AboutFilePath);
            if (File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                this.txtAdminEditor.Text = content;
            }
            else
            {
                await this.DisplayAlert("Информация", "Файл about.txt не найден", "OK");
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось загрузить текст: {ex.Message}", "OK");
        }
    }

    private async void OnSaveTextClicked(object sender, EventArgs e)
    {
        try
        {
            string path = Path.Combine(AppContext.BaseDirectory, AboutFilePath);
            await File.WriteAllTextAsync(path, this.txtAdminEditor.Text ?? "");
            await this.DisplayAlert("Успех", "Описание сохранено", "OK");
            await Application.Current.MainPage.Navigation.PushAsync(new PageUserProfile(this.SessionData));
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось сохранить текст: {ex.Message}", "OK");
        }
    }

}
