using ChattingApp.CORE.DTO.GroupChatDTO;
using ChattingApp.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupChatController : ControllerBase
    {
        private readonly IGroupChatRepository _groupChatRepository;
        public GroupChatController(IGroupChatRepository groupChatRepository)
        {
            _groupChatRepository = groupChatRepository;
        }
        [HttpPost("AddGroupChat")]
        public async Task<ActionResult> Add(AddGroupChatDTO group)
        {
            if(ModelState.IsValid)
            {
                var result = await _groupChatRepository.AddAsync(group);
                if(result.Id == 0)
                {
                    return BadRequest(result.Message);
                }
                var url = Url.Action(nameof(Get), new {id=result.Id});
                return Created(url,_groupChatRepository.Get(result.Id));
            }
            return BadRequest(ModelState);
        }
        [HttpPatch("ChangeGroupName")]
        public async Task<ActionResult> Update(UpdateGroupChatDTO group)
        {
            if (ModelState.IsValid)
            {
                var result=await _groupChatRepository.Update(group);
                if (result.Id == 0)
                {
                    return BadRequest(result.Message);
                }
                return StatusCode(StatusCodes.Status204NoContent);
            }
            return BadRequest(ModelState);
        }
        [HttpGet("GetChatInformation/{id:int}")]
        public ActionResult Get(int id)
        {
            var group=_groupChatRepository.Get(id);
            if(group is null)
            {
                return BadRequest("No group has this Id");
            }
            return Ok(group);
        }
        [HttpGet("GetChatInformationWithMessages/{id:int}")]
        public ActionResult GetWithMessages(int id)
        {
            var group = _groupChatRepository.GetWithMessages(id);
            if (group is null)
            {
                return BadRequest("No group has this Id");
            }
            return Ok(group);
        }
        [HttpPatch("ConvertMemberToAdmin/{id:int}")]
        public ActionResult ConvertMemberToAdmin(int id)
        {
            var result=_groupChatRepository.AddAdmin(id);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpPatch("ConvertAdminToMember/{id:int}")]
        public ActionResult ConvertAdminToMember(int id)
        {
            var result = _groupChatRepository.DeleteAdmin(id);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpPost("AddMemberToChat/{chatId:int}/{userId}")]
        public ActionResult AddMemberToChat(string userId, int chatId)
        {
            var result=_groupChatRepository.AddMember(userId, chatId);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            string url = Url.Action(nameof(Get), new {id=chatId});
            return Created(url, _groupChatRepository.Get(chatId));
        }
        [HttpDelete("DeleteMemberFromChat/{id:int}")]
        public ActionResult DeleteMember(int id)
        {
            var result = _groupChatRepository.DeleteMember(id);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpDelete("DeleteGroupChat/{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _groupChatRepository.Delete(id);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
