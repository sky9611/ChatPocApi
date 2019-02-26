using System.Collections.Generic;

namespace ChatPocApi.Data.Entities
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserChannel> UserChannels { get; set; }
        public ICollection<Message> Messages { get; set; }

    }
}