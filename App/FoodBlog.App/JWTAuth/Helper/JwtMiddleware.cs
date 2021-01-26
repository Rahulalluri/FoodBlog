using Connector.Model;
using FoodBlog.App.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityProfile = Connector.Model.SecurityProfile;

namespace FoodBlog.App.Controller
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SecurityProfile _security;

        public JwtMiddleware(RequestDelegate next, IOptions<SecurityProfile> appSettings)
        {
            _next = next;
            _security = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, userService, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_security.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero,

                    ValidIssuer = _security.Issuer,
                    ValidAudience = _security.Audience,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                var roles = JsonConvert.DeserializeObject(jwtToken.Claims.First(x => x.Type == nameof(User.Role)).Value);

                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetById(userId);
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(Errors.InvalidToken);
            }
        }
    }
}
