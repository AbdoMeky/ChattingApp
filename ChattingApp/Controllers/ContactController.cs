using ChattingApp.CORE.DTO.ContactDTO;
using ChattingApp.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;
        public ContactController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        [HttpGet("GetContact/{id:int}")]
        public ActionResult GetById(int id)
        {
            var contant =_contactRepository.GetById(id);
            if(contant is null)
            {
                return BadRequest("No contact has this Id");
            }
            return Ok(contant);
        }
        [HttpGet("GetAllContactOfUser/{id}")]
        public ActionResult GetContactForUser(string id) 
        {
            var contacts=_contactRepository.GetAllContactForUserById(id);
            if(contacts is null)
            {
                return BadRequest("No User has this Id");
            }
            return Ok(contacts);
        }
        [HttpPost]
        public ActionResult Add(AddContactDTO contact)
        {
            if (ModelState.IsValid)
            {
                var result = _contactRepository.Add(contact);
                if (result.Id == 0)
                {
                    return BadRequest(result.Message);
                }
                var url = Url.Action(nameof(GetById), new { id = result.Id });
                return Created(url, _contactRepository.GetById(result.Id));
            }
            return BadRequest(ModelState);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var result=_contactRepository.Delete(id);
            if(result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
