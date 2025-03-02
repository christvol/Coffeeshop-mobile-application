using System.Collections.ObjectModel;

namespace Mobile_application.Controls
{
    public partial class CustomCollectionView : ContentView
    {
        public CustomCollectionView()
        {
            this.InitializeComponent();
            this.BindingContext = this; // Важно! Без этого привязка не работает.
        }

        public ObservableCollection<object> Items
        {
            get => (ObservableCollection<object>)this.collectionView.ItemsSource;
            set => this.collectionView.ItemsSource = value;
        }
    }
}
