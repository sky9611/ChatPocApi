using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatPocApi.Models
{
    public class ChannelModel
    {
        public string Name { get; set; }
        public ICollection<UserChannelModel> userChannels { get; set; }
        public ICollection<MessageModel> Messages { get; set; }
    }
}
