﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBlog.App.JWTAuth
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
