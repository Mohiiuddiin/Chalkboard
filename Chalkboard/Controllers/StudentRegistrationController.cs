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
    public class StudentRegistrationController : ControllerBase
    {


        private readonly IStudentRegistrationServices _studentRegistrationServices;
        public StudentRegistrationController(IStudentRegistrationServices studentRegistrationServices)
        {
            _studentRegistrationServices = studentRegistrationServices;
        }
        [HttpGet]
        public IEnumerable<StudentRegistration> Gets()
        {
            return _studentRegistrationServices.Gets();
        }

        [HttpGet("{id}")]
        public IEnumerable<StudentRegistration> Get(int id)
        {
            return _studentRegistrationServices.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] StudentRegistration entity)
        {
            await _studentRegistrationServices.Create(entity);
            return Ok(entity);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<StudentRegistration>> Update(int id, StudentRegistration entity)
        {
            await _studentRegistrationServices.Update(id, entity);
            return Ok(entity);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _studentRegistrationServices.Delete(id);
            return Ok();
        }
    }
}
