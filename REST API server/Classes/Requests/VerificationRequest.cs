namespace REST_API_SERVER.Classes.Requests
{
    public class VerificationRequest
    {
        public string PhoneNumber
        {
            get; set;
        }
        public string Code
        {
            get; set;
        }
        public int UserTypeId
        {
            get; set;
        }
    }
}
