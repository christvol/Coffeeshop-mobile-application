using Common.Classes.DB;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
using REST_API_SERVER.DTOs;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageProducts : CustomContentPage
    {
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
            this.Category = this.SessionData.Data as ProductTypes;

            // Устанавливаем обработчики событий
            this.ccvItems.SetEditCommand<ProductDTO>(this.OnEditProductClicked);
            this.ccvItems.SetDeleteCommand<ProductDTO>(this.OnDeleteProductClicked);
        }

        #endregion

        #region Методы

        /// <summary>
        /// Загружает список продуктов.
        /// </summary>
        private async void LoadProducts()
        {
            try
            {
                List<ProductDTO> products;
                // If a category is provided, load products of that category; otherwise, load all products.
                if (this.Category != null)
                {
                    this.Title = $"Products in category: {this.Category.Title}";
                    products = await this.ApiClient.GetProductsByTypeAsync(this.Category.Id);
                }
                else
                {
                    this.Title = "All products";
                    products = await this.ApiClient.GetAllProductsAsync();
                }


                this.Products.UpdateObservableCollection(products);
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось загрузить продукты: {ex.Message}", "OK");
            }
        }

        #endregion

        #region Обработчики событий

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.LoadProducts();

            // Настраиваем CollectionView
            this.ccvItems.SetDisplayedFields("Title", "Description", "Fee");
            this.ccvItems.SetItems(this.Products);
        }

        /// <summary>
        /// Обработчик кнопки "Добавить".
        /// </summary>
        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData?.CurrentUser,
                Mode = WindowMode.Create,
                Data = new ProductDTO
                {
                    Title = string.Empty,
                    Description = string.Empty,
                    Fee = 0,
                    IdProductType = this.Category?.Id ?? 0 // Привязываем к текущей категории, если она есть
                }
            };

            await this.Navigation.PushAsync(new PageProductEdit(newSessionData));
        }

        /// <summary>
        /// Обработчик кнопки "Редактировать".
        /// </summary>
        private async void OnEditProductClicked(ProductDTO product)
        {
            var editSessionData = new SessionData
            {
                CurrentUser = this.SessionData?.CurrentUser,
                Mode = WindowMode.Update,
                Data = product
            };

            await this.Navigation.PushAsync(new PageProductEdit(editSessionData));
        }

        /// <summary>
        /// Обработчик кнопки "Удалить".
        /// </summary>
        private async void OnDeleteProductClicked(ProductDTO product)
        {
            bool confirm = await this.DisplayAlert("Удаление", $"Вы уверены, что хотите удалить продукт \"{product.Title}\"?", "Да", "Нет");
            if (!confirm)
            {
                return;
            }

            try
            {
                await this.ApiClient.DeleteProductAsync(product.Id);
                await this.DisplayAlert("Успех", "Продукт удален.", "OK");
                this.LoadProducts();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось удалить продукт: {ex.Message}", "OK");
            }
        }

        #endregion
    }
}
