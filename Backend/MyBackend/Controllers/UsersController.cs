using AutoMapper;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Any;
using MyBackend.Entities;
using MyBackend.Models;
using MyBackend.Services;
using System.Net.Mail;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SQLitePCL;

namespace MyBackend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IBackendRepository _repos;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly FileExtensionContentTypeProvider _fileExtentionContentTypeProvider;
        private readonly MailService _mailService;
        public UsersController(IBackendRepository repos, IMapper mapper, 
            UserManager<User> userManager, SignInManager<User> signInManager,
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider, 
            IOptions<MailService> mailService)
        {
            _repos = repos;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService.Value;
            _fileExtentionContentTypeProvider = fileExtensionContentTypeProvider ??
                throw new System.ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("logInUser")]
        public async Task<ActionResult> LogInUser(string email, string password)
        {
            var result = new UserLogInResponse();
            result = new UserLogInResponse
            {
                Email = "",
                UserName = "",
                Role = "",
                ProfilePicture = null,
                Discord = "",
                EmailIsCorrect = false,
                PasswordIsCorrect = false
            };

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                if (email.Equals(""))
                    result.EmailIsCorrect = true;

                result.PasswordIsCorrect = true;
                return Ok(result);
            }

            var match = await _signInManager.CheckPasswordSignInAsync(user, password, 
                lockoutOnFailure: false);
            if (!match.Succeeded)
            {
                result.EmailIsCorrect = true;
                if (password.Equals(""))
                    result.PasswordIsCorrect = true;

                return Ok(result);
            }
            if (match.Succeeded)
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
                var profilePicture = File(bytes, contentType, Path.GetFileName(user.ProfilePicturePath));
                result = new UserLogInResponse
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = user.Role,
                    ProfilePicture = profilePicture,
                    Discord = user.Discord,
                    EmailIsCorrect = true,
                    PasswordIsCorrect = true
                };
                return Ok(result);
            }

            return BadRequest("Invalid password.");
        }
        private bool IsValidEmail(string email)
        {
            if (email == "")
                return false;
            try
            {
                MailAddress mailAddress = new MailAddress(email);
                return true; // If no exception is thrown, the email format is valid
            }
            catch
            {
                return false; // If a FormatException is caught, the email format is invalid
            }
        }
        private bool IsValidPassword(string input)
        {
            if(input == "")
                return false;
            if (input.Length <= 8)
            {
                return false;
            }
            bool hasNonAlphanumeric = Regex.IsMatch(input, @"[\W_]");
            bool hasDigit = Regex.IsMatch(input, @"\d");
            bool hasUpperCase = Regex.IsMatch(input, @"[A-Z]");
            return hasNonAlphanumeric && hasDigit && hasUpperCase;
        }
        [HttpPost("registerUser")]
        public async Task<ActionResult<ParametersRegisterResponse>> RegisterUser(UserModel userPosted)
        {
            bool inputError = false;
            var response = new ParametersRegisterResponse();
                
            var user = await _userManager.FindByNameAsync(userPosted.UserName);
            if (userPosted.UserName.Equals(""))
                inputError = true;
            if (user != null)
            {
                inputError = true;
                if(!(userPosted.UserName.Equals("")))
                    response.UserNameIsCorrect = false;
            }
            if (!(IsValidEmail(userPosted.Email)))
            {
                inputError = true;
                if(!(userPosted.Email.Equals("")))
                    response.EmailIsCorrect = false;
            }
            else
            {
                user = await _userManager.FindByEmailAsync(userPosted.Email);
                if (user != null)
                {
                    inputError = true;
                    if(!(userPosted.Email.Equals("")))
                        response.EmailIsUnique = false;
                }
            } 
            if (!(IsValidPassword(userPosted.Password)))
            {
                inputError = true;
                if(!(userPosted.Password.Equals("")))
                    response.PasswordIsCorrect = false;
            }

            if (inputError)
                return Ok(response);

            Gm gm = new Gm();
            Player player = new Player();
            string gmOrPlayerId = "";

            byte[] fileBytes = Convert.FromBase64String(userPosted.ProfilePicture);

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(), $"ProfilePictures\\uploaded_file_{Guid.NewGuid()}.png");

            System.IO.File.WriteAllBytes(filePath, fileBytes);

            if (userPosted.Role.Equals("Gm"))
            {
                gm.Id = Guid.NewGuid().ToString();
                gm.Name = userPosted.UserName;
                gm.Sessions = [];

                gmOrPlayerId = gm.Id;

                var registerGmResult = registerGm(gm);
                if (!registerGmResult)
                    return BadRequest();
            }
            if (userPosted.Role.Equals("Player"))
            {
                player.Id = Guid.NewGuid().ToString();
                player.Name = userPosted.UserName;
                player.Sessions = [];

                gmOrPlayerId = player.Id;

                var registerPlayerResult = registerPlayer(player);
                if (!registerPlayerResult)
                    return BadRequest();
            }

            var userToRegister = new User {
                UserName = userPosted.UserName, 
                Email = userPosted.Email,
                Role = userPosted.Role,
                ProfilePicturePath = filePath,
                GmOrPlayerId = gmOrPlayerId,
                Discord = userPosted.Discord
            };
            var result = await _userManager.CreateAsync(userToRegister, userPosted.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        private bool registerGm(Gm gm)
        {
            if (ModelState.IsValid)
            {
                _repos.AddGm(gm);
                return true;
            }
            else { return false; }
        }

        private bool registerPlayer(Player player)
        {
            if (ModelState.IsValid)
            {
                _repos.AddPlayer(player);
                return true;
            }
            else { return false; }
            
        }

        [HttpGet("getUserGmPicture")]
        public async Task<ActionResult> GetUserGmPicture(string gmUserName)
        {
            User user = _repos.GetUserByUserName(gmUserName).Result;
            if (user == null)
            {
                return NotFound();

            }
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
            var userGmPicture = File(bytes, contentType, Path.GetFileName(user.ProfilePicturePath));

            if (userGmPicture.FileContents.Length == 0 ||
                userGmPicture.FileContents.Length > 20971520)
            {
                return BadRequest("Wrong file");
            }

            else
            {
                return Ok(userGmPicture);
            }
        }

        [HttpGet("relogInUser")]
        public async Task<ActionResult<UserModel>> GetUserGmPicture(string email, string userName)
        {
            if (email == null || userName == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return NotFound();
                }
            }

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
            var profilePicture = File(bytes, contentType, Path.GetFileName(user.ProfilePicturePath));
            var result = new UserLogInResponse
            {
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role,
                ProfilePicture = profilePicture,
                Discord = user.Discord
            };
            return Ok(result);

        }

        [HttpPatch("userName")]
        public async Task<ActionResult> PatchUserName(PatchUserNameBody body)
        {
            try
            {
                if(body.Email == string.Empty)
                {
                    return BadRequest();
                }
                bool errorHappened = false;
                bool userNameEmpty = false;
                bool userNameUnique = true;

                if (body.UserName.Equals(""))
                {
                    userNameEmpty = true;
                    errorHappened = true;
                }
                else
                {
                    var userTemp =  await _repos.GetUserByUserName(body.UserName);
                    if (userTemp != null)
                    {
                        userNameUnique = false;
                        errorHappened = true;
                    }
                }
                var result = new
                {
                    UserNameEmpty = userNameEmpty,
                    UserNameUnique = userNameUnique
                };
                if (errorHappened)
                    return Ok(result);

                var user = await _userManager.FindByEmailAsync(body.Email);
                if(user == null)
                {  
                    return NotFound(); 
                }
                if (user.Role.Equals("Gm"))
                {
                    var gm = await _repos.GetGmAsyncName(user.UserName);
                    gm.Name = body.UserName;
                }
                else
                {
                    if (user.Role.Equals("Player"))
                    {
                        var player = await _repos.GetPlayerAsyncName(user.UserName);
                        player.Name = body.UserName;
                    }
                }

                user.UserName = body.UserName;
                _repos.SaveContextChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
            
        }
        [HttpPatch("email")]
        public async Task<ActionResult> PatchEmail(PatchEmailBody body)
        {
            try
            {
                if (body.UserName == string.Empty)
                {
                    return BadRequest();
                }

                bool errorHappened = false;
                bool emailEmptyOrWrong = false;
                bool emailUnique = true;

                if (!(IsValidEmail(body.Email)))
                {
                    emailEmptyOrWrong = true;
                    errorHappened = true;
                }
                else
                {
                    var usersTemp = await _repos.GetUsersAsync();
                    var userTemp = usersTemp.FirstOrDefault(u => u.Email.Equals(body.Email));
                    if (userTemp != null)
                    {
                        emailUnique = false;
                        errorHappened = true;
                    }
                }
                var result = new
                {
                    EmailEmptyOrWrong = emailEmptyOrWrong,
                    EmailUnique = emailUnique,
                };
                if (errorHappened)
                    return Ok(result);

                var user = await _userManager.FindByNameAsync(body.UserName);
                if (user == null)
                {
                    return NotFound();
                }

                user.Email = body.Email;
                _repos.SaveContextChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPatch("profilePicture")]
        public async Task<ActionResult> PathProfilePicture(PatchProfilePicBody body)
        {
            try
            {
                bool wrongPicture= false;
                if (body.UserName == string.Empty)
                {
                    return BadRequest();
                }

                var user = await _userManager.FindByNameAsync(body.UserName);
                if (user == null)
                {
                    return NotFound();
                }

                if (body.ProfilePic.Equals(""))
                    wrongPicture = true;

                if (wrongPicture==true)
                {
                    var result = new
                    {
                        WrongPicture = wrongPicture,
                    };
                    return Ok(result);
                }

                var profilePicModified = body.ProfilePic.Replace("data:image/png;base64,", "");
                byte[] fileBytes = Convert.FromBase64String(profilePicModified);

                var filePath = user.ProfilePicturePath;
                System.IO.File.WriteAllBytes(filePath, fileBytes);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPatch("discord")]
        public async Task<ActionResult> PatchDiscord(PatchDiscordBody body)
        {
            try
            {
                if (body.UserName == string.Empty)
                {
                    return BadRequest();
                }

                if (String.IsNullOrEmpty(body.Discord)) 
                {
                    var response = new
                    {
                        EmptyDiscord = true
                    };
                    return Ok(response);
                }

                var user = await _userManager.FindByNameAsync(body.UserName);
                if (user == null)
                {
                    return NotFound();
                }

                user.Discord = body.Discord;
                _repos.SaveContextChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }

        
        [HttpPatch("password")]
        public async Task<ActionResult> PathPassword(PatchPasswordBody body)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(body.Email);
                if (user == null)
                    return BadRequest();
                try
                {
                    var token = await _userManager.GetAuthenticationTokenAsync(
                    user, "Application", "PasswordReset");
                    await _userManager.ResetPasswordAsync(user, token, body.NewPassword);
                    _repos.SaveContextChanges();
                    return Ok(new
                    {
                        PasswordIsCorrect = true
                    });
                }
                catch
                {
                    return Ok(new
                    {
                        PasswordIsCorrect = false
                    });
                }

            }
            catch
            {
                return BadRequest();
            }
            
        }
        [HttpPost("checkToken")]
        public async Task<ActionResult> CheckToken(PatchPasswordBody body)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(body.Email);
                if (user == null)
                    return BadRequest();
                var token = await _userManager.GetAuthenticationTokenAsync(
                    user, "Application", "PasswordReset");
                if (token == null)
                    return BadRequest();
                var shortCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(token)).Substring(0, 6);

                if (shortCode == body.ResetCode)
                {
                    try
                    {
                        return Ok(new
                        {
                            CodeIsCorrect = true
                        });
                    }
                    catch
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return Ok(new
                    {
                        CodeIsCorrect = false
                    });
                }

            }
            catch
            {
                return BadRequest();
            }
        }

        

        [HttpPost("sendMail")]
        public async Task<ActionResult> SendMail(MailModel body)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(body.EmailTo);
                if (body == null)
                    return BadRequest();

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.SetAuthenticationTokenAsync(
                    user,"Application", "PasswordReset", token);

                var shortCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(token)).Substring(0, 6);

                body.EmailBody = $"Your password reset code is: {shortCode}";
                body.EmailSubcject = "Password reset";

                _mailService.SendMailAsync(body);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("user")]
        public async Task<ActionResult> DeleteUser(string userName)
        {
            if (userName == string.Empty)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            var player = await _repos.GetPlayerAsync(user.GmOrPlayerId);
            if (player == null) 
            {
                var gm = await _repos.GetGmAsync(user.GmOrPlayerId);
                if(gm == null)
                {
                    return BadRequest();
                }
            }
            
            _repos.DeleteUser(userName);
            _repos.SaveContextChanges();

            return Ok();

        }
    }
}
