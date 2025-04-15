using Common.Classes.DB;
using Common.Classes.DTO;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
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
            // Устанавливаем обработчик нажатия на элемент списка
            this.ccvItems.SetItemSelectedCommand<ProductDTO>(this.OnProductSelected);
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.LoadProducts();

            // Настраиваем CollectionView
            this.ccvItems.SetDisplayedFields("Title", "Description");
            this.ccvItems.SetItems(this.Products);

            // Проверяем, является ли пользователь администратором
            bool isAdmin = await this.IsUserAdminAsync(this.SessionData.CurrentUser.Id);
            this.ccvItems.IsListItemEditButtonsVisible = isAdmin;
            this.btnAdd.IsVisible = isAdmin;


        }

        /// <summary>
        /// Обработчик выбора продукта в списке.
        /// </summary>
        private async void OnProductSelected(ProductDTO selectedProduct)
        {
            if (selectedProduct == null || this.SessionData?.CurrentUser == null)
            {
                await this.DisplayAlert("Ошибка", "Не удалось определить пользователя или продукт", "OK");
                return;
            }

            int userId = this.SessionData.CurrentUser.Id;
            OrderDTO? currentOrder = null;

            // Получаем статус "Created"
            OrderStatusDTO? createdStatus = await this.ApiClient.GetOrderStatusByTitleAsync("Created");
            if (createdStatus == null)
            {
                await this.DisplayAlert("Ошибка", "Не удалось получить статус 'Created'", "OK");
                return;
            }

            // Проверяем существующий черновик заказа
            List<OrderDTO> orders = await this.ApiClient.GetOrdersByCustomerIdAsync(userId);
            currentOrder = orders.FirstOrDefault(o => o.IdStatus == createdStatus.Id);

            if (currentOrder == null)
            {
                // Создаём новый заказ
                var newOrder = new OrderDTO
                {
                    IdCustomer = userId,
                    IdStatus = createdStatus.Id,
                    IdStatusPayment = 1, // Not Paid
                    CreationDate = DateTime.UtcNow,
                    OrderItems = new List<OrderItemsDTO>
            {
                new()
                {
                    IdProduct = selectedProduct.Id,
                    Total = selectedProduct.Fee,
                    Ingredients = new List<OrderItemIngredientDTO>()
                }
            }
                };

                currentOrder = await this.ApiClient.CreateOrderAsync(newOrder);

                if (currentOrder == null)
                {
                    await this.DisplayAlert("Ошибка", "Не удалось создать заказ", "OK");
                    return;
                }
            }
            else
            {
                // Добавляем выбранный продукт в существующий заказ
                var newProduct = new OrderProductDTO
                {
                    IdProduct = selectedProduct.Id,
                    Quantity = 1
                };

                bool success = await this.ApiClient.AddProductToOrderAsync(currentOrder.Id, newProduct);

                if (!success)
                {
                    await this.DisplayAlert("Ошибка", "Не удалось добавить продукт в заказ", "OK");
                    return;
                }
            }

            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData.CurrentUser,
                Data = new { Order = currentOrder, Product = selectedProduct },
                Mode = WindowMode.Read
            };

            await this.Navigation.PushAsync(new PageProductCustomer(newSessionData));
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
