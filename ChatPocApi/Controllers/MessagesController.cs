using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MessagesController : ControllerBase
    {
        private readonly IChatPocRepository _chatPocRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public MessagesController(IChatPocRepository chatPocRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _chatPocRepository = chatPocRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{senderName}/{msgDate}")]
        public async Task<ActionResult<MessageModel>> Get(string senderName, DateTime msgDate)
        {
            try
            {
                var result = await _chatPocRepository.GetMessageAsync(senderName, msgDate);
                return _mapper.Map<MessageModel>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MessageModel>> Post(CreatingMessageModel model)
        {
            try
            {
                var sender = await _chatPocRepository.GetUserAsync(model.SenderName);
                if (sender == null)
                    return BadRequest("sender doesn't exist");

                var channel = await _chatPocRepository.GetChannelByNameAsync(model.ChannelName);
                if (channel == null)
                    return BadRequest("channel doesn't exist");

                string location = _linkGenerator.GetPathByAction("Get", "Messages", new { senderName = model.SenderName, msgDate = model.MsgDate });

                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current channel name");
                }

                DateTime dtMsgDate = DateTime.ParseExact(model.MsgDate, "yyyy-MM-ddTHH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);

                if (await _chatPocRepository.PostMessageAsync(model.SenderName, model.ChannelName, model.Content, dtMsgDate))
                {
                    return Created(location, _mapper.Map<MessageModel>(await _chatPocRepository.GetMessageAsync(model.SenderName, dtMsgDate)));
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }

            return BadRequest();
        }
    }
}