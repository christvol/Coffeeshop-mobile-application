namespace Мобильное_приложение.Controls
{
    public class CountryCodeModel
    {
        public string CountryCode { get; set; }
        public string CountryFlag { get; set; }
        public string CountryName { get; set; }
    }

    public partial class CountryCodesView : ContentView
    {
        public CountryCodesView()
        {
            InitializeComponent();
        }

        // Метод для установки источника данных
        public void SetItemsSource(IEnumerable<CountryCodeModel> items)
        {
            if (items != null)
            {
                CountryPicker.ItemsSource = items.ToList();
            }
        }
    }
}
