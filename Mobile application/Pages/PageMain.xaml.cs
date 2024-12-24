namespace Мобильное_приложение.Pages;

public partial class PageMain : ContentPage
{
    public PageMain()
    {
        InitializeComponent();
        NavigationPage.SetHasBackButton(this, false);
    }
    private async void OnAccountButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PageUserProfile());
    }

}