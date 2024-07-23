using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.ToDoItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private readonly IToDoItemService _toDoItemService;
        private readonly ILogger<ToDoItemController> _logger;

        public ToDoItemController(IToDoItemService toDoItemService, ILogger<ToDoItemController> logger)
        {
            _toDoItemService = toDoItemService;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddAsync([FromBody] ToDoItemPostDto dto)
        {
            _logger.LogInformation("Attempting to add ToDo item with title: {Title}", dto.Title);
            await _toDoItemService.AddAsync(dto);
            _logger.LogInformation("ToDo item with title {Title} added successfully.", dto.Title);
            return Ok();
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAsync([FromBody] ToDoItemPutDto dto)
        {
            _logger.LogInformation("Attempting to update ToDo item with id: {Id}", dto.Id);
            await _toDoItemService.UpdateAsync(dto);
            _logger.LogInformation("ToDo item with id {Id} updated successfully.", dto.Id);
            return Ok();
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> CompliteAsync([FromRoute] Guid id)
        {
            _logger.LogInformation("Attempting to update ToDo item with id: {Id}", id);
            await _toDoItemService.CompliteAsync(id);
            _logger.LogInformation("ToDo item with id {Id} updated successfully.", id);
            return Ok();
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] Guid id)
        {
            _logger.LogInformation("Attempting to delete ToDo item with id: {Id}", id);
            await _toDoItemService.DeleteByIdAsync(id);
            _logger.LogInformation("ToDo item with id {Id} deleted successfully.", id);
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ToDoItemGetDto>?>> GetWithConditionsAsync([FromQuery] bool? isGetCompleted, int? priorityLevel, Guid? userId)
        {
            _logger.LogInformation("Fetching ToDo items with conditions: isGetCompleted={IsGetCompleted}, priorityLevel={PriorityLevel}, userId={UserId}", isGetCompleted, priorityLevel, userId);
            var result = await _toDoItemService.GetWithConditionsAsync(isGetCompleted, priorityLevel, userId);
            _logger.LogInformation("Fetched {Count} ToDo items with conditions.", result.Count);
            return Ok(result);
        }

        [HttpGet("get-my-todo")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ToDoItemGetDto>?>> GetMyWithConditionsAsync([FromQuery] bool? isGetCompleted, int? priorityLevel)
        {
            _logger.LogInformation("Fetching ToDo items with conditions: isGetCompleted={IsGetCompleted}, priorityLevel={PriorityLevel}", isGetCompleted, priorityLevel);
            var result = await _toDoItemService.GetMyToDoWithConditionsAsync(isGetCompleted, priorityLevel);
            _logger.LogInformation("Fetched {Count} ToDo items with conditions.", result.Count);
            return Ok(result);
        }

        [HttpGet("get-by-id")]
        [AllowAnonymous]
        public async Task<ActionResult<ToDoItemGetDto?>> GetByIdAsync([FromQuery] Guid id)
        {
            _logger.LogInformation("Fetching ToDo item with id: {Id}", id);
            var result = await _toDoItemService.GetByIdAsync(id);

            if (result is not null)            
                _logger.LogInformation("ToDo item with id {Id} found.", id);            
            else            
                _logger.LogWarning("ToDo item with id {Id} not found.", id);
            
            return Ok(result);
        }
    }

}
