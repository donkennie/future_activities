using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Reactivities.API.DTOs;
using Reactivities.API.Services;
using Reactivities.Domain;
using System.Security.Claims;
using System.Text;

namespace Reactivities.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]

    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized("Invalid email");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                return new UserDTO
                {
                    DisplayName = user.DisplayName,
                    Image = null,
                    Token = _tokenService.CreateToken(user),
                    Username = user.UserName
                };
                // return CreateUserObject(user);
            }

            //  if (user.UserName == "bob") user.EmailConfirmed = true;

            //  if (!user.EmailConfirmed) return Unauthorized("Email not confirmed");

            return Unauthorized();
           // return Ok();
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email taken");
                //ModelState.AddModelError("email", "Email taken");
               // return ValidationProblem();
            }
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {

                return BadRequest("Username taken");
                //ModelState.AddModelError("username", "Username taken");
                //return ValidationProblem();
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
               /* return new UserDTO
                {
                    DisplayName = user.DisplayName,
                    Image = null,
                    Token = _tokenService.CreateToken(user),
                    Username = user.UserName
                };*/
            }

            //if (!result.Succeeded) return BadRequest("Problem registering user");

            //var origin = Request.Headers["origin"];
            // var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            //var verifyUrl = $"{origin}/account/verifyEmail?token={token}&email={user.Email}";
            // var message = $"<p>Please click the below link to verify your email address:</p><p><a href='{verifyUrl}'>Click to verify email</a></p>";

            // await _emailSender.SendEmailAsync(user.Email, "Please verify email", message);

            return BadRequest("Problem registering user");
            //return Ok("Registration success - please verify email");
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.Users//.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
        //    await SetRefreshToken(user);
            return CreateUserObject(user);
        }


        private UserDTO CreateUserObject(AppUser user)
        {
            return new UserDTO
            {
                DisplayName = user.DisplayName,
              //  Image = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }
    }
}
