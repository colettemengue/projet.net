namespace BlazorSignalRApp.Models
{

    public class Message
    {
        public int Id { get; set; } // Assuming Id is an integer and auto-generated
        public string Sender { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}