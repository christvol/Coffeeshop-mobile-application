namespace Mobile_application.Controls
{
    public partial class PageHeader : ContentView
    {
        public PageHeader()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty CafeLogoSourceProperty =
            BindableProperty.Create(nameof(CafeLogoSource), typeof(string), typeof(PageHeader), default(string));

        public string CafeLogoSource
        {
            get => (string)this.GetValue(CafeLogoSourceProperty);
            set => this.SetValue(CafeLogoSourceProperty, value);
        }

        public static readonly BindableProperty UserAvatarSourceProperty =
            BindableProperty.Create(nameof(UserAvatarSource), typeof(string), typeof(PageHeader), default(string));

        public string UserAvatarSource
        {
            get => (string)this.GetValue(UserAvatarSourceProperty);
            set => this.SetValue(UserAvatarSourceProperty, value);
        }
    }
}
