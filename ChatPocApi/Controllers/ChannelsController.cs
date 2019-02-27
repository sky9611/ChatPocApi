using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChatPocApi.Data;
using ChatPocApi.Data.Entities;
using ChatPocApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ChatPocApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelsController : ControllerBase
    {
        private readonly IChatPocRepository _chatPocRepository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public ChannelsController(IChatPocRepository chatPocRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _chatPocRepository = chatPocRepository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<ChannelModel[]>> Get(bool includeMessages = false, bool includeUsers = false)
        {
            try
            {
                var results = await _chatPocRepository.GetAllChannelsAsync(includeMessages, includeUsers);
                return _mapper.Map<ChannelModel[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }
        }

        [HttpGet("{channelName}")]
        public async Task<ActionResult<ChannelModel>> Get(string channelName, bool includeMessages = false, bool includeUsers = false)
        {
            try
            {
                var results = await _chatPocRepository.GetChannelByNameAsync(channelName, includeMessages, includeUsers);
                return _mapper.Map<ChannelModel>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ChannelModel>> Post(CreatingChannelModel model)
        {
            try
            {
                var existing = await _chatPocRepository.GetChannelByNameAsync(model.Name);
                if (existing != null)
                    return BadRequest("Channel name in use");

                string location = _linkGenerator.GetPathByAction("Get", "Channels", new { channelName = model.Name });

                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current channel name");
                }
                

                if (await _chatPocRepository.CreateChannelAsync(model.Name, model.Users))
                {
                    return Created(location, _mapper.Map<ChannelModel>(await _chatPocRepository.GetChannelByNameAsync(model.Name)));
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }

            return BadRequest();
        }

        //[HttpPatch("{channelName}")]
        //public async Task<ActionResult<ChannelModel>> Patch(string channelName, JsonPatchDocument<ChannelModel> patchDoc)
        //{
        //    try
        //    {
        //        if (patchDoc == null)
        //            return BadRequest("Content is null");
        //        var channel = await _chatPocRepository.GetChannelByNameAsync(channelName);
        //        if (channel == null) return NotFound($"Could not find channel named {channelName}");
        //        var channelModel = _mapper.Map<ChannelModel>(channel);
        //        patchDoc.ApplyTo(channelModel);
        //        _mapper.Map(channelModel, channel);
        //        if (await _chatPocRepository.SaveChangesAsync())
        //        {
        //            return _mapper.Map<ChannelModel>(channel);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
        //    }

        //    return BadRequest();
        //}

        [HttpDelete("{channelName}")]
        public async Task<IActionResult> Delete(string channelName)
        {
            try
            {
                var channelToBeDeleted = await _chatPocRepository.GetChannelByNameAsync(channelName);
                if (channelToBeDeleted == null) return NotFound($"Could not find channel named {channelName}");

                _chatPocRepository.Delete(channelToBeDeleted);

                if (await _chatPocRepository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DataBase Failure");
            }
            return BadRequest("Failed to delete");
        }
    }
}