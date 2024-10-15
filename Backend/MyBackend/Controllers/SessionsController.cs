using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Any;
using MyBackend.Entities;
using MyBackend.Models;
using MyBackend.Services;
using System.Reflection.Metadata;

namespace MyBackend.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly FileExtensionContentTypeProvider _fileExtentionContentTypeProvider;
        public SessionsController(IBackendRepository repos, IMapper mapper,
            UserManager<User> userManager, SignInManager<User> signInManager,
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _repos = repos;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _fileExtentionContentTypeProvider = fileExtensionContentTypeProvider ??
                throw new System.ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<SessionModel>>> GetSessions()
        {
            var sessions = await _repos.GetSessionsAsync();
            return Ok(_mapper.Map<IEnumerable<SessionModel>>(sessions));
        }

        [HttpGet("{sessionId}", Name = "GetPlayer")]
        public async Task<ActionResult<SessionModel>> GetSession(string sessionId)
        {
            var sessions = await _repos.GetSessionsAsync();
            var session = sessions.FirstOrDefault(s => s.Id.Equals(sessionId));
            if (session == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<SessionModel>(session));
        }

        [HttpGet("sessionService")]
        public async Task<ActionResult<IEnumerable<SessionForService>>> GetSessionsForService()
        {
            try
            {
                var sessions = await _repos.GetSessionsAsync();
                var result = _mapper.Map<IEnumerable<SessionForService>>(sessions);
                //.Where(s => s.CurrentNumberOfPlayers < s.MaxNumberOfPlayers)
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
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

        [HttpGet("sessionPicture")]
        public async Task<ActionResult> GetSessionPicture(string title)
        {
            Session session = _repos.GetSessionAsyncTitle(title).Result;
            var gm = _repos.GetGmAsync(session.GmId).Result;
            var gmUser = _repos.GetUserByGmOrPlayerId(gm.Id.ToString()).Result;
            var gmPicturePath = gmUser.ProfilePicturePath;
            if (session == null)
            {
                return NotFound();

            }
            if (!System.IO.File.Exists(session.PicturePath) ||
                !System.IO.File.Exists(gmPicturePath))
            {
                return NotFound();
            }

            if (!_fileExtentionContentTypeProvider.TryGetContentType(
                session.PicturePath, out var contentType))
            {
                contentType = "image/png";
            }

            var bytes = System.IO.File.ReadAllBytes(session.PicturePath);
            var sessionPicture = File(bytes, contentType, Path.GetFileName(session.PicturePath));
            bytes = System.IO.File.ReadAllBytes(gmPicturePath);
            var gmPicture = File(bytes, contentType, Path.GetFileName(gmPicturePath));

            var response = new { sessionPicture = sessionPicture, gmPicture = gmPicture };

            if (sessionPicture.FileContents.Length == 0 ||
                sessionPicture.FileContents.Length > 20971520)
            {
                return BadRequest("Wrong file");
            }

            else
            {
                return Ok(response);
            }

        }

        [HttpPost("addSession")]
        public async Task<ActionResult<ParamsAddSessionResponse>> AddSession(SessionPost sessionPosted)
        {
            try
            {
                bool errorHappened = false;
                if (sessionPosted == null)
                {
                    return BadRequest();
                }

                var result = new ParamsAddSessionResponse();

                var systems = await _repos.GetSystemsAsync();
                var index = systems.FirstOrDefault(s => s.Name == sessionPosted.System.Name);
                if (index == null)
                {
                    errorHappened = true;
                    result.IsSystemNew = true;
                }
                else
                {
                    if (index.Name.Equals(""))
                        errorHappened = true;
                }

                if (sessionPosted.Title.Equals(""))
                {
                    errorHappened = true;
                    result.IsTitleUnique = true;
                }

                var title = await _repos.GetSessionAsyncTitle(sessionPosted.Title);
                if (title != null)
                {
                    errorHappened = true;
                    result.IsTitleUnique = false;
                }
                if (errorHappened)
                    return Ok(result);

                var gms = await _repos.GetGmsAsync();
                systems = await _repos.GetSystemsAsync();

                var gm = gms.FirstOrDefault(lol => lol.Name == sessionPosted.GmUserName);
                var system = systems.FirstOrDefault(s => s.Name.Equals(sessionPosted.System.Name));

                byte[] fileBytes = Convert.FromBase64String(sessionPosted.Picture);

                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(), $"SessionPictures\\uploaded_file_{Guid.NewGuid()}.png");

                System.IO.File.WriteAllBytes(filePath, fileBytes);

                var sessionToAdd = new Session
                {
                    Id = Guid.NewGuid().ToString("N"),
                    GmId = gm.Id,
                    Gm = gm,
                    Players = [],
                    Title = sessionPosted.Title,
                    Description = sessionPosted.Description,
                    SystemId = system.Id,
                    System = system,
                    Tags = sessionPosted.Tags,
                    PicturePath = filePath,
                    MaxNumberOfPlayers = sessionPosted.MaxNumberOfPlayers,
                };
                _repos.AddSession(sessionToAdd);
                return Ok();
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new session record");
            }
        }

        [HttpPatch("addPlayerToSession")]
        public async Task<ActionResult> AddPlayerForSession(AddPlayerToSessionBody body)
        {
            try
            {
                var playerName = body.PlayerName;
                var sessionTitle = body.SessionTitle;
                _repos.AddPlayerToSession(sessionTitle, playerName);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("deleteSession")]
        public async Task<ActionResult> DeleteSession(string sessionTitle)
        {
            if (string.IsNullOrEmpty(sessionTitle))
            {
                return BadRequest();
            }

            _repos.DeleteSession(sessionTitle);
            return Ok();
        }

        [HttpGet("testDeploy")]
        public ActionResult TestDeploy()
        {
            var response = new
            {
                text = "Requests from deployed app work for deployed backend"
            };
            return Ok(response);
        }
    }
}
