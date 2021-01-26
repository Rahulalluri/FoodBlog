using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Connector.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace FoodBlog.App.Controller
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Rahul", LastName = "Alluri", Username = "Rahul_Alluri", Password = "test123", 
                Role = new UserRole("Admin", new List<string>(){ "CanAddUser", "CanDeleteUser"}) }
        };

        private readonly SecurityProfile _appSettings;

        public UserService(IOptions<SecurityProfile> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public IEnumerable<User> GetAll()
        {
            
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<int> AddUser(User user)
        {
            return default;
        }
    }
}
