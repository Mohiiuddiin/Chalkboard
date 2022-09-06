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
    public class Vw_TeacherNewController : ControllerBase
    {

        private IVw_TeacherNewServices _vw_TeacherNewServices;

        public Vw_TeacherNewController(IVw_TeacherNewServices studentsService)
        {
            _vw_TeacherNewServices = studentsService;
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
            var users = _vw_TeacherNewServices.GetById(id);
            return Ok(users);

        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _vw_TeacherNewServices.GetAll();
            return Ok(users);
        }















        //private IVw_TeacherNewServices _oVw_TeacherNewServices;

        //public Vw_TeacherNewController(IVw_TeacherNewServices oVw_TeacherNewServices)
        //{
        //    _oVw_TeacherNewServices = oVw_TeacherNewServices;
        //}

        //// GET: api/ Examss
        //[HttpGet]
        //public IEnumerable<Vw_TeacherNew> Get()
        //{
        //    return _oVw_TeacherNewServices.Gets();
        //}

        //// GET: api/ Examss/5
        //[HttpGet("{id}")]
        //public IEnumerable<Vw_TeacherNew> Get(int id)
        //{
        //    return _oVw_TeacherNewServices.Get(id);
        //}

        // POST: api/ Examss
        //[HttpPost]
        //public Exams Post([FromBody] Exams oExams)
        //{
        //    //if (ModelState.IsValid) return _o ExamsService.Save(o Exams);
        //    return null;
        //}

        //// PUT: api/ Examss/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ Examss/5
        //[HttpDelete("{id}")]
        //public string Delete(int id)
        //{
        //    //return _o ExamsService.Delete(id);
        //    return null;
        //}
    }
}
