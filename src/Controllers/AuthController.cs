using AutoMapper;
using BachelorTherasoftDotnetApi.src.Dtos.Models;
using BachelorTherasoftDotnetApi.src.Models;
using Google.Apis.Auth;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace BachelorTherasoftDotnetApi.src.Controllers
{
    [Route("")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly SignInManager<User> _signInManager;
        public AuthController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
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

            Console.WriteLine("------------------- ------------------- WOWOWO SUCESS -------------------------------------- ");

            var payload = result.Principal;

            Console.WriteLine("IT WORKED I GUESS NOW DO STUFF TO SEE IF USER IS ALRIGHT LOGGED IN....");

            return Redirect("http://localhost:4200/");
        }
    }
}
