using CommunityToolkit.Maui.Views;


namespace Мобильное_приложение.Controls
{
    public class ItemModel
    {
        public string DisplayName { get; set; } // Название элемента
        public string ImageSource { get; set; } // Путь к изображению
    }

    public partial class PickerWithImages : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<ItemModel>), typeof(PickerWithImages), propertyChanged: OnItemsSourceChanged);

        public IEnumerable<ItemModel> ItemsSource
        {
            get => (IEnumerable<ItemModel>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public PickerWithImages()
        {
            InitializeComponent();
        }



        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (ItemsSource == null || !ItemsSource.Any())
            {
                return;
            }

            var popup = new PickerWithImagesPopup(ItemsSource);
            popup.ItemSelected += OnItemSelected;
            Application.Current.MainPage.ShowPopup(popup);
        }


        private void OnItemSelected(object sender, ItemModel e)
        {
            SelectedItemButton.Text = $"{e.DisplayName}";
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Обработка изменений списка элементов, если необходимо
        }
    }
}
