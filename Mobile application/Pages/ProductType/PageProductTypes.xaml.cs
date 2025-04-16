using Common.Classes.DB;
using Common.Classes.Session;
using Mobile_application.Classes.Utils;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageProductTypes : CustomContentPage
    {
        #region Свойства

        public ObservableCollection<ProductTypes> Categories { get; set; } = new();

        #endregion

        #region Конструкторы/Деструкторы

        public PageProductTypes(SessionData? sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            // Устанавливаем обработчики событий
            this.ccvItems.SetEditCommand<ProductTypes>(this.OnEditClicked);
            this.ccvItems.SetDeleteCommand<ProductTypes>(this.OnDeleteClicked);
            this.ccvItems.SetItemSelectedCommand<ProductTypes>(this.OnCategorySelected);
        }

        #endregion

        #region Методы

        /// <summary>
        /// Загружает категории продуктов.
        /// </summary>
        private async void LoadCategoriesAsync()
        {
            try
            {
                this.Categories.UpdateObservableCollection(await this.ApiClient.GetAllProductTypesAsync());
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось загрузить категории: {ex.Message}", "OK");
            }
        }

        #endregion

        #region Обработчики событий

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            this.LoadCategoriesAsync();

            // Настраиваем CollectionView
            this.ccvItems.SetDisplayedFields("Title");
            this.ccvItems.SetItems(this.Categories);

            //TODO: убрать авторизацию
            //this.SessionData.CurrentUser = await this.ApiClient.GetUserByIdAsync(2);

            // Проверяем, является ли пользователь администратором
            bool isAdmin = await this.IsUserAdminAsync(this.SessionData.CurrentUser.Id);
            this.ccvItems.IsEditButtonVisible = isAdmin;
            this.ccvItems.IsDeleteButtonVisible = isAdmin;
            this.btnAdd.IsVisible = isAdmin;



        }

        /// <summary>
        /// Обработчик кнопки "Добавить".
        /// </summary>
        private async void OnAddClicked(object sender, EventArgs e)
        {
            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData?.CurrentUser,
                Mode = WindowMode.Create
            };

            await this.Navigation.PushAsync(new PageProductTypeEdit(newSessionData));
        }

        /// <summary>
        /// Обработчик выбора категории (переход к продуктам).
        /// </summary>
        private async void OnCategorySelected(ProductTypes selectedCategory)
        {
            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData?.CurrentUser,
                Data = selectedCategory
            };

            await this.Navigation.PushAsync(new PageProducts(newSessionData));
        }

        /// <summary>
        /// Обработчик кнопки "Редактировать".
        /// </summary>
        private async void OnEditClicked(ProductTypes category)
        {
            var editSessionData = new SessionData
            {
                CurrentUser = this.SessionData?.CurrentUser,
                Mode = WindowMode.Update,
                Data = category
            };

            await this.Navigation.PushAsync(new PageProductTypeEdit(editSessionData));
        }

        /// <summary>
        /// Обработчик кнопки "Удалить".
        /// </summary>
        private async void OnDeleteClicked(ProductTypes category)
        {
            bool confirm = await this.DisplayAlert("Удаление", $"Вы уверены, что хотите удалить категорию \"{category.Title}\"?", "Да", "Нет");
            if (!confirm)
            {
                return;
            }

            try
            {
                await this.ApiClient.DeleteProductTypeAsync(category.Id);
                await this.DisplayAlert("Успех", "Категория удалена.", "OK");
                this.LoadCategoriesAsync();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось удалить категорию: {ex.Message}", "OK");
            }
        }

        #endregion
    }
}