using Mobile_application.Pages;

namespace Mobile_application
{
    public partial class App : Application
    {

        #region Поля

        #endregion

        #region Свойства
        /// <summary>
        /// Флаг, указывающий, что приложение запущено в режиме отладки.
        /// </summary>
        public static bool IsDebugMode
        {
            get; private set;
        }

        #endregion

        #region Методы
        public void OnDebugAction()
        {
            // Устанавливаем MainPage с данными пользователя
            //this.MainPage = new NavigationPage(new PageEditMenu(null));
            //this.MainPage = new NavigationPage(new PageProductTypes(null));
            this.MainPage = new NavigationPage(new PageLogin());
        }

        public void OnRelease()
        {
            this.MainPage = new NavigationPage(new PageLogin());
        }
        #endregion

        #region Конструкторы/Деструкторы
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
                this.OnDebugAction();
            }
            else
            {
                this.OnRelease();
            }
        }

        #endregion

    }
}
