using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User = Connector.Model.User;

namespace FoodBlog.App.Controller
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);

        Task<int> AddUser(User user);
    }
}
