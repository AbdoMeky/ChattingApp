using ChattingApp.CORE.DTO.User;
using ChattingApp.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChattingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result= _userRepository.GetUser(id);
            if (result is null)
            {
                return BadRequest("No user has this Id");
            }
            return Ok(result);
        }
        [HttpGet("Search/{searchKey}")]
        public IActionResult Search(string searchKey)
        {
            var result=_userRepository.Search(searchKey);
            return Ok(result);
        }
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var result=_userRepository.Delete(id);
            if (result.Id == 0)
            {
                return BadRequest(result.Message);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpPatch("ChangeName")]
        public IActionResult ChangeName(UpdateNameDTO user)
        {
            var result=_userRepository.ChangeName(user);
            if (!string.IsNullOrEmpty(result.Message))
            {
                return BadRequest(result.Message);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
        [HttpPatch("ChangeProfilePictureUrl")]
        public async Task<IActionResult> ChangeProfilePicture(UpdateProfilePictureUrlDTO user)
        {
            var result =await _userRepository.ChangeProfilePictureUrl(user);
            if (!string.IsNullOrEmpty(result.Message))
            {
                return BadRequest(result.Message);
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
