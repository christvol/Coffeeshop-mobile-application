namespace Mobile_application.Controls
{
    public partial class CustomCollectionView : ContentView
    {
        public CustomCollectionView()
        {
            this.InitializeComponent();
        }

        // Обработчик выбора элемента
        private void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.CurrentSelection.FirstOrDefault();
            if (selectedItem != null)
            {
                // Логика обработки выбранного элемента
            }
        }

        // Обработчик клика по кнопке редактирования
        private void OnEditItemClicked(object sender, EventArgs e)
        {
            //var item = button?.BindingContext as YourEntityModel; // Замените на вашу модель
            //if (item != null)
            //{
            //    // Логика редактирования сущности
            //}
        }

        // Обработчик клика по кнопке удаления
        private void OnDeleteItemClicked(object sender, EventArgs e)
        {
            //var item = button?.BindingContext as YourEntityModel; // Замените на вашу модель
            //if (item != null)
            //{
            //    // Логика удаления сущности
            //}
        }
    }
}