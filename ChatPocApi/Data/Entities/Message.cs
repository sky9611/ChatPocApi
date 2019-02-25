namespace ChatPocApi.Data.Entities
{
    public class Message
    {
        public int MessageID { get; set; }
        public User Sender { get; set; }
        public User Recevier { get; set; }
        public string Content { get; set; }
    }
}