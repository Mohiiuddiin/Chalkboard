using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChalkboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChalkboardAPI.Services;

namespace ChalkboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {

        private ITeachersServices _teachersServices;

        public TeachersController(ITeachersServices teachersService)
        {
            _teachersServices = teachersService;
        }

        //[Authorize]
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var users = _teachersServices.GetAll();
        //    return Ok(users);
        //}
        [Authorize]
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var users = _teachersServices.GetById(id);
            return Ok(users);

        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _teachersServices.GetAll();
            return Ok(users);
        }
    }
}
