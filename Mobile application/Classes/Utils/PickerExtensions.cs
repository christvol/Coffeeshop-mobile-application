using System.Collections;

namespace Mobile_application.Classes.Utils
{
    public static class PickerExtensions
    {
        /// <summary>
        /// Настраивает <see cref="Picker"/> с заданными параметрами.
        /// </summary>
        /// <typeparam name="T">Тип элементов источника данных.</typeparam>
        /// <param name="picker">Экземпляр <see cref="Picker"/>, который настраивается.</param>
        /// <param name="itemsSource">Источник данных для списка выбора.</param>
        /// <param name="displayProperty">Имя свойства объекта, которое будет отображаться в списке.</param>
        /// <param name="selectedItem">Выбранный элемент.</param>
        public static void ConfigurePicker<T>(this Picker picker, IEnumerable itemsSource, string displayProperty, object selectedItem = null)
        {
            if (picker == null)
            {
                throw new ArgumentNullException(nameof(picker), "Picker не может быть null.");
            }

            if (itemsSource == null)
            {
                throw new ArgumentNullException(nameof(itemsSource), "ItemsSource не может быть null.");
            }

            if (string.IsNullOrWhiteSpace(displayProperty))
            {
                throw new ArgumentException("Свойство отображения не может быть пустым.", nameof(displayProperty));
            }

            // Устанавливаем источник данных
            picker.ItemsSource = itemsSource.Cast<object>().ToList();

            // Устанавливаем привязку для отображаемого свойства
            picker.ItemDisplayBinding = new Binding(displayProperty);

            // Устанавливаем выбранный элемент (если задан)
            if (selectedItem != null)
            {
                picker.SelectedItem = selectedItem;
            }
        }
    }

}
