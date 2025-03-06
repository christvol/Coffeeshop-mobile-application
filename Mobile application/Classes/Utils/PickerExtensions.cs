using System.Collections;

public static class PickerExtensions
{
    /// <summary>
    /// Настраивает <see cref="Picker"/> с указанными параметрами.
    /// </summary>
    /// <typeparam name="T">Тип элементов источника данных.</typeparam>
    /// <param name="picker">Экземпляр <see cref="Picker"/>, который настраивается.</param>
    /// <param name="itemsSource">Источник данных для списка выбора.</param>
    /// <param name="displayProperty">Имя свойства объекта, которое будет отображаться в списке.</param>
    /// <param name="valueProperty">Имя свойства, используемого для сравнения и выбора элемента.</param>
    /// <param name="selectedItem">Выбранный элемент, содержащий значение для поиска.</param>
    public static void ConfigurePicker<T>(
        this Picker picker,
        IEnumerable itemsSource,
        string displayProperty,
        string valueProperty,
        object selectedItem = null)
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

        if (string.IsNullOrWhiteSpace(valueProperty))
        {
            throw new ArgumentException("Свойство сравнения не может быть пустым.", nameof(valueProperty));
        }

        // Устанавливаем источник данных
        picker.ItemsSource = itemsSource.Cast<object>().ToList();

        // Устанавливаем привязку для отображаемого свойства
        picker.ItemDisplayBinding = new Binding(displayProperty);

        // Если передан selectedItem, пытаемся найти соответствующий объект в itemsSource
        if (selectedItem != null)
        {
            object? selectedValue = selectedItem.GetType().GetProperty(valueProperty)?.GetValue(selectedItem);

            if (selectedValue != null)
            {
                object? matchingItem = itemsSource.Cast<object>()
                    .FirstOrDefault(item =>
                        item.GetType().GetProperty(valueProperty)?.GetValue(item)?.Equals(selectedValue) == true);

                if (matchingItem != null)
                {
                    picker.SelectedItem = matchingItem;
                }
            }
        }
    }
}
