using Common.Classes.Session;
using Mobile_application.Pages;

namespace Mobile_application.Controls
{
    public partial class PageHeader : ContentView
    {
        public PageHeader()
        {
            this.InitializeComponent();

            // Устанавливаем логотип (если потребуется)
            this.CafeLogoSource = "logo.png";

            // Привязываем контекст данных
            this.BindingContext = this;
        }

        #region Bindable Properties

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
            if (this.SessionData != null)
            {
                this.SessionData.Mode = WindowMode.Update;
                _ = Application.Current.MainPage.Navigation.PushAsync(new PageUserProfile(this.SessionData));
            }
        }

        private void btnCart_Clicked(object sender, EventArgs e)
        {
            // TODO: перейти на страницу корзины
        }
    }
}
