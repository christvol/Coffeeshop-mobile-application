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

        // Обработчик для кнопки добавления нового товара
        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            // Создаем объект SessionData для режима Create
            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData.CurrentUser,
                Mode = WindowMode.Create, // Режим создания
                Data = new ProductDTO // Новый объект продукта
                {
                    Title = string.Empty,
                    Description = string.Empty,
                    Fee = 0,
                    IdProductType = this.Category?.Id ?? 0 // Привязываем к текущей категории
                }
            };

            // Переходим на страницу редактирования
            await this.Navigation.PushAsync(new PageProductEdit(newSessionData));
        }

        // Обработчик для кнопки редактирования товара
        private async void OnEditProductClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ProductDTO product)
            {
                // Создаем объект SessionData для режима Update
                var editSessionData = new SessionData
                {
                    CurrentUser = this.SessionData.CurrentUser,
                    Mode = WindowMode.Update, // Режим обновления
                    Data = product // Передаем выбранный продукт для редактирования
                };

                // Переходим на страницу редактирования
                await this.Navigation.PushAsync(new PageProductEdit(editSessionData));
            }
        }

        // Обработчик для кнопки удаления товара
        private async void OnDeleteProductClicked(object sender, EventArgs e)
        {
            // Получаем выбранный продукт
            if (sender is Button button && button.BindingContext is ProductDTO product)
            {
                var confirm = await this.DisplayAlert("Подтверждение", $"Вы уверены, что хотите удалить продукт \"{product.Title}\"?", "Да", "Нет");
                if (!confirm)
                {
                    return;
                }

                try
                {
                    // Удаляем продукт через API
                    await this.ApiClient.DeleteProductAsync(product.Id);

                    // Удаляем продукт из локального списка
                    _ = this.Products.Remove(product);

                    await this.DisplayAlert("Успех", "Продукт успешно удален.", "OK");
                }
                catch (Exception ex)
                {
                    await this.DisplayAlert("Ошибка", $"Не удалось удалить продукт: {ex.Message}", "OK");
                }
            }
        }


    }
}
