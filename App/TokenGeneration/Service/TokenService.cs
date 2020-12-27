using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TokenGeneration.Model;
using TokenGeneration.Resources;

namespace TokenGeneration.Service
{
    public class TokenService : ITokenService
    {
        private readonly UserDetails _userDetails;

        private readonly SecurityProfile _securityProfile;

        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Rahul", LastName = "Alluri", Username = "Rahul_Alluri", Password = "test123" }
        };

        public TokenService(IOptions<UserDetails> options, IOptions<SecurityProfile> securityProfile)
        {
            _userDetails = options.Value;
            _securityProfile = securityProfile.Value;
        }

        public async Task<string> GenerateTokenSceret()
        {
            if (!isAdmin())
            {
                return string.Empty;
            }

            return await GenerateHash().ConfigureAwait(false);
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null)
                return null;

            try
            {
                // authentication successful so generate jwt token
                var token = await GenerateJwtToken(user).ConfigureAwait(false);
                return new AuthenticateResponse(user, token);
            }
            catch (Exception ex)
            {

                if (ex is UnauthorizedAccessException)
                {
                    return null;
                }

                throw new Exception();
            }
        }

        private bool isAdmin()
        {
            return _userDetails.UserName == DefaultConstants.Name && _userDetails.Email == DefaultConstants.Email;
        }

        private async Task<string> GenerateHash()
        {
            var key = new HMACSHA256();
            return Convert.ToBase64String(key.Key);
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            if (_securityProfile.Validity.AddDays(90).CompareTo(DateTime.Today) < 0)
            {
                throw new UnauthorizedAccessException(string.Format(Errors.JWTSecertExpired, _securityProfile.Validity.AddDays(90)));
            }
            // generate token that is valid for 30 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_securityProfile.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
