namespace Hermes.Models
{
    public class Message
    {
        public string[] RegistrationIds { get; set; }
        public Notification Notification { get; set; }
        public object Data { get; set; }
    }
}