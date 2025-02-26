using ChattingApp.CORE.DTO.MessageDTO;
using ChattingApp.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var message=_messageRepository.Get(id);
            if(message is null)
            {
                return BadRequest("No message has this Id");
            }
            return Ok(message);
        }
        [HttpPost]
        public async Task<ActionResult> Add(AddMessageDTO message)
        {
            if(ModelState.IsValid)
            {
                var result=await _messageRepository.Add(message);
                if(result.Id == 0)
                {
                    return BadRequest(result.Message);
                }
                var url = Url.Action(nameof(Get), new {id = result.Id });
                return Created(url,_messageRepository.Get(result.Id));
            }
            return BadRequest(ModelState);
        }
        [HttpPatch("DeleteForAll/{id:int}")]
        public ActionResult Delete(int id)
        {
            var result= _messageRepository.Delete(id);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
