using Common.Classes.DB;
using Common.Classes.Session;
using REST_API_SERVER.DTOs;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageProducts : CustomContentPage
    {
        public ObservableCollection<ProductDTO> Products { get; set; } = new();

        public ProductTypes Category
        {
            get; set;
        }

        public PageProducts(SessionData sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.SessionData = sessionData;
            this.Category = sessionData.Data as ProductTypes;

            // Настраиваем заголовок
            this.Title = $"Продукты категории: {this.Category?.Title ?? "Неизвестно"}";

            // Загружаем продукты категории
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            if (this.Category == null)
            {
                return;
            }

            try
            {
                // Используем ApiClient для получения продуктов
                var products = await this.ApiClient.GetProductsByTypeAsync(this.Category.Id);

                this.Products.Clear();
                foreach (var product in products)
                {
                    this.Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось загрузить продукты: {ex.Message}", "OK");
            }
        }
    }
}
