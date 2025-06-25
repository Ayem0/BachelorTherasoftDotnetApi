using System.Text.Json;
using BachelorTherasoftDotnetApi.src.Models;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        /// <summary>
        /// Logout.
        /// </summary>
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("Api/[controller]/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            Response.Cookies?.Delete(".AspNetCore.Identity.Application");
            return Ok();
        }

        /// <summary>
        /// Login with google.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Api/[controller]/Login/Google")]
        public ChallengeResult LoginWithGoogle()
        {
            var props = _signInManager.ConfigureExternalAuthenticationProperties(
                GoogleOpenIdConnectDefaults.AuthenticationScheme,
                "http://localhost:8080/signin-google"
            );
            return Challenge(props, GoogleOpenIdConnectDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Login with google callback from google.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("signin-google")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithGoogleCallBack()
        {
            Console.WriteLine("------------------- ------------------- RECEIVED DATA FROM GOOGLE------------------- ------------------- ");

            var result = await HttpContext.AuthenticateAsync(GoogleOpenIdConnectDefaults.AuthenticationScheme);
            if (!result.Succeeded)
            {
                Console.WriteLine("------------------- ------------------- ERROR FROM GOOGLE :", result.Failure?.Message);
                return Unauthorized();
            }
            var principal = result.Principal;
            var email = principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                ?? principal.FindFirst("email")?.Value;
            var name = (principal.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value
                ?? principal.FindFirst("name")?.Value)?.Split(' ');
            var firstName = name?[0] ?? null;
            var lastName = name?[1] ?? null;
            if (email == null)
                return Unauthorized();
            User? user;
            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new() { UserName = email, Email = email, FirstName = firstName, LastName = lastName };
                await _userManager.CreateAsync(user);
            }

            await _signInManager.SignInAsync(user, true);
            return Redirect("http://localhost:4200/");
        }
    }
}
