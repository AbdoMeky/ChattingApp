using ChattingApp.CORE.DTO.TwosomeChatDTO;
using ChattingApp.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwosomeChatController : ControllerBase
    {
        private readonly ITwosomeChatRepository _chatRepository;
        public TwosomeChatController(ITwosomeChatRepository chatRepository)
        {
            _chatRepository= chatRepository;
        }
        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            var chat=_chatRepository.GetById(id);
            if (chat is null)
            {
                return BadRequest("No Twosome chat has this Id");
            }
            return Ok(chat);
        }
        [HttpGet("GetByUsersId/{UserOneId}/{UserTwoId}")]
        public ActionResult GetByUsersId(string UserOneId,string UserTwoId)
        {
            var chat = _chatRepository.Get(UserOneId,UserTwoId);
            if (chat is null)
            {
                return BadRequest("No Twosome chat has this Id");
            }
            return Ok(chat);
        }
        [HttpGet("WithMessages/{id:int}")]
        public ActionResult GetWithMessages(int id)
        {
            var chat = _chatRepository.GetWithMessages(id);
            if (chat is null)
            {
                return BadRequest("No Twosome chat has this Id");
            }
            return Ok(chat);
        }
        [HttpPost]
        public ActionResult Add(AddTwosomeChatDTO chatUsers)
        {
            if (ModelState.IsValid)
            {
                var result = _chatRepository.Add(chatUsers);
                if (result.Id == 0)
                {
                    return BadRequest(result.Message);
                }
                var url = Url.Action(nameof(Get), new { id = result.Id });
                return Created(url, _chatRepository.GetById(result.Id));
            }
            return BadRequest(ModelState);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var result=_chatRepository.Delete(id);
            if(result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
