using Common.Classes.Session;

namespace Mobile_application.Pages
{
    public partial class PageEditMenu : CustomContentPage
    {
        #region Поля
        private readonly SessionData _sessionData;
        #endregion

        #region Конструкторы/Деструкторы
        public PageEditMenu(SessionData sessionData)
        {
            this.InitializeComponent();
            this._sessionData = sessionData; // Инициализация поля сессии
        }
        #endregion

        #region Обработчики событий
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Синхронное ожидание задачи
            Common.Classes.DB.Users? user = await this.ApiClient.GetUserByIdAsync(1);

            // Формируем SessionData
            this.SessionData = new SessionData
            {
                CurrentUser = user,
                Data = null, // Можно передать дополнительные данные, если нужно
                Mode = WindowMode.Read
            };
        }


        // Обработчик клика по кнопке "Типы продуктов"
        private async void OnProductTypesClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageProductTypes(this.SessionData));
        }

        // Обработчик клика по кнопке "Продукты"
        private async void OnProductClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageProducts(this.SessionData));
        }

        // Обработчик клика по кнопке "Типы ингредиентов"
        private async void OnIngredientTypesClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageIngredientTypes(this.SessionData));
        }

        // Обработчик клика по кнопке "Ингредиенты"
        private async void OnIngredientClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageIngredients(this.SessionData));
        }

        #endregion
    }
}
