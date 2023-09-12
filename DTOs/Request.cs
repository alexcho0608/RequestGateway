namespace DTOs
{
    public class Request
    {
        public string RequestId { get; set; }

        public string SessionId { get; set; }

        public DateTime? Datetime { get; set; }

        public long? UserId { get; set; }
    }
}