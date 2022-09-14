using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ESCHOOL.IServices;
using ESCHOOL.Models;
using Chalkboard.Models.CustomModels;

namespace ESCHOOL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamResultController : ControllerBase
    {
        private IExamResultServices _oExamResultService;

        public ExamResultController(IExamResultServices oExamResultService)
        {
            _oExamResultService = oExamResultService;
        }

        // GET: api/ExamResults
        [HttpGet]
        public IEnumerable<ExamResult> Get()
        {
            return _oExamResultService.Gets();
        }

        // GET: api/ExamResults/5
        [HttpGet("{id}", Name = "Get114")]
        public IEnumerable<ExamResult> Get(int id)
        {
            return _oExamResultService.Get(id);
        }

        // POST: api/ExamResults
        [HttpPost]
        public ExamResult Post([FromBody] ExamResult oExamResult)
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

        [Authorize]
        [HttpGet("GetExamResult")]
        public IActionResult GetExamResult(int id)
        {
            Data users = _oExamResultService.GetExamResult(id);
            return Ok(users);
        }
    }
}
