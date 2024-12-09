using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyBackend.Entities;
using MyBackend.Models;
using MyBackend.Services;

namespace MyBackend.Controllers
{
    [Route("api/gameSystems")]
    [ApiController]
    public class GameSystemController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        public GameSystemController(IBackendRepository repos, IMapper mapper)
        {
            _repos = repos;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<GameSystemModel>>> GetSystems()
        {
            var systems = await _repos.GetSystemsAsync();
            var result = _mapper.Map<IEnumerable<GameSystemModel>>(systems);
            return Ok(result);
        }

        [HttpPost("addSystem")]
        public async Task<ActionResult<GameSystem>> AddSystem(GameSystemModel systemPosted)
        {
            try
            {
                if (systemPosted == null)
                {
                    return BadRequest();
                }
                var systems = await _repos.GetSystemsAsync();
                var systemToAdd = new GameSystem
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = systemPosted.Name
                };
                _repos.AddSystem(systemToAdd);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new session record");
            }
        }
        [HttpDelete("deleteSystem")]
        public ActionResult DeleteSystem(string systemName)
        {
            try
            {
                if (systemName == null || systemName == string.Empty)
                {
                    return BadRequest();
                }
                _repos.DeleteSystem(systemName);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
