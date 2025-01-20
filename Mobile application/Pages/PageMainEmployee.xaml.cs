using Common.Classes.Session;

namespace Mobile_application.Pages;

public partial class PageMainEmployee : CustomContentPage
{

    public PageMainEmployee(SessionData sessionData)
    {
        this.InitializeComponent();
        this.SessionData = sessionData;
        NavigationPage.SetHasBackButton(this, false);
        this.LoadEmployeeData();
        this.BindSessionDataToHeader();
    }

    private void LoadEmployeeData()
    {
        // Используйте _sessionData для настройки страницы
        if (this.SessionData.CurrentUser != null)
        {
            // Например, настройка интерфейса для сотрудника
            this.Title = $"Dashboard: {this.SessionData.CurrentUser.FirstName}";
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