using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService) => _usersService = usersService;

        [HttpGet]
        [Route("Get", Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            IList<User>? users; 
            try
            {
                users = await _usersService.Get();                
                if (users.Count == 0)

                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            return Ok(users);
        }

        [HttpGet]
        [Route("GetById", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([Required] Guid userId)
        {
            User? user;
            try
            {
                user = await _usersService.Get(userId)!;
                if (user == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("GetByName", Name = "GetUserByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([Required] string forename, [Required] string surname)
        {
            IList<User>? users;
            try
            {
                users = await _usersService.Get(forename, surname)!;
                if (users == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            return Ok(users);
        }

        [HttpPost]
        [Route("AddUser", Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Insert(UserDto user)
        {
            User response;
            try
            {
                if(user.Forename == "string" || user.Surname == "string")
                {
                    return BadRequest();
                }
                response = await _usersService.Insert(user!);
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            return Ok(response);
        }
    }
}