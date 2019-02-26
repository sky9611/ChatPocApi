using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatPocApi.Models
{
    public class UserModel
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(50)]
        public string ProfilePicture { get; set; }
        public ICollection<UserChannelModel> userChannels { get; set; }
    }
}
