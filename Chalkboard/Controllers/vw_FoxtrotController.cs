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
    public class vw_FoxtrotController : ControllerBase
    {
        private Ivw_FoxtrotService _vw_FoxtrotService;

        public vw_FoxtrotController(Ivw_FoxtrotService vw_FoxtrotService)
        {
            _vw_FoxtrotService = vw_FoxtrotService;
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
            var users = _vw_FoxtrotService.GetById(id);
            return Ok(users);

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _vw_FoxtrotService.GetAll();
            return Ok(users);

        }


    }
}
