using ESCHOOL.IServices;
using ESCHOOL.Models;
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
    public class StudentprofileController : ControllerBase
    {


        private readonly IStudentprofileServices _studentprofileServices;
        public StudentprofileController(IStudentprofileServices studentprofileServices)
        {
            _studentprofileServices = studentprofileServices;
        }
        [HttpGet]
        public IEnumerable<Studentprofile> Gets()
        {
            return _studentprofileServices.Gets();
        }

        [HttpGet("{id}")]
        public IEnumerable<Studentprofile> Get(int id)
        {
            return _studentprofileServices.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Studentprofile entity)
        {
            await _studentprofileServices.Create(entity);
            return Ok(entity);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Studentprofile>> Update(int id, Studentprofile entity)
        {
            await _studentprofileServices.Update(id, entity);
            return Ok(entity);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _studentprofileServices.Delete(id);
            return Ok();
        }
    }
}
