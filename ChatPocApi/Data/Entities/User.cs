using System.Collections.Generic;

namespace ChatPocApi.Data.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Profileicture { get; set; }
        public ICollection<Chat> Chats { get; set; }

    }
}
