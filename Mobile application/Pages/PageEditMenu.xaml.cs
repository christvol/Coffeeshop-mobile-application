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

        // Обработчик клика по кнопке "Продукты"
        private void OnProductTypesClicked(object sender, EventArgs e)
        {
            // Логика для открытия страницы или выполнения действия для продуктов
            _ = this.DisplayAlert("Продукты", "Открыт список продуктов", "OK");
        }

        // Обработчик клика по кнопке "Типы продуктов"
        private void OnAddProductClicked(object sender, EventArgs e)
        {
            // Логика для открытия страницы или выполнения действия для типов продуктов
            _ = this.DisplayAlert("Типы продуктов", "Открыт список типов продуктов", "OK");
        }

        // Обработчик клика по кнопке "Типы ингредиентов"
        private void OnIngredientTypesClicked(object sender, EventArgs e)
        {
            // Логика для открытия страницы или выполнения действия для типов ингредиентов
            _ = this.DisplayAlert("Типы ингредиентов", "Открыт список типов ингредиентов", "OK");
        }

        // Обработчик клика по кнопке "Ингредиенты"
        private void OnIngredientClicked(object sender, EventArgs e)
        {
            // Логика для открытия страницы или выполнения действия для ингредиентов
            _ = this.DisplayAlert("Ингредиенты", "Открыт список ингредиентов", "OK");
        }

        #endregion
    }
}
