namespace REST_API_SERVER.Controllers
{
    public class OrderDTO
    {
        public int Id
        {
            get; set;
        }
        public int? IdCustomer
        {
            get; set;
        }
        public int? IdEmployee
        {
            get; set;
        }
        public int IdStatus
        {
            get; set;
        }
        public List<OrderItemsDTO> OrderItems
        {
            get; set;
        }
    }
}
