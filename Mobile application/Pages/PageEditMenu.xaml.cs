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

        public PageEditMenu(SessionData sessionData) : base(sessionData)
        {
            this.InitializeComponent();
            this._sessionData = sessionData;
        }

        #endregion

        #region Обработчики событий

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Попробуем найти PageHeader и установить ему SessionData
            if (this.FindByName("PageHeader") is Controls.PageHeader header)
            {
                header.SessionData = this.SessionData;
            }
        }

        private async void OnProductTypesClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageProductTypes(this.SessionData));
        }

        private async void OnProductClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageProducts(this.SessionData));
        }

        private async void OnIngredientTypesClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageIngredientTypes(this.SessionData));
        }

        private async void OnIngredientClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageIngredients(this.SessionData));
        }

        /// <summary>
        /// Обработчик кнопки "Заказы".
        /// </summary>
        private async void OnOrdersClicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new PageOrders(this.SessionData));
        }

        #endregion
    }
}
