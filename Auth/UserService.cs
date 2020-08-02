using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Scraper.Services.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using Scraper.Services;
using Scraper.Models;
using Scraper.Services.ExecuteCommands;
using Dapper;
using System.Data.SqlClient;

namespace Scraper.Auth
{
    public class UserService
    {
        private readonly IConfiguration _configuration;
        private readonly IdentityCommands _identityCommands;
        private readonly string _connectionString;
        private readonly IExecuters _executers;
        
        public UserService(IConfiguration configuration, IdentityCommands identityCommands, IExecuters executers)
        {
            _configuration = configuration;
            _identityCommands = identityCommands;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }

        public Users Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            dynamic user = _executers.ExecuteCommand(_connectionString, connection =>
                connection.Query(_identityCommands.GetAll, new { @Username = username })).SingleOrDefault();

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            //casting dapperrow object type to internal model 
            Users users = new Users {Id=user.Id, UserName = user.UserName, PasswordHash = user.PasswordHash, PasswordSalt = user.PasswordSalt };

            return users;
        }



        public void Create(Users user, string password)
        {
            //// validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");
            
            
                var list = _executers.ExecuteCommand(_connectionString, connection =>
                connection.Query<string>(_identityCommands.GetUsernameColumn));

                if (list.Any(x => x == user.UserName))
                    throw new AppException("Username \"" + user.UserName + "\" is already taken");

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _executers.ExecuteCommand(_connectionString, connection=>
                connection.Query<Users>(_identityCommands.CreateUser, user));
            
        }



        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }



    }
}