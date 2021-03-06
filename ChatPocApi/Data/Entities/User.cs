﻿using System.Collections.Generic;

namespace ChatPocApi.Data.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public ICollection<UserChannel> UserChannels { get; set; }

    }
}
