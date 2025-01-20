using Common.Classes.Session;

namespace Mobile_application.Pages;

public partial class PageMainCustomer : CustomContentPage
{
    private readonly SessionData _sessionData;

    public PageMainCustomer(SessionData sessionData)
    {
        this.InitializeComponent();
        this.SessionData = sessionData;
        this.LoadCustomerData();
        this.BindSessionDataToHeader();
    }

    private void LoadCustomerData()
    {
        // Используйте _sessionData для настройки страницы
        if (this.SessionData.CurrentUser != null)
        {
            // Например, загрузите имя пользователя или другую информацию
            this.Title = $"Welcome, {this.SessionData.CurrentUser.FirstName}";
        }
    }

    private void BindSessionDataToHeader()
    {
        // Получение PageHeader из XAML и передача SessionData
        if (this.FindByName<Controls.PageHeader>("PageHeader") is Controls.PageHeader header)
        {
            header.SessionData = this.SessionData;
        }
    }
}