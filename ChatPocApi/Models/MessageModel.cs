﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ChatPocApi.Models
{
    public class MessageModel
    {
        public UserModel Sender { get; set; }
        public UserModel Recevier { get; set; }
        public string Content { get; set; }
        [Required]
        public DateTime MsgDate { get; set; } = DateTime.MinValue;
    }
}