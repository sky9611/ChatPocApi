using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatPocApi.Models
{
    public class UserChannelModel
    {
        public UserModel User { get; set; }
        public ChannelModel Channel { get; set; }
    }
}
