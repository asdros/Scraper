using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Scraper.Auth;
using Scraper.Models;
using Scraper.Services;

namespace Scraper.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private UserService _userService;
        private IMapper _mapper;

        public UsersController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]ApplicationUser model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]ApplicationUser model)
        {
            // map model to entity
            var user = _mapper.Map<Users>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        //    [HttpGet]
        //    public IActionResult GetAll()
        //    {
        //        var users = _userService.GetAll();
        //        var model = _mapper.Map<IList<UserModel>>(users);
        //        return Ok(model);
        //    }

        //    [HttpPut("{id}")]
        //    public IActionResult Update(int id, [FromBody]ApplicationUser model)
        //    {
        //        // map model to entity and set id
        //        var user = _mapper.Map<User>(model);
        //        user.Id = id;

        //        try
        //        {
        //            // update user 
        //            _userService.Update(user, model.Password);
        //            return Ok();
        //        }
        //        catch (AppException ex)
        //        {
        //            // return error message if there was an exception
        //            return BadRequest(new { message = ex.Message });
        //        }
        //    }

        //    [HttpDelete("{id}")]
        //    public IActionResult Delete(int id)
        //    {
        //        _userService.Delete(id);
        //        return Ok();
        //    }
        //}
    }
}