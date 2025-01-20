using Common.Classes.Session;
using Mobile_application.Pages;

namespace Mobile_application.Controls
{
    public partial class PageHeader : ContentView
    {
        public PageHeader()
        {
            this.InitializeComponent();

            // Устанавливаем значения по умолчанию
            this.CafeLogoSource = "logo.png";
            this.UserAvatarSource = "avatarDefault.png";

            // Привязываем контекст данных
            this.BindingContext = this;
        }

        #region Bindable Properties

        // Свойство для источника логотипа
        public static readonly BindableProperty CafeLogoSourceProperty =
            BindableProperty.Create(
                nameof(CafeLogoSource),
                typeof(string),
                typeof(PageHeader),
                "logo.png");

        public string CafeLogoSource
        {
            get => (string)this.GetValue(CafeLogoSourceProperty);
            set => this.SetValue(CafeLogoSourceProperty, value);
        }

        // Свойство для источника аватара пользователя
        public static readonly BindableProperty UserAvatarSourceProperty =
            BindableProperty.Create(
                nameof(UserAvatarSource),
                typeof(string),
                typeof(PageHeader),
                "avatarDefault.png");

        public string UserAvatarSource
        {
            get => (string)this.GetValue(UserAvatarSourceProperty);
            set => this.SetValue(UserAvatarSourceProperty, value);
        }

        // Свойство для передачи SessionData
        public static readonly BindableProperty SessionDataProperty =
            BindableProperty.Create(
                nameof(SessionData),
                typeof(SessionData),
                typeof(PageHeader),
                default(SessionData));

        public SessionData? SessionData
        {
            get => (SessionData?)this.GetValue(SessionDataProperty);
            set => this.SetValue(SessionDataProperty, value);
        }

        #endregion

        private void btnAvatar_Clicked(object sender, EventArgs e)
        {
            // Проверяем наличие SessionData
            if (this.SessionData != null)
            {
                // Переход на страницу профиля с передачей SessionData
                this.SessionData.Mode = WindowMode.Update;
                Application.Current.MainPage.Navigation.PushAsync(new PageUserProfile(this.SessionData));
            }
        }
    }
}
