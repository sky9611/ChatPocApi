using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatPocApi.Data.Entities
{
    public class UserChannel
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
