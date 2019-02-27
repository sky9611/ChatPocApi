using System;
using System.ComponentModel.DataAnnotations;

namespace ChatPocApi.Models
{
    public class CreatingMessageModel
    {
        [Required]
        public string SenderName { get; set; }
        [Required]
        public string ChannelName { get; set; }
        public string Content { get; set; }
        [Required]
        public string MsgDate { get; set; }
    }
}
