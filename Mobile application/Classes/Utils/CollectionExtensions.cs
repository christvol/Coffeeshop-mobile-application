using System.Collections.ObjectModel;

namespace Mobile_application.Classes.Utils
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Заполняет существующую ObservableCollection из List, заменяя содержимое.
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции.</typeparam>
        /// <param name="observableCollection">ObservableCollection, которую нужно обновить.</param>
        /// <param name="items">Список элементов для добавления.</param>
        public static void UpdateObservableCollection<T>(this ObservableCollection<T> observableCollection, List<T> items)
        {
            if (observableCollection == null)
            {
                throw new ArgumentNullException(nameof(observableCollection), "Целевая коллекция не может быть null.");
            }

            observableCollection.Clear();
            foreach (T item in items)
            {
                observableCollection.Add(item);
            }
        }
    }

}
