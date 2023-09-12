namespace DTOs.Models.JsonAPI
{
    public class InsertRequest
    {
        public string RequestId { get; set; }
        public long Timestamp { get; set; }
        public int ProducerId { get; set; }
        public string SessionId { get; set; }
    }
}
