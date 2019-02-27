using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChatPocApi.Data;
using ChatPocApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ChatPocApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IChatPocRepository _chatPocRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public UsersController(IChatPocRepository chatPocRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _chatPocRepository = chatPocRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<UserModel[]>> Get(bool includeChannels = false)
        {
            try
            {
                var results = await _chatPocRepository.GetAllUsersAsync(includeChannels);
                return _mapper.Map<UserModel[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<UserModel>> Get(string userName, bool includeChannels = false)
        {
            try
            {
                var results = await _chatPocRepository.GetUserAsync(userName, includeChannels);
                return _mapper.Map<UserModel>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }
        }

        [HttpGet("{userName}/channels")]
        public async Task<ActionResult<ChannelModel[]>> GetChannel(string userName, bool includeMessages = false, bool includeUsers = false)
        {
            try
            {
                var results = await _chatPocRepository.GetChannelsByUserAsync(userName, includeMessages, includeUsers);
                return _mapper.Map<ChannelModel[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }
        }
    }
}
