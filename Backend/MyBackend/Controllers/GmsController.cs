using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MyBackend.Entities;
using MyBackend.Models;
using MyBackend.Services;

namespace MyBackend.Controllers
{
    [Route("api/gms")]
    [ApiController]
    public class GmsController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly FileExtensionContentTypeProvider _fileExtentionContentTypeProvider;
        public GmsController(IBackendRepository repos, IMapper mapper, UserManager<User> userManager,
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _repos = repos;
            _mapper = mapper;
            _userManager = userManager;
            _fileExtentionContentTypeProvider = fileExtensionContentTypeProvider ??
                throw new System.ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("gmForSession")]
        public async Task<ActionResult<Gm>> GetGmForSession(
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

                var user = users.FirstOrDefault(u => u.GmOrPlayerId.Equals(session.GmId));

                var results = new GmWithDiscordAndPicture();

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
                string playerPictureContents = Convert.ToBase64String(playerPicture.FileContents);
                string picToSend = "data:image/png;base64," + playerPictureContents;
                results = new GmWithDiscordAndPicture
                {
                    Name = user.UserName,
                    Email = user.Email,
                    ProfilePicture = picToSend,
                    Discord = user.Discord
                };

                return Ok(results);
            }

            catch
            {
                return BadRequest();
            }
        }
    }
}
