using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenGeneration.Model;
using TokenGeneration.Service;

namespace TokenGeneration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;

        private readonly ITokenService _service;

        public TokenController(ITokenService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> TokenSceret()
        {
            var sceret = await _service.GenerateTokenSceret().ConfigureAwait(false);

            return !string.IsNullOrEmpty(sceret) ? Ok(sceret) : (IActionResult)Unauthorized("Not AuthorizeAdmin");
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _service.Authenticate(model).ConfigureAwait(false);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}
