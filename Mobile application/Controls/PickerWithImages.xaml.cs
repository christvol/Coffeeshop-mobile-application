using CommunityToolkit.Maui.Views;


namespace Mobile_application.Controls
{
    public class ItemModel
    {
        public string DisplayName
        {
            get; set;
        } // Название элемента
        public string ImageSource
        {
            get; set;
        } // Путь к изображению
    }

    public partial class PickerWithImages : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<ItemModel>), typeof(PickerWithImages), propertyChanged: OnItemsSourceChanged);

        public IEnumerable<ItemModel> ItemsSource
        {
            get => (IEnumerable<ItemModel>)this.GetValue(ItemsSourceProperty);
            set => this.SetValue(ItemsSourceProperty, value);
        }

        public PickerWithImages()
        {
            this.InitializeComponent();
        }



        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (this.ItemsSource == null || !this.ItemsSource.Any())
            {
                return;
            }

            var popup = new PickerWithImagesPopup(this.ItemsSource);
            popup.ItemSelected += this.OnItemSelected;
            Application.Current.MainPage.ShowPopup(popup);
        }


        private void OnItemSelected(object sender, ItemModel e)
        {
            this.SelectedItemButton.Text = $"{e.DisplayName}";
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Обработка изменений списка элементов, если необходимо
        }
    }
}
