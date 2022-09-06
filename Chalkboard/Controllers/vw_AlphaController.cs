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
    public class vw_AlphaController : ControllerBase
    {
        private Ivw_AlphaServices _vw_AlphaServices;

        public vw_AlphaController(Ivw_AlphaServices vw_AlphaServices)
        {
            _vw_AlphaServices = vw_AlphaServices;
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
        public IActionResult GetById(int id)
        {
            var users = _vw_AlphaServices.GetById(id);
            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _vw_AlphaServices.GetAll();
            return Ok(users);
        }


        [Authorize]
        [HttpGet("GetStudentTask")]
        public IActionResult GetStudentTask(int id)
        {
            var users = _vw_AlphaServices.GetStudentTaskData(id);
            return Ok(users);
        }
    }
}
