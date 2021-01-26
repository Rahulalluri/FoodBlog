using Connector.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBlog.App.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        private readonly User _loggedUser;

        public UsersController(IUserService userService, HttpContext context)
        {
            _userService = userService;

            _loggedUser = (User)context.Items["User"];
        }



        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            if (_loggedUser.Role.Operations.TryGetValue("Admin", out IEnumerable<string> operations) && operations.Any())
            {
                var users = _userService.GetAll();
            }
            
            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            _ = user.Username ?? throw new ArgumentNullException(nameof(user.Username));
            _ = user.Email ?? throw new ArgumentNullException(nameof(user.Email));

            var id = await _userService.AddUser(user).ConfigureAwait(false);

            return Ok(id);
        }
    }
}
