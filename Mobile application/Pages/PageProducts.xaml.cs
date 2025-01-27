using Common.Classes.DB;
using Common.Classes.Session;

namespace Mobile_application.Pages;

public partial class PageProducts : CustomContentPage
{
    public ProductTypes Category
    {
        set; get;
    }

    public PageProducts(SessionData sessionData)
    {
        this.InitializeComponent();
        this.SessionData = sessionData;
        this.Category = sessionData.Data as ProductTypes;

        // Используйте данные для настройки интерфейса
        this.Title = $"Продукты категории: {this.Category?.Title ?? "Неизвестно"}";
    }
}