using Common.Classes.Session;

namespace Mobile_application.Pages
{
    public partial class PageEditMenu : CustomContentPage
    {
        #region Поля

        /// <summary>
        /// Данные текущей сессии.
        /// </summary>
        private readonly SessionData _sessionData;

        #endregion

        #region Конструкторы/Деструкторы

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="PageEditMenu"/> с указанными данными сессии.
        /// </summary>
        /// <param name="sessionData">Данные сессии, содержащие информацию о текущем пользователе и режиме работы.</param>
        public PageEditMenu(SessionData sessionData)
        {
            this.InitializeComponent();
            this._sessionData = sessionData; // Инициализация поля сессии
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Вызывается при появлении страницы. Загружает данные текущего пользователя и инициализирует объект сессии.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Получаем данные пользователя по ID
            Common.Classes.DB.Users? user = await this.ApiClient.GetUserByIdAsync(1);

            // Формируем SessionData
            this.SessionData = new SessionData
            {
                CurrentUser = user,
                Data = null, // Можно передать дополнительные данные, если нужно
                Mode = WindowMode.Read
            };
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Типы продуктов".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void OnProductTypesClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageProductTypes(this.SessionData));
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Продукты".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void OnProductClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageProducts(this.SessionData));
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Типы ингредиентов".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void OnIngredientTypesClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageIngredientTypes(this.SessionData));
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Ингредиенты".
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private async void OnIngredientClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageIngredients(this.SessionData));
        }

        #endregion
    }
}
