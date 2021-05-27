using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Services;

namespace WSVenta.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] AuthRequest model)
        {
            Response response = new Response();
            var userreponse = _userService.Auth(model);
            if (userreponse == null)
            {
                response.Success = 0;
                response.Message = "Usuario o contraseña incorrecta";
                return BadRequest(response); 
            }
            response.Success = 1;
            response.Data = userreponse;
            return Ok(response);
        }
    }
}
