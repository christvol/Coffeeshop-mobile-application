namespace Common.Classes.DTO
{
    public class AllowedIngredientsDTO
    {
        public int Id
        {
            get; set;
        }

        public int IdIngredient
        {
            get; set;
        }

        public int IdProduct
        {
            get; set;
        }

        public byte AllowedNumber
        {
            get; set;
        }

        // Убраны навигационные свойства
        public ICollection<OrderItemIngredientDTO> OrderItemIngredients { get; set; } = new List<OrderItemIngredientDTO>();
    }
}
