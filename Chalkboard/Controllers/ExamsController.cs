using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESCHOOL.IServices;
using ESCHOOL.Models;
using ESCHOOL.Services;

namespace ESCHOOL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private IExamsServices _examsServices;

        public ExamsController(IExamsServices examsServices)
        {
            _examsServices = examsServices;
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
            var users = _examsServices.GetById(id);
            return Ok(users);

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _examsServices.GetAll();
            return Ok(users);

        }
    }
}
