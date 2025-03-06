namespace Common.Classes.DTO
{
    /// <summary>
    /// DTO (Data Transfer Object) для типа ингредиента.
    /// Используется для передачи данных без навигационных свойств.
    /// </summary>
    public class IngredientTypeDTO
    {
        /// <summary>
        /// Уникальный идентификатор типа ингредиента.
        /// </summary>
        public int Id
        {
            get; set;
        }

        /// <summary>
        /// Название типа ингредиента.
        /// </summary>
        public string Title { get; set; } = string.Empty;
    }
}
