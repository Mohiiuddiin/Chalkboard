using ChalkboardAPI.Models;
using ChalkboardAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChalkboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private IStudentsService _studentsService;

        public StudentsController(IStudentsService studentsService)
        {
            _studentsService = studentsService;
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
            var users = _studentsService.GetById(id);
            return Ok(users);

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _studentsService.GetAll();
            return Ok(users);

        }


    }
}
