namespace Common.Classes.DTO
{
    public class OrderItemIngredientDTO
    {
        public int Id
        {
            get; set;
        }
        public int? IdOrderProduct
        {
            get; set;
        }
        public int? IdIngredient
        {
            get; set;
        }
        public int Amount
        {
            get; set;
        }
    }
}
