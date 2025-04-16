using Common.Classes.Session;

namespace Mobile_application.Pages;

public partial class PageContacts : CustomContentPage
{
    private const string ContactsFilePath = "Pages/contacts.txt";

    public PageContacts(SessionData sessionData) : base(sessionData)
    {
        this.InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = this.CheckAdminAccessAndShowEditorAsync();
        _ = this.LoadContactsTextAsync();
    }

    private async Task CheckAdminAccessAndShowEditorAsync()
    {
        if (this.SessionData?.CurrentUser == null)
        {
            return;
        }

        bool isAdmin = await this.IsUserAdminAsync(this.SessionData.CurrentUser.Id);
        this.adminPanel.IsVisible = isAdmin;
        this.txtContactsEditor.IsEnabled = isAdmin;
    }

    private async Task LoadContactsTextAsync()
    {
        try
        {
            string path = Path.Combine(AppContext.BaseDirectory, ContactsFilePath);
            if (File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                this.txtContactsEditor.Text = content;
            }
            else
            {
                this.txtContactsEditor.Text = "";
            }
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось загрузить контакты: {ex.Message}", "OK");
        }
    }

    private async void OnSaveContactsClicked(object sender, EventArgs e)
    {
        try
        {
            string path = Path.Combine(AppContext.BaseDirectory, ContactsFilePath);
            await File.WriteAllTextAsync(path, this.txtContactsEditor.Text ?? "");
            await this.DisplayAlert("Успех", "Контакты сохранены", "OK");
            await Application.Current.MainPage.Navigation.PushAsync(new PageUserProfile(this.SessionData));
        }
        catch (Exception ex)
        {
            await this.DisplayAlert("Ошибка", $"Не удалось сохранить: {ex.Message}", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        _ = await this.Navigation.PopAsync();
    }
}
