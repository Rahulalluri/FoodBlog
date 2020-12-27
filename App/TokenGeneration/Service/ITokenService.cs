using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenGeneration.Model;

namespace TokenGeneration.Service
{
    public interface ITokenService
    {
        Task<string> GenerateTokenSceret();

        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    }
}
