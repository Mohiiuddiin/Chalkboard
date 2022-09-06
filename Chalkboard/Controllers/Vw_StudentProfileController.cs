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
    public class Vw_StudentProfileController : ControllerBase
    {
        private IVw_StudentProfileServices _oVw_StudentProfileServices;

        public Vw_StudentProfileController(IVw_StudentProfileServices oVw_StudentProfileServices)
        {
            _oVw_StudentProfileServices = oVw_StudentProfileServices;
        }

        // GET: api/ExamResults
        [HttpGet]
        public IEnumerable<Vw_StudentProfile> Get()
        {
            return _oVw_StudentProfileServices.Gets();
        }

        // GET: api/ExamResults/5
        [HttpGet("{id}")]
        public IEnumerable<Vw_StudentProfile> Get(int id)
        {
            return _oVw_StudentProfileServices.Get(id);
        }

        // POST: api/ExamResults
        [HttpPost]
        public Vw_StudentProfile Post([FromBody] Vw_StudentProfile oExamResult)
        {
            //if (ModelState.IsValid) return _oExamResultService.Save(oExamResult);
            return null;
        }

        // PUT: api/ExamResults/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ExamResults/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //return _oExamResultService.Delete(id);
            return null;
        }

    }
}
