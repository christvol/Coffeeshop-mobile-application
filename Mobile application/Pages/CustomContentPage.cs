using Common.Classes.Session;
using Mobile_application.Classes;
using Mobile_application.Classes.API;

namespace Mobile_application.Pages
{
    public class CustomContentPage : ContentPage
    {
        #region Свойства       
        public ApiClient ApiClient { get; private set; } = new ApiClient(CommonLocal.API.entryPoint);
        public SessionData? SessionData
        {
            set; get;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public CustomContentPage(SessionData? sessionData = null)
        {

            NavigationPage.SetHasBackButton(this, this.SessionData == null ? true : this.SessionData.HasBackButton);
            // Гарантируем, что SessionData не будет null
            this.SessionData = sessionData ?? new SessionData();
        }
        #endregion

        #region Методы
        /// <summary>
        /// Выводит всплывающее уведомление об ошибке.
        /// </summary>
        /// <param name="ex">Исключение, текст которого будет отображён.</param>
        /// <param name="title">Заголовок окна. По умолчанию "Ошибка".</param>
        protected async Task ShowError(Exception ex, string title = "Ошибка")
        {
            await this.DisplayAlert(title, ex.Message, "OK");
        }
        /// <summary>
        /// Выводит всплывающее уведомление с произвольным текстом.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="message">Текст сообщения.</param>
        protected async Task ShowMessage(string title, string message)
        {
            await this.DisplayAlert(title, message, "OK");
        }
        /// <summary>
        /// Проверяет, является ли объект SessionData пустым (null).
        /// </summary>
        /// <returns>
        /// Возвращает <c>true</c>, если SessionData равно <c>null</c>, иначе <c>false</c>.
        /// </returns>
        public bool IsSessionDataNull()
        {
            return this.SessionData == null;
        }
        /// <summary>
        /// Проверяет, задан ли текущий пользователь в объекте SessionData.
        /// </summary>
        /// <returns>
        /// Возвращает <c>true</c>, если SessionData равно <c>null</c> или текущий пользователь (CurrentUser) не задан, иначе <c>false</c>.
        /// </returns>
        public bool IsCurrentUserNull()
        {
            if (this.SessionData == null)
            {
                return true;
            }

            return this.SessionData.CurrentUser == null;
        }
        public static async Task NavigateToPage(Page page)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page), "The target page cannot be null.");
            }

            if (Application.Current == null)
            {
                throw new InvalidOperationException("The application instance is null.");
            }

            Page? mainPage = Application.Current.MainPage;
            if (mainPage == null)
            {
                throw new InvalidOperationException("The main page is null.");
            }

            INavigation navigation = mainPage.Navigation;
            if (navigation == null)
            {
                throw new InvalidOperationException("Navigation stack is null.");
            }

            try
            {
                await navigation.PushAsync(page);
            }
            catch (Exception ex)
            {
                // Здесь можно добавить логирование ошибки или другое действие
                throw new InvalidOperationException("Failed to navigate to the specified page.", ex);
            }
        }

        /// <summary>
        /// Проверяет, является ли пользователь администратором.
        /// </summary>
        public async Task<bool> IsUserAdminAsync(int userId)
        {
            // Получаем данные о пользователе
            Common.Classes.DB.Users? user = await this.ApiClient.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            // Если навигационное свойство уже загружено, используем его
            if (user.IdUserTypeNavigation != null)
            {
                return user.IdUserTypeNavigation.Title == "Admin";
            }

            // Загружаем все типы пользователей и ищем "Admin"
            List<Common.Classes.DB.UserTypes> userTypes = await this.ApiClient.GetAllUserTypesAsync();

            // Ищем тип "Admin" в отсортированном списке
            Common.Classes.DB.UserTypes? adminType = userTypes
                .OrderBy(t => t.Title)
                .FirstOrDefault(t => t.Title == "Admin");

            if (adminType == null)
            {
                return false;
            }

            return user.IdUserType == adminType.Id;
        }


        #endregion
    }
}
