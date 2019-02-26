using System;

namespace ChatPocApi.Data.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public User Sender { get; set; }
        public string Content { get; set; }
        public DateTime MsgDate { get; set; } = DateTime.MinValue;
    }
}