
namespace Auction.Domain.Entities
{
    public class LogDataEntity
    {
        public int Id {get;set;}
        public string Sender { get; set; }
        public string Message { get; set; }
        public string TypeOfMessage { get; set; }
        public DateTime Time { get; set; }
    }
}