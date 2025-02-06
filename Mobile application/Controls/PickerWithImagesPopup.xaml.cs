using CommunityToolkit.Maui.Views;

namespace Mobile_application.Controls
{
    public partial class PickerWithImagesPopup : Popup
    {
        public event EventHandler<ItemModel> ItemSelected;

        public PickerWithImagesPopup(IEnumerable<ItemModel> items)
        {
            this.InitializeComponent();
            this.ItemsCollectionView.ItemsSource = items;
            this.ItemsCollectionView.SelectionChanged += this.OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is ItemModel selectedItem)
            {
                ItemSelected?.Invoke(this, selectedItem);
                // Закрытие всплывающего окна после выбора
                (this.Parent as Popup)?.Close();
            }
        }
    }
}
