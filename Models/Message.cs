namespace Hermes.Models
{
    public class Message
    {
        public string to { get; set; }
        public Notification notification { get; set; }
        public AlertPayload data { get; set; }
    }
}