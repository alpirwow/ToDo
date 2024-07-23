using BusinessLayer.Interfaces;
using CommonLayer.Models.Dto.Priority;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityController : ControllerBase
    {
        private readonly IPriorityService _priorityService;
        private readonly ILogger<PriorityController> _logger;

        public PriorityController(IPriorityService priorityService, ILogger<PriorityController> logger)
        {
            _priorityService = priorityService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<PriorityDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all priority levels.");
            var result = await _priorityService.GetAllAsync();
            _logger.LogInformation("Fetched {Count} priority levels.", result.Count);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddAsync([FromBody] PriorityDto dto)
        {
            _logger.LogInformation("Attempting to add priority level: {Level}", dto.Level);
            await _priorityService.AddAsync(dto);
            _logger.LogInformation("Priority level {Level} added successfully.", dto.Level);
            return Ok();
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateAsync([FromBody] PriorityDto dto)
        {
            _logger.LogInformation("Attempting to update priority level: {Level}", dto.Level);
            await _priorityService.UpdateAsync(dto);
            _logger.LogInformation("Priority level {Level} updated successfully.", dto.Level);
            return Ok();
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int id)
        {
            var dto = new PriorityDto() { Level = id };
            _logger.LogInformation("Attempting to delete priority level: {Level}", dto.Level);
            await _priorityService.DeleteByIdAsync(dto);
            _logger.LogInformation("Priority level {Level} deleted successfully.", dto.Level);
            return Ok();
        }
    }
}
