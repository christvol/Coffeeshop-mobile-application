using Common.Classes.DB;
using Common.Classes.DTO;
using Common.Classes.Session;
using REST_API_SERVER.DTOs;
using System.Collections.ObjectModel;

namespace Mobile_application.Pages
{
    public partial class PageProductEdit : CustomContentPage
    {
        public ProductDTO Product
        {
            get; set;
        }
        public ObservableCollection<ProductTypes> ProductTypes { get; private set; } = new();
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

            this.SessionData = sessionData;
            this.Mode = sessionData.Mode;
            this.Product = sessionData.Data as ProductDTO ?? new ProductDTO();

            // Устанавливаем BindingContext на весь объект, чтобы привязки работали корректно
            this.BindingContext = this;

            // Загружаем типы продуктов
            this.LoadProductTypes();

            // Блокируем поля, если режим Read
            if (this.Mode == WindowMode.Read)
            {
                //this.BtnSave.IsVisible = false;
                this.EntryTitle.IsEnabled = false;
                this.EditorDescription.IsEnabled = false;
                this.EntryFee.IsEnabled = false;
                this.PickerProductType.IsEnabled = false;
            }
        }

        private async void LoadProductTypes()
        {
            try
            {
                var types = await this.ApiClient.GetAllProductTypesAsync();
                this.ProductTypes.Clear();

                foreach (var type in types)
                {
                    this.ProductTypes.Add(type);
                }

                this.SelectedProductType = this.ProductTypes.FirstOrDefault(pt => pt.Id == this.Product.IdProductType);
                this.OnPropertyChanged(nameof(this.ProductTypes)); // Обновление привязки списка
                this.OnPropertyChanged(nameof(this.SelectedProductType)); // Обновление привязки выбранного типа
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Ошибка", $"Не удалось загрузить типы продуктов: {ex.Message}", "OK");
            }
        }
        private async void OnAddIngredientClicked(object sender, EventArgs e)
        {
            // Создаем новый объект ингредиента
            var newIngredient = new IngredientDTO
            {
                Title = string.Empty,
                Description = string.Empty,
                Fee = 0
            };

            // Создаем объект SessionData для режима Create
            var newSessionData = new SessionData
            {
                CurrentUser = this.SessionData.CurrentUser,
                Mode = WindowMode.Create, // Режим создания
                Data = newIngredient // Передаем новый ингредиент
            };

            // Переходим на страницу редактирования ингредиента
            await this.Navigation.PushAsync(new PageIngredientEdit(newSessionData));
        }
        private async void OnEditIngredientClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is IngredientDTO ingredient)
            {
                // Создаем объект SessionData для режима Update
                var editSessionData = new SessionData
                {
                    CurrentUser = this.SessionData.CurrentUser,
                    Mode = WindowMode.Update, // Режим обновления
                    Data = ingredient // Передаем выбранный ингредиент для редактирования
                };

                // Переходим на страницу редактирования ингредиента
                await this.Navigation.PushAsync(new PageIngredientEdit(editSessionData));
            }
        }
        private async void OnDeleteIngredientClicked(object sender, EventArgs e)
        {
            //if (sender is Button button && button.BindingContext is IngredientDTO ingredient)
            //{
            //    var confirm = await this.DisplayAlert("Подтверждение", $"Вы уверены, что хотите удалить ингредиент \"{ingredient.Title}\"?", "Да", "Нет");
            //    if (!confirm)
            //    {
            //        return;
            //    }

            //    try
            //    {
            //        // Удаляем ингредиент через API
            //        await this.ApiClient.DeleteIngredientAsync(ingredient.Id);

            //        // Удаляем ингредиент из локального списка
            //        _ = this.Product.Ingredients.Remove(ingredient);

            //        await this.DisplayAlert("Успех", "Ингредиент успешно удален.", "OK");
            //    }
            //    catch (Exception ex)
            //    {
            //        await this.DisplayAlert("Ошибка", $"Не удалось удалить ингредиент: {ex.Message}", "OK");
            //    }
            //}
        }
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                // Удаляем символ валюты и пробелы, заменяем запятую на точку
                string feeText = this.EntryFee.Text?.Replace("₽", "").Trim().Replace(",", ".") ?? "0";

                // Пробуем преобразовать строку в decimal
                if (!decimal.TryParse(feeText, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out var fee))
                {
                    await this.DisplayAlert("Ошибка", "Некорректный формат цены. Используйте число с точкой или запятой.", "OK");
                    return;
                }

                // Обновляем данные продукта
                this.Product.Title = this.EntryTitle.Text;
                this.Product.Description = this.EditorDescription.Text;
                this.Product.Fee = (float)fee; // Сохраняем правильное значение
                this.Product.IdProductType = this.SelectedProductType?.Id ?? this.Product.IdProductType;

                if (this.Mode == WindowMode.Create)
                {
                    _ = await this.ApiClient.CreateProductAsync(this.Product);
                    await this.DisplayAlert("Успех", "Продукт успешно создан.", "OK");
                }
                else if (this.Mode == WindowMode.Update)
                {
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
