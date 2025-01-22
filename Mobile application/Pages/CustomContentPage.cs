using Common.Classes.Session;
using Mobile_application.Classes;
using Mobile_application.Classes.API;

namespace Mobile_application.Pages
{
    public class CustomContentPage : ContentPage
    {
        #region Свойства
        public ApiClient ApiClient { get; private set; } = new ApiClient(CommonLocal.API.entryPoint);
        public SessionData SessionData
        {
            set; get;
        }
        #endregion

        #region Методы

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

            var mainPage = Application.Current.MainPage;
            if (mainPage == null)
            {
                throw new InvalidOperationException("The main page is null.");
            }

            var navigation = mainPage.Navigation;
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

        public void CheckSessionData()
        {
            if (this.SessionData == null)
                throw new InvalidOperationException("SessionData не может быть null.");
        }

        public void CheckCurrentUser()
        {
            this.CheckSessionData();
            if (this.SessionData.CurrentUser == null)
                throw new InvalidOperationException("Текущий пользователь не задан в SessionData.");
        }
        #endregion
    }
}
