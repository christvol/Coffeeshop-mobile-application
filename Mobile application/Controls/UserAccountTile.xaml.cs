namespace Mobile_application.Controls;

public partial class UserAccountTile : ContentView
{
    public UserAccountTile()
    {
        this.InitializeComponent();
    }

    // Свойства для привязки данных
    public static readonly BindableProperty NameProperty =
        BindableProperty.Create(nameof(Name), typeof(string), typeof(UserAccountTile), string.Empty);

    public static readonly BindableProperty PhoneNumberProperty =
        BindableProperty.Create(nameof(PhoneNumber), typeof(string), typeof(UserAccountTile), string.Empty);

    public static readonly BindableProperty AvatarProperty =
        BindableProperty.Create(nameof(Avatar), typeof(string), typeof(UserAccountTile), "profile_placeholder.png");

    public static readonly BindableProperty ChevronIconProperty =
        BindableProperty.Create(nameof(ChevronIcon), typeof(string), typeof(UserAccountTile), "chevron_right.png");

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(Command), typeof(UserAccountTile));

    public string Name
    {
        get => (string)this.GetValue(NameProperty);
        set => this.SetValue(NameProperty, value);
    }

    public string PhoneNumber
    {
        get => (string)this.GetValue(PhoneNumberProperty);
        set => this.SetValue(PhoneNumberProperty, value);
    }

    public string Avatar
    {
        get => (string)this.GetValue(AvatarProperty);
        set => this.SetValue(AvatarProperty, value);
    }

    public string ChevronIcon
    {
        get => (string)this.GetValue(ChevronIconProperty);
        set => this.SetValue(ChevronIconProperty, value);
    }

    public Command Command
    {
        get => (Command)this.GetValue(CommandProperty);
        set => this.SetValue(CommandProperty, value);
    }

    // Обработчик нажатия на плитку
    private void OnTileClicked(object sender, EventArgs e)
    {
        this.Command?.Execute(null);
    }
}
