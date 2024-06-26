using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBackend.Entities;
using MyBackend.Models;
using MyBackend.Services;

namespace MyBackend.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        public SessionsController(IBackendRepository repos, IMapper mapper)
        {
            _repos = repos;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<SessionModel>>> GetSessions()
        {
            var sessions = await _repos.GetSessionsAsync();
            return Ok(_mapper.Map<IEnumerable<SessionModel>>(sessions));
        }

        [HttpGet("sessionService")]
        public async Task<ActionResult<IEnumerable<SessionForService>>> GetSessionsForService()
        {
            var sessions = await _repos.GetSessionsAsync();
            return Ok(_mapper.Map<IEnumerable<SessionForService>>(sessions));
        }

        [HttpGet("playerService")]
        public async Task<ActionResult<IEnumerable<PlayerForService>>> GetPlayersForService()
        {
            var players = await _repos.GetPlayersAsync();
            return Ok(_mapper.Map<IEnumerable<PlayerForService>>(players));
        }

        [HttpGet("gmService")]
        public async Task<ActionResult<IEnumerable<GmForService>>> GetGmsForService()
        {
            var gms = await _repos.GetGmsAsync();
            return Ok(_mapper.Map<IEnumerable<GmForService>>(gms));
        }

        [HttpPost("addSession")]
        public async void AddSession(SessionPost sessionPosted)
        {
            //Added by Gm1, will be added by currently logged in Gm
            var gms = await _repos.GetGmsAsync();
            var gm = gms.FirstOrDefault(lol => lol.Name == "Gm1");

            //var sessionToAdd = _mapper.Map<Session>(sessionPosted);
            var sessionToAdd = new Session {
                Id = Guid.NewGuid().ToString("N"),
                Gm = gm,
                GmId = gm.Id,
                Players = [],
                Title = sessionPosted.Title,
                Description = sessionPosted.Description,
                System = sessionPosted.System,
                Tags = sessionPosted.Tags,
                MaxNumberOfPlayers = sessionPosted.MaxNumberOfPlayers,
            };
            /*
            sessionToAdd.Id = Guid.NewGuid().ToString("N");
            sessionToAdd.Gm = gm;
            sessionToAdd.GmId = gm.Id;
            sessionToAdd.Players = [];
            */
            _repos.AddSession(sessionToAdd);
        }
    }
}
