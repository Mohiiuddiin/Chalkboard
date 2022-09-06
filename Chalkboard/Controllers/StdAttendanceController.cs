using ESCHOOL.Models;
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
    public class StdAttendanceController : ControllerBase
    {
        private IStdAttendanceServices _stdAttendanceServices;

        public StdAttendanceController(IStdAttendanceServices stdAttendanceServices)
        {
            _stdAttendanceServices = stdAttendanceServices;
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
            var users = _stdAttendanceServices.GetById(id);

            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _stdAttendanceServices.GetAll();
            return Ok(users);
        }
    }
}
