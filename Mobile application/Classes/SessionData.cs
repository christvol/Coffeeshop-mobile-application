using Common.Classes.DB;

namespace Common.Classes.Session
{
    /// <summary>
    /// Представляет данные сессии для передачи информации между окнами.
    /// </summary>
    public class SessionData
    {
        #region Свойства

        /// <summary>
        /// Текущий пользователь.
        /// </summary>
        public Users? CurrentUser
        {
            get; set;
        }

        /// <summary>
        /// Общие данные, передаваемые между окнами.
        /// </summary>
        public object? Data
        {
            get; set;
        }

        /// <summary>
        /// Режим работы окна (например, Создание/Чтение/Обновление/Удаление).
        /// </summary>
        public WindowMode Mode
        {
            get; set;
        }

        /// <summary>
        /// Определяет, содержит ли окно кнопку возврата назад.
        /// </summary>
        public bool HasBackButton
        {
            get; set;
        }

        #endregion
    }

    /// <summary>
    /// Определяет режим работы окна для операций CRUD.
    /// </summary>
    public enum WindowMode
    {
        /// <summary>
        /// Режим создания.
        /// </summary>
        Create,

        /// <summary>
        /// Режим чтения.
        /// </summary>
        Read,

        /// <summary>
        /// Режим обновления.
        /// </summary>
        Update,

        /// <summary>
        /// Режим удаления.
        /// </summary>
        Delete
    }
}
