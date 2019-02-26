using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChatPocApi.Data.Entities;
using ChatPocApi.Models;

namespace ChatPocApi.Data
{
    public class MyProfile: Profile
    {
        public MyProfile()
        {
            CreateMap<Channel, ChannelModel>().ReverseMap();
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Message, MessageModel>().ReverseMap();
            CreateMap<UserChannel, UserChannelModel>().ReverseMap();
        }
    }
}
