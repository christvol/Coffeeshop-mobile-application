namespace REST_API_SERVER.DTO
{
    public class OrderItemsDTO
    {
        public int? IdProduct
        {
            get; set;
        }
        public float Total
        {
            get; set;
        }
        public List<OrderItemIngredientDTO> Ingredients
        {
            get; set;
        }
    }
}
