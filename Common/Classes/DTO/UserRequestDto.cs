namespace Common.Classes.DTO
{
    public class UserRequestDto
    {
        public int Id
        {
            get; set;
        }
        public int IdUserType
        {
            get; set;
        }
        public string FirstName
        {
            get; set;
        }
        public string LastName
        {
            get; set;
        }
        public DateTime BirthDate
        {
            get; set;
        }
        public string Email
        {
            get; set;
        }
        public string PhoneNumber
        {
            get; set;
        }
    }

}
