using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatPocApi.Models
{
    public class CreatingChannelModel
    {
        public string Name { get; set; }
        public ICollection<string> Users { get; set; }
    }
}
