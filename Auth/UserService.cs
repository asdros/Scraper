﻿using System.Collections.Generic;
using System.Linq;

namespace Scraper.Auth
{
    public class UserService
    {
        private List<User> _users = new List<User>
        {
    #warning public user data
        new User {Username = "administrator", Password = "1234" }
        };

        public User Authenticate(string username, string password)
        {
            return _users.SingleOrDefault(x => x.Username == username && x.Password == password);
        }
    }
}