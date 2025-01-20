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
