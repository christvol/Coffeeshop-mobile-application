namespace Common.Classes.DTO
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
        public DateTime? CreationDate
        {
            get; set;
        }
        public List<OrderItemsDTO> OrderItems
        {
            get; set;
        }
    }
}
