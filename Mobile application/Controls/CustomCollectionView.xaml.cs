using System.Collections;
using System.Windows.Input;

namespace Mobile_application.Controls
{
    public partial class CustomCollectionView : ContentView
    {
        #region Свойства

        public static readonly BindableProperty IsEditButtonVisibleProperty =
            BindableProperty.Create(nameof(IsEditButtonVisible), typeof(bool), typeof(CustomCollectionView), true, propertyChanged: OnButtonsVisibilityChanged);

        public bool IsEditButtonVisible
        {
            get => (bool)this.GetValue(IsEditButtonVisibleProperty);
            set => this.SetValue(IsEditButtonVisibleProperty, value);
        }

        public static readonly BindableProperty IsDeleteButtonVisibleProperty =
            BindableProperty.Create(nameof(IsDeleteButtonVisible), typeof(bool), typeof(CustomCollectionView), true, propertyChanged: OnButtonsVisibilityChanged);

        public bool IsDeleteButtonVisible
        {
            get => (bool)this.GetValue(IsDeleteButtonVisibleProperty);
            set => this.SetValue(IsDeleteButtonVisibleProperty, value);
        }

        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(nameof(Items), typeof(IEnumerable), typeof(CustomCollectionView),
                default(IEnumerable), propertyChanged: OnItemsChanged);

        public IEnumerable Items
        {
            get => (IEnumerable)this.GetValue(ItemsProperty);
            set => this.SetValue(ItemsProperty, value);
        }

        public static readonly BindableProperty DisplayedFieldsProperty =
            BindableProperty.Create(nameof(DisplayedFields), typeof(List<string>), typeof(CustomCollectionView),
                new List<string>(), propertyChanged: OnFieldsChanged);

        public List<string> DisplayedFields
        {
            get => (List<string>)this.GetValue(DisplayedFieldsProperty);
            set => this.SetValue(DisplayedFieldsProperty, value);
        }

        public static readonly BindableProperty EditCommandProperty =
            BindableProperty.Create(nameof(EditCommand), typeof(ICommand), typeof(CustomCollectionView), null);

        public ICommand EditCommand
        {
            get => (ICommand)this.GetValue(EditCommandProperty);
            set => this.SetValue(EditCommandProperty, value);
        }

        public static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(CustomCollectionView), null);

        public ICommand DeleteCommand
        {
            get => (ICommand)this.GetValue(DeleteCommandProperty);
            set => this.SetValue(DeleteCommandProperty, value);
        }

        public static readonly BindableProperty ItemSelectedCommandProperty =
            BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(CustomCollectionView), null);

        public ICommand ItemSelectedCommand
        {
            get => (ICommand)this.GetValue(ItemSelectedCommandProperty);
            set => this.SetValue(ItemSelectedCommandProperty, value);
        }

        #endregion

        #region События

        public event EventHandler<object> ItemSelected;

        #endregion

        #region Конструкторы

        public CustomCollectionView()
        {
            this.InitializeComponent();
            this.BindingContext = this;
            this.collectionView.SelectionChanged += this.OnItemSelectedInternal;
        }

        #endregion

        #region Методы API

        public void SetDisplayedFields(params string[] fields)
        {
            this.DisplayedFields = fields.ToList();
        }

        public void SetItems(IEnumerable items)
        {
            this.Items = items;
        }

        public void SetEditCommand<T>(Action<T> execute)
        {
            this.EditCommand = new Command<T>(execute);
        }

        public void SetDeleteCommand<T>(Action<T> execute)
        {
            this.DeleteCommand = new Command<T>(execute);
        }

        public void SetItemSelectedCommand<T>(Action<T> execute)
        {
            this.ItemSelectedCommand = new Command<T>(execute);
        }

        #endregion

        #region Обработчики BindableProperty

        private static void OnButtonsVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomCollectionView collectionView)
            {
                collectionView.UpdateItemTemplate();
            }
        }

        private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomCollectionView collectionView)
            {
                collectionView.collectionView.ItemsSource = (IEnumerable)newValue;
            }
        }

        private static void OnFieldsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomCollectionView collectionView)
            {
                collectionView.UpdateItemTemplate();
            }
        }

        #endregion

        #region Обработчики событий

        private void OnItemSelectedInternal(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is object selectedItem)
            {
                this.ItemSelected?.Invoke(this, selectedItem);
                this.ItemSelectedCommand?.Execute(selectedItem);
            }
        }

        #endregion

        #region Генерация шаблона

        private void UpdateItemTemplate()
        {
            if (this.DisplayedFields == null || !this.DisplayedFields.Any())
            {
                return;
            }

            this.collectionView.ItemTemplate = new DataTemplate(() =>
            {
                var stackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Spacing = 5
                };

                foreach (string field in this.DisplayedFields)
                {
                    var label = new Label
                    {
                        FontSize = 14,
                        VerticalOptions = LayoutOptions.Center,
                        Margin = new Thickness(5, 0)
                    };
                    label.SetBinding(Label.TextProperty, field);
                    stackLayout.Children.Add(label);
                }

                var buttonsStack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    Spacing = 5
                };

                if (this.IsEditButtonVisible)
                {
                    var editButton = new Button
                    {
                        Text = "✎",
                        BackgroundColor = (Color)Application.Current.Resources["Primary"],
                        TextColor = Colors.White,
                        CornerRadius = 10
                    };
                    editButton.SetBinding(Button.CommandProperty, new Binding(nameof(this.EditCommand), source: this));
                    editButton.SetBinding(Button.CommandParameterProperty, new Binding("."));
                    buttonsStack.Children.Add(editButton);
                }

                if (this.IsDeleteButtonVisible)
                {
                    var deleteButton = new Button
                    {
                        Text = "🗑",
                        BackgroundColor = (Color)Application.Current.Resources["Danger"],
                        TextColor = Colors.White,
                        CornerRadius = 10
                    };
                    deleteButton.SetBinding(Button.CommandProperty, new Binding(nameof(this.DeleteCommand), source: this));
                    deleteButton.SetBinding(Button.CommandParameterProperty, new Binding("."));
                    buttonsStack.Children.Add(deleteButton);
                }

                var frame = new Frame
                {
                    Padding = 10,
                    CornerRadius = 10,
                    HasShadow = true,
                    BackgroundColor = (Color)Application.Current.Resources["Tertiary"],
                    Content = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 5,
                        Children =
                        {
                            stackLayout,
                            buttonsStack
                        }
                    }
                };

                return frame;
            });
        }

        #endregion
    }
}
