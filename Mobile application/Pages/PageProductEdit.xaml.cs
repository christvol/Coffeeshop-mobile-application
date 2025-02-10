using Common.Classes.DB;
using Common.Classes.Session;
using REST_API_SERVER.DTOs;

namespace Mobile_application.Pages
{
    public partial class PageProductEdit : CustomContentPage
    {
        public ProductDTO Product
        {
            get; set;
        }
        public List<ProductTypes> ProductTypes { get; private set; } = new();
        public ProductTypes SelectedProductType
        {
            get; set;
        }
        public WindowMode Mode
        {
            get;
        }

        public PageProductEdit(SessionData sessionData)
        {
            this.InitializeComponent();
            this.BindingContext = this;

            this.SessionData = sessionData;
            this.Mode = sessionData.Mode; // Получаем текущий режим окна
            this.Product = sessionData.Data as ProductDTO ?? new ProductDTO();

            // Если режим не Create, загружаем данные типа продуктов
            if (this.Mode != WindowMode.Create)
            {
                this.LoadProductTypes();
            }
        }

        private async void LoadProductTypes()
        {
            try
            {
                this.ProductTypes = await this.ApiClient.GetAllProductTypesAsync();
                this.SelectedProductType = this.ProductTypes.FirstOrDefault(pt => pt.Id == this.Product.IdProductType);
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось загрузить типы продуктов: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                // Обновляем данные продукта
                this.Product.Title = EntryTitle.Text;
                this.Product.Description = EditorDescription.Text;
                this.Product.Fee = Int32.TryParse(EntryFee.Text, out var fee) ? fee : 0;
                this.Product.IdProductType = this.SelectedProductType?.Id ?? this.Product.IdProductType;

                if (this.Mode == WindowMode.Create)
                {
                    // Создаем новый продукт
                    _ = await this.ApiClient.CreateProductAsync(this.Product);
                    await this.DisplayAlert("Успех", "Продукт успешно создан.", "OK");
                }
                else if (this.Mode == WindowMode.Update)
                {
                    // Обновляем существующий продукт
                    _ = await this.ApiClient.UpdateProductAsync(this.Product.Id, this.Product);
                    await this.DisplayAlert("Успех", "Продукт успешно обновлен.", "OK");
                }

                _ = await this.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось сохранить продукт: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            _ = await this.Navigation.PopAsync();
        }
    }
}