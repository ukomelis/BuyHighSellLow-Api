using System;
using System.Threading.Tasks;
using BuyHighSellLow.Logic.Models.Requests;
using BuyHighSellLow.Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuyHighSellLow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// Returns user details
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        // GET: api/user/
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUser(string username)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var user = await _userService.GetUser(username);

                return Ok(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Signs in an user
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        // POST: api/user/signin
        [HttpPost]
        [AllowAnonymous]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserLoginRequest requestModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                return Ok(await _userService.SignIn(requestModel));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Signs out an user
        /// </summary>
        /// <returns></returns>
        // POST: api/user/signout
        [HttpPost]
        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                await _userService.SignOut();

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Registers an user
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        // POST: api/user/register
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Post([FromBody] UserDetailsRequest requestModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                await _userService.RegisterUser(requestModel);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Edits an user, if new password is specified it is changed
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        // POST: api/user/edit
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditUser([FromBody] UserDetailsRequest requestModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                await _userService.EditUser(requestModel);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }
    }
}