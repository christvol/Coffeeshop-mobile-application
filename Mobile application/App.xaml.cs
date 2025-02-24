using Common.Classes.Session;
using Mobile_application.Classes;
using Mobile_application.Classes.API;
using Mobile_application.Pages;

namespace Mobile_application
{
    public partial class App : Application
    {
        /// <summary>
        /// Флаг, указывающий, что приложение запущено в режиме отладки.
        /// </summary>
        public static bool IsDebugMode
        {
            get; private set;
        }

        public App()
        {
            this.InitializeComponent();

            // Устанавливаем значение флага IsDebugMode в зависимости от компиляции.
#if DEBUG
            IsDebugMode = true;
#else
            IsDebugMode = false;
#endif

            // Выводим отладочную информацию
            if (IsDebugMode)
            {
                System.Diagnostics.Debug.WriteLine("Приложение запущено в режиме отладки. Флаг снимается при запуске в Release.");
            }

            //this.MainPage = new NavigationPage(new PageLogin());
            var apiClient = new ApiClient(CommonLocal.API.entryPoint);
            var user = apiClient.GetUserByIdAsync(1);
            // Формируем SessionData
            var sessionData = new SessionData
            {
                CurrentUser = user,
                Data = null, // Можно передать дополнительные данные, если нужно
                Mode = WindowMode.Read
            };
            this.MainPage = new NavigationPage(new PageEditMenu(sessionData));
        }
    }
}
