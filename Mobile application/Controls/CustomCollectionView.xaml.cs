using System.Collections;

namespace Mobile_application.Controls
{
    public partial class CustomCollectionView : ContentView
    {
        public CustomCollectionView()
        {
            this.InitializeComponent();
            this.BindingContext = this;
        }

        #region Свойства

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

        #endregion

        #region Обработчики изменений свойств

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

        #region Генерация разметки

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
                        Margin = new Thickness(5, 0) // Отступ сверху и снизу
                    };
                    label.SetBinding(Label.TextProperty, field);
                    stackLayout.Children.Add(label);
                }

                return new Frame
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
                            stackLayout, // Вывод полей вертикально
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                                Spacing = 5,
                                Children =
                                {
                                    new Button { Text = "✎", BackgroundColor = (Color)Application.Current.Resources["Primary"], TextColor = Colors.White, CornerRadius = 10 },
                                    new Button { Text = "🗑", BackgroundColor = (Color)Application.Current.Resources["Danger"], TextColor = Colors.White, CornerRadius = 10 }
                                }
                            }
                        }
                    }
                };
            });
        }

        #endregion
    }
}
