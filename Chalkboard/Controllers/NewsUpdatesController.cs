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
    public class NewsUpdatesController : ControllerBase
    {
        private INewsUpdatesServices _newsUpdatesServices;

        public NewsUpdatesController(INewsUpdatesServices newsUpdatesServices)
        {
            _newsUpdatesServices = newsUpdatesServices;
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
            var users = _newsUpdatesServices.GetById(id);
            return Ok(users);

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _newsUpdatesServices.GetAll();
            return Ok(users);

        }

        [Authorize]
        [HttpGet("GetNewsParamWiseData")]
        public IActionResult GetNewsParamWiseData(int studentId)
        {
            var users = _newsUpdatesServices.GetNewsParamWiseData(studentId);
            return Ok(users);

        }


    }
}
