using CommunityToolkit.Maui.Views;

namespace Мобильное_приложение.Controls
{
    public partial class PickerWithImagesPopup : Popup
    {
        public event EventHandler<ItemModel> ItemSelected;

        public PickerWithImagesPopup(IEnumerable<ItemModel> items)
        {
            InitializeComponent();
            ItemsCollectionView.ItemsSource = items;
            ItemsCollectionView.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is ItemModel selectedItem)
            {
                ItemSelected?.Invoke(this, selectedItem);
                // Закрытие всплывающего окна после выбора
                (Parent as Popup)?.Close();
            }
        }
    }
}
