
namespace OrderWebsite.Domain.Entities
{
    public class LogDataEntity
    {
        public int Id {get;set;}
        public string Sender { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string TypeOfMessage { get; set; } = string.Empty;
        public DateTime Time { get; set; }
    }
}