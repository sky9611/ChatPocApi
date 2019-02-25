using System.Collections.Generic;

namespace ChatPocApi.Data.Entities
{
    public class Chat
    {
        public int ChatID { get; set; }
        public User User1 { get; set; }
        public User User2 { get; set; }
        public ICollection<Message> Messages {get; set;}

    }
}