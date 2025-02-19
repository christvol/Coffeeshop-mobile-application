namespace Common.Classes.DTO
{
    public class IngredientResponseDto
    {
        public int Id
        {
            get; set;
        }
        public int? IdIngredientType
        {
            get; set;
        }
        public string Title { get; set; } = null!;
        public string? Description
        {
            get; set;
        }
        public float Fee
        {
            get; set;
        }
        public IngredientTypeDto? IngredientType
        {
            get; set;
        }  // Включаем только нужные данные об типе ингредиента
    }
}
