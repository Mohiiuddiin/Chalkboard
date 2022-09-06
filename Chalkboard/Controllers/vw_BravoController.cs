using ESCHOOL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESCHOOL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vw_BravoController : ControllerBase
    {
        private Ivw_BravoServices _vw_BravoServices;

        public vw_BravoController(Ivw_BravoServices vw_BravoServices)
        {
            _vw_BravoServices = vw_BravoServices;
        }

        //[HttpPost("authenticate")]
        //public IActionResult Authenticate(AuthenticateRequest model)
        //{
        //    var response = _studentsService.Authenticate(model);

        //    if (response == null)
        //        return BadRequest(new { message = "Email or password is incorrect" });

        //    return Ok(response);
        //}
        [Authorize]
        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            var users = _vw_BravoServices.GetById(id);
            if (users.Count==0)
            {
                return Ok("Your search Id can not found");
            }
            else
            {
                return Ok(users);
            }


        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _vw_BravoServices.GetAll();
            return Ok(users);
        }
    }
}
