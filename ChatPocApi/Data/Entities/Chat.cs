using System.Collections.Generic;

namespace ChatPocApi.Data.Entities
{
    public class Chat
    {
        public int User1Id { get; set; }
        public User User1 { get; set; }
        public int User2Id { get; set; }
        public User User2 { get; set; }
        public ICollection<Message> Messages {get; set;}

    }
}