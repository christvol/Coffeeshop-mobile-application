namespace Common.Classes.DTO
{
    public class OrderProductDTO
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int IdProduct
        {
            get; set;
        }

        /// <summary>
        /// Количество продукта в заказе.
        /// </summary>
        public int Quantity
        {
            get; set;
        }
    }
}
