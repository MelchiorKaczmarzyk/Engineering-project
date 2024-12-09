using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBackend.Entities;
using MyBackend.Models;
using MyBackend.Services;

namespace MyBackend.Controllers
{
    [Route("api/triggers")]
    [ApiController]
    public class TriggersController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        public TriggersController(IBackendRepository repos, IMapper mapper)
        {
            _repos = repos;
            _mapper = mapper;
        }
        [HttpGet("triggerService")]
        public async Task<ActionResult<IEnumerable<TriggerModel>>> GetTriggersForService()
        {
            try
            {
                var triggers = await _repos.GetTriggersAsync();
                var result = _mapper.Map<IEnumerable<TriggerModel>>(triggers);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("addTrigger")]
        public ActionResult AddTrigger(TriggerModel trigger)
        {
            try
            {
                if (trigger == null)
                {
                    return BadRequest();
                }
                var triggerToAdd = new Trigger
                {
                    Id = new Guid().ToString(),
                    Name = trigger.Name,
                };
                _repos.AddTrigger(triggerToAdd);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("deleteTrigger")]
        public ActionResult DeleteTrigger(string triggerName)
        {
            try
            {
                if (triggerName == null || triggerName == string.Empty)
                {
                    return BadRequest();
                }
                _repos.DeleteTrigger(triggerName);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }
    }

}
