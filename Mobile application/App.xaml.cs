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

            this.MainPage = new NavigationPage(new PageLogin());
        }
    }
}
