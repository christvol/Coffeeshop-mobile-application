using Common.Classes.DB;
using Common.Classes.Session;
using REST_API_SERVER.DTOs;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageProducts : CustomContentPage
    {
        #region Поля

        #endregion

        #region Свойства

        public ObservableCollection<ProductDTO> Products { get; set; } = new();

        public ProductTypes? Category
        {
            get; set;
        }

        #endregion

        #region Конструкторы/Деструкторы

        public PageProducts(SessionData? sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.SessionData = sessionData ?? new SessionData(); // Гарантируем, что SessionData не будет null

            if (this.SessionData.Data is ProductTypes productType)
            {
                this.Category = productType;
            }
            else
            {
                this.Category = null;
            }

            // Настраиваем заголовок
            this.Title = $"Продукты категории: {this.Category?.Title ?? "Неизвестно"}";
        }


        #endregion

        #region Методы

        private async void LoadProducts()
        {
            if (this.Category == null)
            {
                return;
            }

            try
            {
                // Используем ApiClient для получения продуктов
                List<ProductDTO> products = await this.ApiClient.GetProductsByTypeAsync(this.Category.Id);

                this.Products.Clear();
                foreach (ProductDTO product in products)
                {
                    this.Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось загрузить продукты: {ex.Message}", "OK");
            }
        }

        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Загружаем продукты категории
            this.LoadProducts();
        }

        private async void OnProductSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is ProductDTO selectedProduct)
            {
                // Создаем объект SessionData для режима Read
                var readSessionData = new SessionData
                {
                    CurrentUser = this.SessionData?.CurrentUser,
                    Mode = WindowMode.Read, // Режим чтения
                    Data = selectedProduct // Передаем выбранный продукт
                };

                // Переходим на страницу редактирования в режиме Read
                await this.Navigation.PushAsync(new PageProductEdit(readSessionData));

                // Сбрасываем выбор
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            // Создаем объект SessionData для режима Create
            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData?.CurrentUser,
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

        private async void OnEditProductClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ProductDTO product)
            {
                // Создаем объект SessionData для режима Update
                var editSessionData = new SessionData
                {
                    CurrentUser = this.SessionData?.CurrentUser,
                    Mode = WindowMode.Update, // Режим обновления
                    Data = product // Передаем выбранный продукт для редактирования
                };

                // Переходим на страницу редактирования
                await this.Navigation.PushAsync(new PageProductEdit(editSessionData));
            }
        }

        private async void OnDeleteProductClicked(object sender, EventArgs e)
        {
            // Получаем выбранный продукт
            if (sender is Button button && button.BindingContext is ProductDTO product)
            {
                bool confirm = await this.DisplayAlert("Подтверждение", $"Вы уверены, что хотите удалить продукт \"{product.Title}\"?", "Да", "Нет");
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

        #endregion
    }
}
