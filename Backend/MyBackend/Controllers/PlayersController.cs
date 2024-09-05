using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MyBackend.Entities;
using MyBackend.Models;
using MyBackend.Services;
using System.Collections;

namespace MyBackend.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly FileExtensionContentTypeProvider _fileExtentionContentTypeProvider;
        public PlayersController(IBackendRepository repos, IMapper mapper, UserManager<User> userManager,
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _repos = repos;
            _mapper = mapper;
            _userManager = userManager;
            _fileExtentionContentTypeProvider = fileExtensionContentTypeProvider ??
                throw new System.ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("playersService")]
        public async Task<ActionResult<IEnumerable<PlayerForService>>> GetPlayersForService(
            string title)
        {
            try
            {
                if (string.IsNullOrEmpty(title))
                    return BadRequest();
                
                var users = await _repos.GetUsersAsync();
                if (users.Count() == 0)
                {
                    return BadRequest();
                }
                var session = await _repos.GetSessionAsyncTitle(title);
                if (session == null)
                {
                    return BadRequest();
                }

                List<User> usersPlayersInSession = [];
                foreach (var player in session.Players)
                {
                    var user = await _repos.GetUserByUserName(player.Name);
                    usersPlayersInSession.Add(user);
                }

                List<PlayerForService> results = [];
                foreach (var user in usersPlayersInSession)
                {
                    if (!System.IO.File.Exists(user.ProfilePicturePath))
                    {
                        return NotFound();
                    }

                    if (!_fileExtentionContentTypeProvider.TryGetContentType(
                        user.ProfilePicturePath, out var contentType))
                    {
                        contentType = "image/png";
                    }

                    var bytes = System.IO.File.ReadAllBytes(user.ProfilePicturePath);
                    var playerPicture = File(bytes, contentType, Path.GetFileName(
                        user.ProfilePicturePath));
                    string  playerPictureContents = Convert.ToBase64String(playerPicture.FileContents);
                    string  picToSend = "data:image/png;base64," + playerPictureContents;
                    results.Add(new PlayerForService
                    {
                        Name = user.UserName,
                        Email = user.Email,
                        ProfilePicture = picToSend,
                        Discord = user.Discord
                    }); 
                }
                return Ok(results);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPatch("leaveSession")]
        public async Task<ActionResult> LeaveSession(LeaveSessionBody body)
        {
            if(body == null)
            {
                return BadRequest();
            }

            var session = await _repos.GetSessionAsyncTitle(body.SessionTitle);
            if (session == null)
                return NotFound();

            var playerToRemove = await _repos.GetPlayerAsyncName(body.UserName);
            if (playerToRemove == null)
                return NotFound();

            foreach (var player in session.Players)
            {
                if (player.Name == playerToRemove.Name)
                {
                    //nwm czy usunie, bo porównuje referencje
                    var removedPlayer = session.Players.Remove(player);
                    _repos.SaveContextChanges();
                    return Ok();
                }
            }
            return BadRequest();

        }
    }
}
