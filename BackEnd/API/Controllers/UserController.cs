using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddAsync([FromBody] UserPostDto user)
        {
            _logger.LogInformation("Attempting to add user with name: {Name}", user.Name);
            await _userService.AddAsync(user);
            _logger.LogInformation("User with name {Name} added successfully.", user.Name);
            return Ok();
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] Guid id)
        {
            _logger.LogInformation("Attempting to delete user with id: {Id}", id);
            await _userService.DeleteByIdAsync(id);
            _logger.LogInformation("User with id {Id} deleted successfully.", id);
            return Ok();
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAsync([FromBody] UserPutDto dto)
        {
            _logger.LogInformation("Attempting to update user with id: {Id}", dto.Id);
            await _userService.UpdateAsync(dto);
            _logger.LogInformation("User with id {Id} updated successfully.", dto.Id);
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<UserGetDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all users.");
            var result = await _userService.GetAllAsync();
            _logger.LogInformation("Fetched {Count} users.", result.Count);
            return Ok(result);
        }

        [HttpGet("get-by-id")]
        [AllowAnonymous]
        public async Task<ActionResult<UserGetDto>> GetByIdAsync([FromQuery] Guid id)
        {
            _logger.LogInformation("Fetching user with id: {Id}", id);

            var result = await _userService.GetByIdAsync(id);
            if (result is not null)            
                _logger.LogInformation("User with id {Id} found.", id);            
            else            
                _logger.LogWarning("User with id {Id} not found.", id);
            
            return Ok(result);
        }

        [HttpGet("get-with-info/all")]
        [AllowAnonymous]
        public async Task<ActionResult<List<UserGetDto>>> GetAllFullAsync()
        {
            _logger.LogInformation("Fetching all users with full details.");
            var result = await _userService.GetAllFullAsync();
            _logger.LogInformation("Fetched {Count} users with full details.", result.Count);
            return Ok(result);
        }

        [HttpGet("get-with-info")]
        [AllowAnonymous]
        public async Task<ActionResult<UserGetDto>> GetByIdFullAsync([FromQuery] Guid id)
        {
            _logger.LogInformation("Fetching user with id: {Id} and full details.", id);
            var result = await _userService.GetByIdFullAsync(id);

            if (result is not null)            
                _logger.LogInformation("User with id {Id} found.", id);            
            else            
                _logger.LogWarning("User with id {Id} not found.", id);
            
            return Ok(result);
        }
    }
}
